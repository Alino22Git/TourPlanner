﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TourPlannerDAL;

#nullable disable

namespace TourPlannerDAL.Migrations
{
    [DbContext(typeof(TourPlannerDbContext))]
    partial class TourPlannerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Models.Tour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Distance")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("From")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Time")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("To")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TransportType")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("Models.TourLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("Date")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Difficulty")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Rating")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<double>("TotalDistance")
                        .HasColumnType("double precision");

                    b.Property<double>("TotalTime")
                        .HasColumnType("double precision");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.Property<string>("Weather")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("TourId");

                    b.ToTable("TourLogs");
                });

            modelBuilder.Entity("Models.TourLog", b =>
                {
                    b.HasOne("Models.Tour", "Tour")
                        .WithMany("TourLogs")
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tour");
                });

            modelBuilder.Entity("Models.Tour", b =>
                {
                    b.Navigation("TourLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
