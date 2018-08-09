using JerryPlat.DAL.Migrations;
using JerryPlat.Models;
using JerryPlat.Utils.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace JerryPlat.DAL.Context
{
    public class JerryPlatDbContext : DbContext
    {
        static JerryPlatDbContext()
        {
            using (JerryPlatDbContext context = new JerryPlatDbContext())
            {
                if (context.Database.Exists())
                {
                    Database.SetInitializer<JerryPlatDbContext>(null);
                    var migrator = new DbMigrator(new Configuration());
                    // if (doseed || !context.Database.CompatibleWithModel(false))
                    if (migrator.GetPendingMigrations().Any())
                        migrator.Update();
                }
                else
                {
                    Database.SetInitializer(new CreateAndMigrateDatabaseInitializer<Configuration>());
                }

                context.Database.Initialize(false);

                SystemConfigModel.Reset(context.SystemConfigs.ToDictionary(o => o.Name, o => o.Config));
            }
        }

        public JerryPlatDbContext() : base("name=JerryPlatDbContext")
        {
#if DEBUG
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Map();
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Navigation> Navigations { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionChapter> QuestionChapters { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionFavorite> QuestionFavorites { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionRecord> QuestionRecords { get; set; }
        public DbSet<AnswerRecord> AnswerRecords { get; set; }
        public DbSet<Exam> Exams { get; set; }

        public DbSet<OwinConfig> OwinConfigs { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<School> Schools { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberScoreHistory> MemberScoreHistories { get; set; }
        public DbSet<ScoreHistory> ScoreHistories { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Ground> Grounds { get; set; }
        public DbSet<Coach> Coaches { get; set; }

        public DbSet<WxTicket> WxTickets { get; set; }

        public DbSet<WithdrawType> WithdrawTypes { get; set; }
        public DbSet<Withdraw> Withdraws { get; set; }

        public DbSet<PayRecord> PayRecords { get; set; }
    }
}