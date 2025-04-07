﻿// <auto-generated />
using System;
using CityVilleDotnet.Api.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityVilleDotnet.Api.Common.Persistence.Migrations
{
    [DbContext(typeof(CityVilleDbContext))]
    [Migration("20250407183107_AddQuests")]
    partial class AddQuests
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CityVilleDotnet.Api.Common.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.QuestService.Domain.Quest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Complete")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "complete");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Progress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "progress");

                    b.Property<string>("Purchased")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "purchased");

                    b.Property<string>("QuestType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Quest");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.Commodities", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Commodities");

                    b.HasAnnotation("Relational:JsonPropertyName", "commodities");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.Inventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "count");

                    b.HasKey("Id");

                    b.ToTable("Inventory");

                    b.HasAnnotation("Relational:JsonPropertyName", "inventory");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.MapRect", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Height")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "height");

                    b.Property<int>("Width")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "width");

                    b.Property<Guid?>("WorldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("X")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "x");

                    b.Property<int>("Y")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "y");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("MapRect");

                    b.HasAnnotation("Relational:JsonPropertyName", "mapRects");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cash")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "cash");

                    b.Property<Guid?>("CommoditiesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Energy")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "energy");

                    b.Property<int>("EnergyMax")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "energyMax");

                    b.Property<int>("ExpansionsPurchased")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "expansionsPurchased");

                    b.Property<int>("Gold")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "gold");

                    b.Property<Guid?>("InventoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LastTrackingTimestamp")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "lastTrackingTimestamp");

                    b.Property<int>("Level")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "level");

                    b.Property<int>("RollCounter")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "rollCounter");

                    b.Property<int>("Uid")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "uid");

                    b.Property<int>("Xp")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "xp");

                    b.HasKey("Id");

                    b.HasIndex("CommoditiesId");

                    b.HasIndex("InventoryId");

                    b.ToTable("Player");

                    b.HasAnnotation("Relational:JsonPropertyName", "player");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserInfoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CreationTimestamp")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "creationTimestamp");

                    b.Property<bool>("FirstDay")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "firstDay");

                    b.Property<bool>("IsNew")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "is_new");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "username");

                    b.Property<Guid?>("WorldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WorldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "worldName");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("WorldId");

                    b.ToTable("UserInfo");

                    b.HasAnnotation("Relational:JsonPropertyName", "userInfo");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.World", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SizeX")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "sizeX");

                    b.Property<int>("SizeY")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "sizeY");

                    b.HasKey("Id");

                    b.ToTable("World");

                    b.HasAnnotation("Relational:JsonPropertyName", "world");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.WorldObject", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Builds")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "builds");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "className");

                    b.Property<string>("ContractName")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "contractName");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "deleted");

                    b.Property<int>("Direction")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "direction");

                    b.Property<int?>("FinishedBuilds")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "finishedBuilds");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "itemName");

                    b.Property<int?>("Stage")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "stage");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "state");

                    b.Property<string>("TargetBuildingClass")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "targetBuildingClass");

                    b.Property<string>("TargetBuildingName")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "targetBuildingName");

                    b.Property<int>("TempId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "tempId");

                    b.Property<int>("WorldFlatId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid?>("WorldId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("WorldObject");

                    b.HasAnnotation("Relational:JsonPropertyName", "objects");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.QuestService.Domain.Quest", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.User", null)
                        .WithMany("Quests")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.Commodities", b =>
                {
                    b.OwnsOne("CityVilleDotnet.Api.Services.UserService.Domain.Storage", "Storage", b1 =>
                        {
                            b1.Property<Guid>("CommoditiesId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Goods")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "goods");

                            b1.HasKey("CommoditiesId");

                            b1.ToTable("Commodities");

                            b1.HasAnnotation("Relational:JsonPropertyName", "storage");

                            b1.WithOwner()
                                .HasForeignKey("CommoditiesId");
                        });

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.MapRect", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.World", null)
                        .WithMany("MapRects")
                        .HasForeignKey("WorldId");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.Player", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.Commodities", "Commodities")
                        .WithMany()
                        .HasForeignKey("CommoditiesId");

                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryId");

                    b.OwnsOne("CityVilleDotnet.Api.Services.UserService.Domain.Options", "Options", b1 =>
                        {
                            b1.Property<Guid>("PlayerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("MusicDisabled")
                                .HasColumnType("bit")
                                .HasAnnotation("Relational:JsonPropertyName", "musicDisabled");

                            b1.Property<bool>("SfxDisabled")
                                .HasColumnType("bit")
                                .HasAnnotation("Relational:JsonPropertyName", "sfxDisabled");

                            b1.HasKey("PlayerId");

                            b1.ToTable("Player");

                            b1.HasAnnotation("Relational:JsonPropertyName", "options");

                            b1.WithOwner()
                                .HasForeignKey("PlayerId");
                        });

                    b.Navigation("Commodities");

                    b.Navigation("Inventory");

                    b.Navigation("Options");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.User", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Common.Domain.ApplicationUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoId");

                    b.Navigation("AppUser");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.UserInfo", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.World", "World")
                        .WithMany()
                        .HasForeignKey("WorldId");

                    b.Navigation("Player");

                    b.Navigation("World");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.World", b =>
                {
                    b.OwnsOne("CityVilleDotnet.Api.Services.UserService.Domain.CitySim", "CitySim", b1 =>
                        {
                            b1.Property<Guid>("WorldId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Population")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "population");

                            b1.Property<int>("PopulationCap")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "populationCap");

                            b1.Property<int>("PotentialPopulation")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "potentialPopulation");

                            b1.HasKey("WorldId");

                            b1.ToTable("World");

                            b1.HasAnnotation("Relational:JsonPropertyName", "citySim");

                            b1.WithOwner()
                                .HasForeignKey("WorldId");
                        });

                    b.Navigation("CitySim");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.WorldObject", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Services.UserService.Domain.World", null)
                        .WithMany("Objects")
                        .HasForeignKey("WorldId");

                    b.OwnsOne("CityVilleDotnet.Api.Services.UserService.Domain.WorldObjectPosition", "Position", b1 =>
                        {
                            b1.Property<Guid>("WorldObjectId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("X")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "x");

                            b1.Property<int>("Y")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "y");

                            b1.Property<int?>("Z")
                                .HasColumnType("int")
                                .HasAnnotation("Relational:JsonPropertyName", "z");

                            b1.HasKey("WorldObjectId");

                            b1.ToTable("WorldObject");

                            b1.HasAnnotation("Relational:JsonPropertyName", "position");

                            b1.WithOwner()
                                .HasForeignKey("WorldObjectId");
                        });

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Common.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Common.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CityVilleDotnet.Api.Common.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CityVilleDotnet.Api.Common.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.User", b =>
                {
                    b.Navigation("Quests");
                });

            modelBuilder.Entity("CityVilleDotnet.Api.Services.UserService.Domain.World", b =>
                {
                    b.Navigation("MapRects");

                    b.Navigation("Objects");
                });
#pragma warning restore 612, 618
        }
    }
}
