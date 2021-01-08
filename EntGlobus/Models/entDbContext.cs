using EntGlobus.Models.AnaliticaDbFolder;
using EntGlobus.Models.DbFolder1;
using EntGlobus.Models.SchoolDbFolder;
using EntGlobus.Models.NishDbFolder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntGlobus.Models.QR;

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


        public DbSet<LiveLesson> liveLessons { get; set; }
        public DbSet<PodLiveLesson> PodLiveLessons { get; set; }
        public DbSet<LiveChat> LiveChats { get; set; }
        public DbSet<PayLiveTest> PayLiveTests { get; set; }


        public DbSet<LiveTestVisitor> LiveTestVisitor { get; set; }

        /// <summary>
        ///                   Nish LiveLessons
        /// </summary>
        public DbSet<NishCourse> NishCourses { get; set; }
        public DbSet<NishPay> NishPays { get; set; }

        public DbSet<ForUserCallAnalitics> ForUserCallAnalitics { get; set; }


        /// <summary>
        ///               School live lessons
        /// </summary>
        public DbSet<School> GetSchools { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<ClassLesson> ClassLessons { get; set; }


        ////                    QR
        public DbSet<QrBook> QrBooks { get; set; }
        public DbSet<QrUserIdentity> QrUserIdentities { get; set; }
        public DbSet<QrVideo> QrVideos { get; set; }
        public DbSet<QrNuska> QrNuskas { get; set; }



        ///
        ///           Qiwi Pay Analitica
        ///
        public DbSet<QiwiAnalitic> QiwiAnalitics { get; set; }
    }
}
