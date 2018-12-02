﻿// <auto-generated />
using DESUBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DESUBot.Migrations
{
    [DbContext(typeof(Memestorage))]
    [Migration("20181202005318_MemestorageInitial")]
    partial class MemestorageInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("DESUBot.Data.MemeStoreModel", b =>
                {
                    b.Property<int>("MemeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<ulong>("AuthorID");

                    b.Property<string>("Content");

                    b.Property<string>("Date");

                    b.Property<int>("MemeUses");

                    b.Property<string>("Time");

                    b.Property<string>("Title");

                    b.HasKey("MemeId");

                    b.ToTable("Memestore");
                });
#pragma warning restore 612, 618
        }
    }
}
