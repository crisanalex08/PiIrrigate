﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PiIrrigateServer.Database;

#nullable disable

namespace PiIrrigateServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250512161636_AddSensorReadingsTable")]
    partial class AddSensorReadingsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PiIrrigateServer.Models.Device", b =>
                {
                    b.Property<string>("Mac")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("ZoneId")
                        .HasColumnType("uuid");

                    b.HasKey("Mac");

                    b.HasIndex("ZoneId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.SensorReading", b =>
                {
                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Humidity")
                        .HasColumnType("double precision");

                    b.Property<string>("Mac")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Rainfall")
                        .HasColumnType("double precision");

                    b.Property<double>("SoilMoisture")
                        .HasColumnType("double precision");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ZoneId")
                        .HasColumnType("uuid");

                    b.HasKey("Timestamp");

                    b.ToTable("SensorReadings");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.Zone", b =>
                {
                    b.Property<Guid>("ZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ZoneId");

                    b.HasIndex("UserId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.Device", b =>
                {
                    b.HasOne("PiIrrigateServer.Models.Zone", "Zone")
                        .WithMany("Devices")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Zone");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.Zone", b =>
                {
                    b.HasOne("PiIrrigateServer.Models.User", "User")
                        .WithMany("Zones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.User", b =>
                {
                    b.Navigation("Zones");
                });

            modelBuilder.Entity("PiIrrigateServer.Models.Zone", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
