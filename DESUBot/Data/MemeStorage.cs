using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DESUBot.Data
{
    public class Memestorage : DbContext
    {
        public DbSet<MemeStoreModel> Memestore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=F:\DESUBot\D-E-S-U-BOT\DESUBot\DESUDB.db");
   
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    } // ME TIRED I GO BED

    public class MemeStoreModel
    {
        [Key]
        public int MemeId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public ulong AuthorID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int MemeUses { get; set; }
    }

}



