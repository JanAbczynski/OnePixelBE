﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnePixelBE.EF;

namespace OnePixelBE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("OnePixelBE.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuildingNumber")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("LocalNumber")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("OnePixelBE.Models.AvaliblePermissionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.ToTable("AvaliblePermissionDetail");
                });

            modelBuilder.Entity("OnePixelBE.Models.AvaliblePermissions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("HasEnumDetail")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AvaliblePermissions");
                });

            modelBuilder.Entity("OnePixelBE.Models.Code", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("text");

                    b.Property<Guid?>("Beneficient")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ConnectedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateOfUsage")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("RawCode")
                        .HasColumnType("text");

                    b.Property<bool>("WasUsed")
                        .HasColumnType("boolean");

                    b.Property<int>("codeType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("OnePixelBE.Models.CrewStand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CrewStands");
                });

            modelBuilder.Entity("OnePixelBE.Models.FieldModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<int>("CoordinateX")
                        .HasColumnType("integer");

                    b.Property<int>("CoordinateY")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("FieldModels");
                });

            modelBuilder.Entity("OnePixelBE.Models.Logg", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("Controller")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("GuiltyUser")
                        .HasColumnType("uuid");

                    b.Property<string>("Info")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Loggs");
                });

            modelBuilder.Entity("OnePixelBE.Models.MenuAccessData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("SideMenuName")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MenuAccessDatas");
                });

            modelBuilder.Entity("OnePixelBE.Models.MenuPart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MainMenu")
                        .HasColumnType("text");

                    b.Property<string>("RouterLink")
                        .HasColumnType("text");

                    b.Property<string>("SubMenu")
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MenuParts");
                });

            modelBuilder.Entity("OnePixelBE.Models.OneRange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Distance")
                        .HasColumnType("integer");

                    b.Property<string>("GunsAsJson")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("NoOfTargets")
                        .HasColumnType("integer");

                    b.Property<Guid>("ShootingRangeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ShootingRangeId");

                    b.ToTable("OneRanges");
                });

            modelBuilder.Entity("OnePixelBE.Models.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("text");

                    b.Property<string>("DocumentParameter")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ExpireDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PermissionName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("OnePixelBE.Models.PointModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Special")
                        .HasColumnType("boolean");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("OnePixelBE.Models.ShootingRange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("ShootingRanges");
                });

            modelBuilder.Entity("OnePixelBE.Models.Target", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AttachmentFileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("PointsIdList")
                        .HasColumnType("text");

                    b.Property<string>("SizeListAsJSON")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentFileId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("OnePixelBE.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("BuildingNumber")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("LocalNumber")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Salt")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnePixelBE.Models.UserPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RawPermissionID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RawPermissionID");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("OnePixelBE.Models.UserPermissionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("RawPermissionDetailId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserPermissionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RawPermissionDetailId");

                    b.HasIndex("UserPermissionId");

                    b.ToTable("UserPermissionDetails");
                });

            modelBuilder.Entity("OnePixelBE.Models.WebFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateUTC")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<int>("FileStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Folder")
                        .HasColumnType("text");

                    b.Property<string>("NewName")
                        .HasColumnType("text");

                    b.Property<string>("OriginalName")
                        .HasColumnType("text");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("isTemp")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("WebFile");
                });

            modelBuilder.Entity("OnePixelBE.Models.AvaliblePermissionDetail", b =>
                {
                    b.HasOne("OnePixelBE.Models.AvaliblePermissions", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("OnePixelBE.Models.OneRange", b =>
                {
                    b.HasOne("OnePixelBE.Models.ShootingRange", null)
                        .WithMany("OneRange")
                        .HasForeignKey("ShootingRangeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnePixelBE.Models.Permission", b =>
                {
                    b.HasOne("OnePixelBE.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnePixelBE.Models.ShootingRange", b =>
                {
                    b.HasOne("OnePixelBE.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("OnePixelBE.Models.Target", b =>
                {
                    b.HasOne("OnePixelBE.Models.WebFile", "AttachmentFile")
                        .WithMany()
                        .HasForeignKey("AttachmentFileId");

                    b.Navigation("AttachmentFile");
                });

            modelBuilder.Entity("OnePixelBE.Models.UserPermission", b =>
                {
                    b.HasOne("OnePixelBE.Models.AvaliblePermissions", "RawPermission")
                        .WithMany()
                        .HasForeignKey("RawPermissionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnePixelBE.Models.User", null)
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RawPermission");
                });

            modelBuilder.Entity("OnePixelBE.Models.UserPermissionDetail", b =>
                {
                    b.HasOne("OnePixelBE.Models.AvaliblePermissionDetail", "RawPermissionDetail")
                        .WithMany()
                        .HasForeignKey("RawPermissionDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnePixelBE.Models.UserPermission", null)
                        .WithMany("UserPermissionDetail")
                        .HasForeignKey("UserPermissionId");

                    b.Navigation("RawPermissionDetail");
                });

            modelBuilder.Entity("OnePixelBE.Models.ShootingRange", b =>
                {
                    b.Navigation("OneRange");
                });

            modelBuilder.Entity("OnePixelBE.Models.User", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("OnePixelBE.Models.UserPermission", b =>
                {
                    b.Navigation("UserPermissionDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
