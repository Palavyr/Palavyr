﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Data.Migrations.ConvoMigrations
{
    [DbContext(typeof(ConvoContext))]
    [Migration("20201107043725_AddSubscriptionDetailsToAccountsTable")]
    partial class AddSubscriptionDetailsToAccountsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.Domain.conversations.CompletedConversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("AreaName")
                        .HasColumnType("text");

                    b.Property<string>("ConversationId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("EmailTemplateUsed")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("ResponsePdfId")
                        .HasColumnType("text");

                    b.Property<bool>("Seen")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("CompletedConversations");
                });

            modelBuilder.Entity("Server.Domain.conversations.ConversationUpdate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("text");

                    b.Property<string>("ConversationId")
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("NodeCritical")
                        .HasColumnType("boolean");

                    b.Property<string>("NodeId")
                        .HasColumnType("text");

                    b.Property<string>("NodeType")
                        .HasColumnType("text");

                    b.Property<string>("Prompt")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserResponse")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });
#pragma warning restore 612, 618
        }
    }
}
