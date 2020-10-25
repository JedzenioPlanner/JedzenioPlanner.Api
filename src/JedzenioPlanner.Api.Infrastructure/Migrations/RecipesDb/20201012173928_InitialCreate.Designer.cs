﻿// <auto-generated />
using System;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JedzenioPlanner.Api.Infrastructure.Migrations.RecipesDb
{
    [DbContext(typeof(RecipesDbContext))]
    [Migration("20201012173928_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes.RecipeDatabaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(3072);

                    b.Property<string>("Ingredients")
                        .HasColumnType("TEXT");

                    b.Property<string>("MealTypes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PictureUrl")
                        .HasColumnType("TEXT")
                        .HasMaxLength(2048);

                    b.Property<bool>("Removed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Steps")
                        .HasColumnType("TEXT");

                    b.Property<string>("Updates")
                        .HasColumnType("TEXT");

                    b.Property<double>("Version")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
