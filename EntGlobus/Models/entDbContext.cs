using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class entDbContext : IdentityDbContext<AppUsern>
    {

        public entDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUsern> Usernew { get; set; }
        public DbSet<Search> Searches { get; set; }
        public DbSet<Blok> Bloks { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Tolem> Tolems { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Ofpay> Ofpays { get; set; }
        public DbSet<Satilim> Satilims { get; set; }
        public DbSet<Qiwipay> Qiwipays { get; set; }
        public DbSet<Newqs> Newqs { get; set; }
        public DbSet<Dayliquestion> Dayliquestions { get; set; }
        public DbSet<Post>Posts { get; set; }
        public DbSet<SellBlok> SellBloks { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<Kurs> Kurs { get; set; }
        public DbSet<AllCourses> AllCourses { get; set; }
        public DbSet<Allowkurs> Allowkurs { get; set; }
        public DbSet<Reseption> Reseptions { get; set; }
        public DbSet<Su_ques_ph> Su_Ques_s { get; set; }
        public DbSet<Sublok> Subloks { get; set; }
        public DbSet<Suvariant> Suvariants { get; set; }
        public DbSet<Su_right_ans> Su_Right_Ans { get; set; }
        public DbSet<Phtest_pay> Phtest_Pays { get; set; }
    }
}
