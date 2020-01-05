﻿// <auto-generated />
using Counsel.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Counsel.Core.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Counsel.Core.Database.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.Property<string>("Team")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Counsel.Core.Database.Statistics", b =>
                {
                    b.Property<int>("Season")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Week")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OpponentTeam")
                        .HasColumnType("TEXT");

                    b.Property<double>("Points")
                        .HasColumnType("REAL");

                    b.Property<double>("ProjectedPoints")
                        .HasColumnType("REAL");

                    b.HasKey("Season", "Week", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("Counsel.Core.Database.Statistics", b =>
                {
                    b.HasOne("Counsel.Core.Database.Player", null)
                        .WithMany("Statistics")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}