﻿// <auto-generated />
using System;
using ChainMarketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChainMarketing.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250510133340_Role added")]
    partial class Roleadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChainMarketing.Domain.Entities.ReferralPath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReferrerId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReferrerId");

                    b.HasIndex("UserId");

                    b.ToTable("ReferralPaths");
                });

            modelBuilder.Entity("ChainMarketing.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasCoApplicant")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferralCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReferredById")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReferredById");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChainMarketing.Domain.Entities.ReferralPath", b =>
                {
                    b.HasOne("ChainMarketing.Domain.Entities.User", "Referrer")
                        .WithMany()
                        .HasForeignKey("ReferrerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ChainMarketing.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Referrer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChainMarketing.Domain.Entities.User", b =>
                {
                    b.HasOne("ChainMarketing.Domain.Entities.User", "ReferredBy")
                        .WithMany("DirectReferrals")
                        .HasForeignKey("ReferredById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ReferredBy");
                });

            modelBuilder.Entity("ChainMarketing.Domain.Entities.User", b =>
                {
                    b.Navigation("DirectReferrals");
                });
#pragma warning restore 612, 618
        }
    }
}
