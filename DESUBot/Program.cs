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

            _commands = new CommandService();
            _onMessage = new OnMessage(_client, _FaceAppClient);
            //_onExecutedCommand = new OnExecutedCommand(_client);          
            _botInitialization = new BotInitialization(_client);
            //_OnDeletedMessage = new OnDeletedMessage(_client);
            
            _services = new ServiceCollection()
                 .AddSingleton(_commands)
                 .AddSingleton(_FaceAppClient)
                 .AddSingleton(_client)
                 .BuildServiceProvider();

            _client.MessageReceived += _onMessage.MessageContainsAsync;
            _client.MessageUpdated += _onMessage.UpdatedMessageContainsAsync;         
            _client.UserJoined += UserHelp.UserJoined;
            _client.Ready += BotInitialization.StartUpMessages;
            //_client.MessageDeleted += _OnDeletedMessage.DeletedMessageStore;
             //_commands.CommandExecuted += _onExecutedCommand.AdminLog;
            _client.Log += Log;
            
            await RegisterCommandAsync();
            await _client.LoginAsync(TokenType.Bot, Config.BotToken);      
            await _client.StartAsync();
            //_DownloadDM = new DownloadDM(_client);
            await Task.Delay(-1);
        }

        private Task _client_MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(Config.CommandPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition:
                        if (result.ErrorReason != "DisableMessage")
                        {
                            await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetNoPerm());
                        }
                        break;
                    case CommandError.ParseFailed:
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetParseFailed());
                        break;
                    default:
                       Console.WriteLine(result.ErrorReason);
                        break;
                }                   
            }   
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
  
    }
}
