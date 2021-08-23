﻿// <auto-generated />
using System;
using ERPNextIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ERPNextIntegration.Migrations
{
    [DbContext(typeof(IntegrationDbContext))]
    [Migration("20210819205133_Customer relationship added")]
    partial class Customerrelationshipadded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "en_US.utf8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ERPNextIntegration.Dtos.QBO.entity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("errorContent")
                        .HasColumnType("text");

                    b.Property<DateTime>("lastUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("operation")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FailedQboWebhooks");
                });

            modelBuilder.Entity("ERPNextIntegration.IntegrationRelationships.CustomerAddressRelationship", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CustomerAddressRelationships");
                });

            modelBuilder.Entity("ERPNextIntegration.IntegrationRelationships.CustomerRelationship", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CustomerRelationships");
                });

            modelBuilder.Entity("ERPNextIntegration.IntegrationRelationships.ItemRelationship", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ItemRelationships");
                });

            modelBuilder.Entity("ERPNextIntegration.IntegrationRelationships.SalesInvoice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SalesInvoices");
                });
#pragma warning restore 612, 618
        }
    }
}
