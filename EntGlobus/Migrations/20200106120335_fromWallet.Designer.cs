﻿// <auto-generated />
using System;
using EntGlobus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntGlobus.Migrations
{
    [DbContext(typeof(entDbContext))]
    [Migration("20200106120335_fromWallet")]
    partial class fromWallet
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntGlobus.Models.AllCourses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("Id");

                    b.ToTable("AllCourses");
                });

            modelBuilder.Entity("EntGlobus.Models.Allowkurs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("Pan_Id");

                    b.Property<int>("Price");

                    b.Property<string>("UserPhone");

                    b.Property<bool>("pay");

                    b.HasKey("Id");

                    b.ToTable("Allowkurs");
                });

            modelBuilder.Entity("EntGlobus.Models.AppUsern", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("AuthType");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int?>("WalletPrice");

                    b.Property<bool>("enable");

                    b.Property<bool>("offenable");

                    b.Property<string>("pan1");

                    b.Property<string>("pan2");

                    b.Property<DateTime>("regdate");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EntGlobus.Models.Audio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Blok");

                    b.Property<string>("Url");

                    b.Property<string>("Variant");

                    b.HasKey("Id");

                    b.ToTable("Audios");
                });

            modelBuilder.Entity("EntGlobus.Models.Blok", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BuyDate");

                    b.Property<string>("IdentityId");

                    b.Property<string>("blok");

                    b.Property<bool>("enable");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Bloks");
                });

            modelBuilder.Entity("EntGlobus.Models.Dayliquestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("A1");

                    b.Property<int>("A2");

                    b.Property<int>("A3");

                    b.Property<int>("A4");

                    b.Property<int>("A5");

                    b.Property<string>("Correctans");

                    b.Property<string>("ans1");

                    b.Property<string>("ans2");

                    b.Property<string>("ans3");

                    b.Property<string>("ans4");

                    b.Property<string>("ans5");

                    b.Property<DateTime>("qstime");

                    b.Property<string>("question");

                    b.Property<string>("subject");

                    b.HasKey("Id");

                    b.ToTable("Dayliquestions");
                });

            modelBuilder.Entity("EntGlobus.Models.Kurs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("course_id");

                    b.Property<string>("mat1");

                    b.Property<string>("mat2");

                    b.Property<string>("mat3");

                    b.Property<string>("mat4");

                    b.Property<string>("photo");

                    b.Property<string>("subject");

                    b.Property<string>("text");

                    b.Property<string>("time");

                    b.Property<string>("video");

                    b.Property<string>("watch");

                    b.HasKey("Id");

                    b.ToTable("Kurs");
                });

            modelBuilder.Entity("EntGlobus.Models.LiveChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("MessDate");

                    b.Property<string>("Message");

                    b.Property<string>("Number");

                    b.Property<Guid>("PodLiveLessonId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("PodLiveLessonId");

                    b.ToTable("LiveChats");
                });

            modelBuilder.Entity("EntGlobus.Models.LiveLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icon");

                    b.Property<string>("Information");

                    b.Property<string>("Name");

                    b.Property<string>("Photo");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("liveLessons");
                });

            modelBuilder.Entity("EntGlobus.Models.Newqs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Check");

                    b.Property<string>("Number");

                    b.Property<DateTime>("Offerdate");

                    b.Property<string>("Question");

                    b.Property<string>("Subject");

                    b.Property<string>("Uriphoto");

                    b.HasKey("Id");

                    b.ToTable("Newqs");
                });

            modelBuilder.Entity("EntGlobus.Models.Ofpay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdentityId");

                    b.Property<string>("Price");

                    b.Property<string>("type");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Ofpays");
                });

            modelBuilder.Entity("EntGlobus.Models.Phtest_pay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number");

                    b.Property<double>("Price");

                    b.Property<string>("Type");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("Id");

                    b.ToTable("Phtest_Pays");
                });

            modelBuilder.Entity("EntGlobus.Models.PodLiveLesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LiveLessonId");

                    b.Property<string>("Nuska");

                    b.Property<DateTime?>("StartDate");

                    b.Property<bool?>("Status");

                    b.Property<int?>("TypeVideo");

                    b.Property<string>("UrlPhoto");

                    b.Property<string>("UrlVideo");

                    b.HasKey("Id");

                    b.HasIndex("LiveLessonId");

                    b.ToTable("PodLiveLessons");
                });

            modelBuilder.Entity("EntGlobus.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("date");

                    b.Property<string>("hashtag");

                    b.Property<string>("pathimg");

                    b.Property<string>("pathvideo");

                    b.Property<string>("subject");

                    b.Property<string>("text");

                    b.Property<int>("watch");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EntGlobus.Models.Qiwipay", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("account");

                    b.Property<string>("number");

                    b.Property<bool>("pan");

                    b.Property<bool>("pay");

                    b.Property<string>("prv_txn");

                    b.Property<double>("sum");

                    b.Property<DateTime>("txn_date");

                    b.Property<string>("txn_id");

                    b.Property<string>("type");

                    b.HasKey("id");

                    b.ToTable("Qiwipays");
                });

            modelBuilder.Entity("EntGlobus.Models.Reseption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Reseptions");
                });

            modelBuilder.Entity("EntGlobus.Models.Satilim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Satilims");
                });

            modelBuilder.Entity("EntGlobus.Models.Search", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdentityId");

                    b.Property<int>("count");

                    b.Property<DateTime>("date");

                    b.Property<bool>("enable");

                    b.Property<bool>("pay");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Searches");
                });

            modelBuilder.Entity("EntGlobus.Models.SellBlok", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Enable");

                    b.Property<string>("Imgurl");

                    b.Property<string>("Name");

                    b.Property<string>("Typi");

                    b.Property<string>("Variants");

                    b.HasKey("Id");

                    b.ToTable("SellBloks");
                });

            modelBuilder.Entity("EntGlobus.Models.Su_ques_ph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Path_1");

                    b.Property<string>("Path_2");

                    b.Property<string>("Path_3");

                    b.Property<string>("Path_4");

                    b.Property<string>("Path_5");

                    b.Property<string>("Path_6");

                    b.Property<string>("Path_7");

                    b.Property<int>("Subject_id");

                    b.Property<int>("Variant_id");

                    b.HasKey("Id");

                    b.ToTable("Su_Ques_s");
                });

            modelBuilder.Entity("EntGlobus.Models.Su_right_ans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Ques_id");

                    b.Property<string>("Right_ans");

                    b.Property<int>("Subject_id");

                    b.Property<int>("Variant_id");

                    b.HasKey("Id");

                    b.ToTable("Su_Right_Ans");
                });

            modelBuilder.Entity("EntGlobus.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("basic");

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("EntGlobus.Models.Sublok", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Subloks");
                });

            modelBuilder.Entity("EntGlobus.Models.Suvariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Blok_id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Suvariants");
                });

            modelBuilder.Entity("EntGlobus.Models.Tolem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdentityId");

                    b.Property<DateTime>("date");

                    b.Property<string>("price");

                    b.Property<bool>("success");

                    b.Property<string>("type");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Tolems");
                });

            modelBuilder.Entity("EntGlobus.Models.UserStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CheckDate");

                    b.Property<string>("Comment");

                    b.Property<string>("Number");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("UserStatuses");
                });

            modelBuilder.Entity("EntGlobus.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("blok");

                    b.Property<string>("url");

                    b.Property<string>("variant");

                    b.HasKey("Id");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EntGlobus.Models.Blok", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("EntGlobus.Models.LiveChat", b =>
                {
                    b.HasOne("EntGlobus.Models.PodLiveLesson", "PodLiveLesson")
                        .WithMany()
                        .HasForeignKey("PodLiveLessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EntGlobus.Models.Ofpay", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("EntGlobus.Models.PodLiveLesson", b =>
                {
                    b.HasOne("EntGlobus.Models.LiveLesson", "LiveLesson")
                        .WithMany()
                        .HasForeignKey("LiveLessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EntGlobus.Models.Search", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern", "Identity")
                        .WithMany("Searches")
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("EntGlobus.Models.Tolem", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EntGlobus.Models.AppUsern")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EntGlobus.Models.AppUsern")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
