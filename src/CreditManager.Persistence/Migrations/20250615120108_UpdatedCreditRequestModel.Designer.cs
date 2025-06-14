﻿// <auto-generated />
using System;
using CreditManager.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CreditManager.Persistence.Migrations
{
    [DbContext(typeof(CreditManagerDbContext))]
    [Migration("20250615120108_UpdatedCreditRequestModel")]
    partial class UpdatedCreditRequestModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("creditmanager")
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CreditManager.Domain.Entities.Audit.TransAuditE", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<Guid>("TransAuditHId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransAuditHId");

                    b.ToTable("TransAuditEs", "creditmanager");
                });

            modelBuilder.Entity("CreditManager.Domain.Entities.Audit.TransAuditH", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Machine")
                        .HasColumnType("text");

                    b.Property<string>("OsUser")
                        .HasColumnType("text");

                    b.Property<string>("PrimKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TranType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TransDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransAuditHs", "creditmanager");
                });

            modelBuilder.Entity("CreditManager.Domain.Entities.Credit.CreditRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ApprovedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Comments")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<int>("CreditType")
                        .HasColumnType("integer");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PeriodDays")
                        .HasColumnType("integer");

                    b.Property<int>("PeriodMonths")
                        .HasColumnType("integer");

                    b.Property<int>("PeriodYears")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CreditRequests", "creditmanager");
                });

            modelBuilder.Entity("CreditManager.Domain.Entities.Identity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", "creditmanager");
                });

            modelBuilder.Entity("CreditManager.Domain.Entities.Audit.TransAuditE", b =>
                {
                    b.HasOne("CreditManager.Domain.Entities.Audit.TransAuditH", "TransAuditH")
                        .WithMany("TransAuditEs")
                        .HasForeignKey("TransAuditHId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransAuditH");
                });

            modelBuilder.Entity("CreditManager.Domain.Entities.Audit.TransAuditH", b =>
                {
                    b.Navigation("TransAuditEs");
                });
#pragma warning restore 612, 618
        }
    }
}
