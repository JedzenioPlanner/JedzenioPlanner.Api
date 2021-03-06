﻿// <auto-generated />
using System;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JedzenioPlanner.Api.Infrastructure.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    [Migration("20201010150208_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("JedzenioPlanner.Api.Infrastructure.Persistence.EventStore.EventDatabaseEntity", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<double>("EntityVersion")
                        .HasColumnType("REAL");

                    b.Property<string>("EventDetails")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EventPublished")
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId", "EntityVersion");

                    b.ToTable("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
