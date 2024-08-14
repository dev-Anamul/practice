﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230614133700_InitialCreate.RecoveryRequests")]
    partial class InitialCreateRecoveryRequests
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Property<int>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Oid"), 1L, 1);

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<string>("ISOCodeAlpha2")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Domain.Entities.District", b =>
                {
                    b.Property<int>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Oid"), 1L, 1);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<string>("DistrictCode")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Domain.Entities.Facility", b =>
                {
                    b.Property<int>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Oid"), 1L, 1);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("HMISCode")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPrivateFacility")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<string>("Latitude")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Longitude")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.HasIndex("DistrictId");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("Domain.Entities.FacilityAccess", b =>
                {
                    b.Property<Guid>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateApproved")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("FacilityId")
                        .HasColumnType("int");

                    b.Property<bool>("ForgotPassword")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIgnored")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Oid");

                    b.HasIndex("FacilityId");

                    b.HasIndex("UserAccountId");

                    b.ToTable("FacilityAccesses");
                });

            modelBuilder.Entity("Domain.Entities.ModuleAccess", b =>
                {
                    b.Property<Guid>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<Guid>("FacilityAccessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.Property<int>("ModuleCode")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.HasIndex("FacilityAccessId");

                    b.ToTable("ModuleAccesses");
                });

            modelBuilder.Entity("Domain.Entities.Province", b =>
                {
                    b.Property<int>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Oid"), 1L, 1);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("Domain.Entities.RecoveryRequest", b =>
                {
                    b.Property<Guid>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cellphone")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("smalldatetime");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRequestOpen")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Oid");

                    b.ToTable("RecoveryRequests");
                });

            modelBuilder.Entity("Domain.Entities.Town", b =>
                {
                    b.Property<int>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Oid"), 1L, 1);

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.HasKey("Oid");

                    b.HasIndex("DistrictId");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("Domain.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Oid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("ContactAddress")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CreatedIn")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("IsAccountActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSynced")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ModifiedIn")
                        .HasColumnType("int");

                    b.Property<string>("NRC")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<bool>("NoNRC")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Sex")
                        .HasColumnType("tinyint");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<byte>("UserType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Oid");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Domain.Entities.District", b =>
                {
                    b.HasOne("Domain.Entities.Province", "Provinces")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provinces");
                });

            modelBuilder.Entity("Domain.Entities.Facility", b =>
                {
                    b.HasOne("Domain.Entities.District", "Districts")
                        .WithMany("Facilities")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Districts");
                });

            modelBuilder.Entity("Domain.Entities.FacilityAccess", b =>
                {
                    b.HasOne("Domain.Entities.Facility", "Facility")
                        .WithMany("FacilityAccesses")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.UserAccount", "UserAccount")
                        .WithMany("FacilityAccesses")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("Domain.Entities.ModuleAccess", b =>
                {
                    b.HasOne("Domain.Entities.FacilityAccess", "FacilityAccess")
                        .WithMany("ModuleAccesses")
                        .HasForeignKey("FacilityAccessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FacilityAccess");
                });

            modelBuilder.Entity("Domain.Entities.Town", b =>
                {
                    b.HasOne("Domain.Entities.District", "District")
                        .WithMany("Towns")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Domain.Entities.District", b =>
                {
                    b.Navigation("Facilities");

                    b.Navigation("Towns");
                });

            modelBuilder.Entity("Domain.Entities.Facility", b =>
                {
                    b.Navigation("FacilityAccesses");
                });

            modelBuilder.Entity("Domain.Entities.FacilityAccess", b =>
                {
                    b.Navigation("ModuleAccesses");
                });

            modelBuilder.Entity("Domain.Entities.Province", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("Domain.Entities.UserAccount", b =>
                {
                    b.Navigation("FacilityAccesses");
                });
#pragma warning restore 612, 618
        }
    }
}
