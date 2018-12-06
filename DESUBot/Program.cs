using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DESUBot.Modules;
using DESUBot.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using FaceApp;
using System.Net.Http;
using DESUBot.Logging;

namespace DESUBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            var bot = program.RunBotAsync();
            bot.Wait();   
        }

        public DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private OnMessage _onMessage;
        private FaceAppClient _FaceAppClient;
        private BotInitialization _botInitialization;

        public async Task RunBotAsync()
        {
            var _HttpClient = new HttpClient();           
            _FaceAppClient = new FaceAppClient(_HttpClient);

            DiscordSocketConfig botConfig = new DiscordSocketConfig()
            {
                MessageCacheSize = 5000
            };
            _client = new DiscordSocketClient(botConfig);

            var commandServiceConfig = new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async 
            };

            _commands = new CommandService(commandServiceConfig);
            _onMessage = new OnMessage(_client, _FaceAppClient);          
            _botInitialization = new BotInitialization(_client);
            
            _services = new ServiceCollection()
                 .AddSingleton(_commands)
                 .AddSingleton(_FaceAppClient)
                 .AddSingleton(_client)
                 .BuildServiceProvider();

            _client.MessageUpdated += _onMessage.UpdatedMessageContainsAsync;         
            _client.UserJoined += UserHelp.UserJoined;
            _client.Ready += BotInitialization.StartUpMessages;
            _client.Log += Log;
            
            await RegisterCommandAsync();
            await _client.LoginAsync(TokenType.Bot, Config.BotToken);      
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {

           _ = Task.Run(() => _onMessage.MessageContainsAsync(arg));

            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(Config.CommandPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                if (BlacklistedUser.BlackListedUser.Contains(context.Message.Author)) return;

                var result = await _commands.ExecuteAsync(context, argPos, _services);
                await ConsoleLog.LogToConsole(result, context);
            }   
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
  
    }
}
