using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using System;
using System.Data.Entity.Migrations;

namespace JerryPlat.DAL.Context
{
    public class CreateAndMigrateDatabaseInitializer<TConfiguration> : CreateAndMigrateDatabaseInitializer<JerryPlatDbContext, TConfiguration>
         where TConfiguration : DbMigrationsConfiguration<JerryPlatDbContext>, new()
    {
        protected override void Seed(JerryPlatDbContext context)
        {
            int intId = 1;

            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = 1, PageName = "通用管理", PageUrl = "", ParentId = 0, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 2, PageName = "Banner图管理", PageUrl = "/Admin/Banner", ParentId = 1, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 3, PageName = "优杰新闻", PageUrl = "/Admin/Article", ParentId = 1, OrderIndex = 2, SiteType = SiteType.Admin },
                new Navigation { Id = 4, PageName = "科目管理", PageUrl = "/Admin/Course", ParentId = 1, OrderIndex = 3, SiteType = SiteType.Admin },
                new Navigation { Id = 5, PageName = "城市管理", PageUrl = "/Admin/City", ParentId = 1, OrderIndex = 4, SiteType = SiteType.Admin },
                new Navigation { Id = 6, PageName = "学校管理", PageUrl = "/Admin/School", ParentId = 1, OrderIndex = 5, SiteType = SiteType.Admin },
                new Navigation { Id = 7, PageName = "场地管理", PageUrl = "/Admin/Ground", ParentId = 1, OrderIndex = 6, SiteType = SiteType.Admin },
                new Navigation { Id = 8, PageName = "教练管理", PageUrl = "/Admin/Coach", ParentId = 1, OrderIndex = 7, SiteType = SiteType.Admin },

                new Navigation { Id = 9, PageName = "题库管理", PageUrl = "", ParentId = 0, OrderIndex = 2, SiteType = SiteType.Admin },
                new Navigation { Id = 10, PageName = "题库类别", PageUrl = "/Admin/QuestionType", ParentId = 9, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 11, PageName = "题库管理", PageUrl = "/Admin/Question", ParentId = 9, OrderIndex = 2, SiteType = SiteType.Admin },

                new Navigation { Id = 12, PageName = "学员管理", PageUrl = "", ParentId = 0, OrderIndex = 3, SiteType = SiteType.Admin },
                new Navigation { Id = 13, PageName = "学员管理", PageUrl = "/Admin/Member", ParentId = 12, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 14, PageName = "报名管理", PageUrl = "/Admin/Enroll", ParentId = 12, OrderIndex = 2, SiteType = SiteType.Admin },
                new Navigation { Id = 15, PageName = "预约管理", PageUrl = "/Admin/Subscribe", ParentId = 12, OrderIndex = 3, SiteType = SiteType.Admin },

                new Navigation { Id = 16, PageName = "统计管理", PageUrl = "", ParentId = 0, OrderIndex = 4, SiteType = SiteType.Admin },
                new Navigation { Id = 17, PageName = "资金统计", PageUrl = "/Admin/Report", ParentId = 16, OrderIndex = 1, SiteType = SiteType.Admin },

                new Navigation { Id = 18, PageName = "提现管理", PageUrl = "", ParentId = 0, OrderIndex = 5, SiteType = SiteType.Admin },
                new Navigation { Id = 19, PageName = "提现类别", PageUrl = "/Admin/WithdrawType", ParentId = 18, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 20, PageName = "提现管理", PageUrl = "/Admin/Withdraw", ParentId = 18, OrderIndex = 2, SiteType = SiteType.Admin },

                new Navigation { Id = 21, PageName = "权限管理", PageUrl = "", ParentId = 0, OrderIndex = 6, SiteType = SiteType.Admin },
                new Navigation { Id = 22, PageName = "角色管理", PageUrl = "/Admin/Group", ParentId = 21, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 23, PageName = "用户管理", PageUrl = "/Admin/User", ParentId = 21, OrderIndex = 2, SiteType = SiteType.Admin },

                new Navigation { Id = 24, PageName = "系统设置", PageUrl = "", ParentId = 0, OrderIndex = 7, SiteType = SiteType.Admin },
                new Navigation { Id = 25, PageName = "系统设置", PageUrl = "/Admin/SystemConfig", ParentId = 24, OrderIndex = 1, SiteType = SiteType.Admin },
                new Navigation { Id = 26, PageName = "开放授权", PageUrl = "/Admin/OwinConfig", ParentId = 24, OrderIndex = 2, SiteType = SiteType.Admin },
            });

            intId = 1;
            context.Groups.AddOrUpdate(new Group[]
            {
                new Group {Id=intId++, Name="报名点" },
                new Group {Id=intId++, Name="系统管理员" }
            });

            intId = 1;
            context.Roles.AddOrUpdate(new Role[]
            {
                new Role {Id= intId++, GroupId = 1, NavigationId = 14 },
                new Role {Id= intId++, GroupId = 1, NavigationId = 15 },
                new Role {Id= intId++, GroupId = 1, NavigationId = 16 },
                new Role {Id= intId++, GroupId = 1, NavigationId = 17 },

                new Role {Id= intId++, GroupId = 2, NavigationId = 1 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 2 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 3 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 4 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 5 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 6 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 7 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 8 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 9 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 10 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 11 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 12 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 13 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 14 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 15 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 16 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 17 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 18 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 19 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 20 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 21 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 22 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 23 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 24 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 25 },
                new Role {Id= intId++, GroupId = 2, NavigationId = 26 }
            });

            intId = 1;
            context.AdminUsers.AddOrUpdate(new AdminUser[] {
                new AdminUser { Id = intId++, UserName = "admin", Password = EncryptHelper.Encrypt("admin"), GroupId = 0 }
            });

            intId = 1;
            context.QuestionTypes.AddOrUpdate(new QuestionType[]
            {
                new QuestionType {Id = intId++, Name="科目一：理论考试", Description = "小车理论考试", Source="https://api2.jiakaobaodian.com/api/open/chapter/list.htm?_appName=jiakaobaodian&_platform=wap&_r=111120180392687420072&carType=car&course=kemu1&limit=5", OrderIndex = 1 },
                new QuestionType {Id = intId++, Name="科目四：理论考试", Description = "小车理论考试", Source="https://api2.jiakaobaodian.com/api/open/chapter/list.htm?_appName=jiakaobaodian&_platform=wap&_r=111120180392687420072&carType=car&course=kemu3&limit=5", OrderIndex = 2 }
            });

            intId = 1;
            context.SystemConfigs.AddOrUpdate(new SystemConfig[] {
                new SystemConfig {Id=intId++, Name="DefaultPassword", Config="123456" },
                new SystemConfig {Id=intId++, Name="IsUseVerifyCode", Config="true" },
                new SystemConfig {Id=intId++, Name="IsUseSms", Config="false" },
                new SystemConfig {Id=intId++, Name="TaxPercentage", Config="0.3" },

                new SystemConfig {Id=intId++, Name="TopLeftName", Config="习题考试" },
                new SystemConfig {Id=intId++, Name="TopLeftUrl", Config="/Mob/Question" },
                new SystemConfig {Id=intId++, Name="TopRightName", Config="考场分布" },
                new SystemConfig {Id=intId++, Name="TopRightUrl", Config="/Mob/Article/TopOne/3" },
                new SystemConfig {Id=intId++, Name="BottomLeftName", Config="学车流程" },
                new SystemConfig {Id=intId++, Name="BottomLeftUrl", Config="/Mob/Article/TopOne/4" },
                new SystemConfig {Id=intId++, Name="BottomRightName", Config="考试预约" },
                new SystemConfig {Id=intId++, Name="BottomRightUrl", Config="/Mob/Article/TopOne/5" },

                new SystemConfig {Id=intId++, Name="SmsGateway", Config="http://dx.ipyy.net/smsJson.aspx" },
                new SystemConfig {Id=intId++, Name="SmsAccount", Config="" },
                new SystemConfig {Id=intId++, Name="SmsPassword", Config="" },
                new SystemConfig {Id=intId++, Name="SmsSignature", Config="【优杰学车】" },
                new SystemConfig {Id=intId++, Name="SmsCodeTemplate", Config="您的验证码是{{Code}}。" },
                new SystemConfig {Id=intId++, Name="SmsPayTemplate", Config="您的提现申请已通过，{{Amount}}元已通过{{WithdrawType}}方式转入您帐号，请查收。" },

                new SystemConfig {Id=intId++, Name="RefereeScore", Config="500" },
                new SystemConfig {Id=intId++, Name="RefereeScoreDescription", Config="一级推荐优币（以缴费为准）" },
                new SystemConfig {Id=intId++, Name="ParentRefereeScore", Config="200" },
                new SystemConfig {Id=intId++, Name="ParentRefereeScoreDescription", Config="二级推荐优币（以缴费为准）" },

                new SystemConfig {Id=intId++, Name="FirstCount", Config="10" },
                new SystemConfig {Id=intId++, Name="FirstScore", Config="100" },
                new SystemConfig {Id=intId++, Name="FirstDescription", Config="一级推荐人数获得优币" },
                new SystemConfig {Id=intId++, Name="SecondCount", Config="30" },
                new SystemConfig {Id=intId++, Name="SecondScore", Config="50" },
                new SystemConfig {Id=intId++, Name="SecondDescription", Config="二级推荐人数获得优币" },

                new SystemConfig {Id=intId++, Name="MatchCount", Config="10" },
                new SystemConfig {Id=intId++, Name="MatchScore", Config="50" },
                new SystemConfig {Id=intId++, Name="MatchDescription", Config="比赛获得优币" },
                new SystemConfig {Id=intId++, Name="GradePercentage", Config="0.6+0.3+0.1" },

                new SystemConfig {Id=intId++, Name="ShareTitle", Config="优杰学车-分享赢优币" },
                new SystemConfig {Id=intId++, Name="ShareContent", Config="优杰学车-分享赢优币" },
            });

            intId = 1;
            context.OwinConfigs.AddOrUpdate(new OwinConfig[]
            {
                new OwinConfig {
                    Id = intId++,
                    Name ="Wechat",
                    AppId ="",
                    AppSecret ="",
                    RequestUri ="https://open.weixin.qq.com/connect/oauth2/authorize?appid={{AppId}}&redirect_uri={{RedirectUri}}&response_type=code&scope=snsapi_userinfo&state={{State}}#wechat_redirect",
                    AccessTokenUri="https://api.weixin.qq.com/sns/oauth2/access_token?appid={{AppId}}&secret={{AppSecret}}&code={{Code}}&grant_type=authorization_code",
                    UserInfoUri="https://api.weixin.qq.com/sns/userinfo?access_token={{Access_Token}}&openid={{OpenId}}",
                    RedirectUri="http://toupiao1.scxfkj.net/Mob/Owin/Wechat"
                }
            });

#if DEBUG
            intId = 1;
            context.Members.AddOrUpdate(new Member[]
            {
                new Member
                {
                    Id = intId++,
                    OpenId = "11212",
                    NickName = "Jerry",
                    Password = EncryptHelper.Encrypt("123456"),
                    Sex = Sex.Male,
                    Avatar = "/File/Banner/avatar.jpg",
                    ShareCode = "ACDE8D03",
                    Latitude= 28.23f,
                    Longitude = 112.93f,
                    Score = 100,
                    Phone = "15802775429",
                    Name = "Jerry"
                }
            });
#endif

            intId = 1;
            context.Courses.AddOrUpdate(new Course[]
            {
                new Course {Id = intId, Name = "C1手动挡 学生班", Amount=3280, OrderIndex = intId++ },
                new Course {Id = intId, Name = "C2自动挡 社会班", Amount=3680, OrderIndex = intId++ },
                new Course {Id = intId, Name = "C1手动挡包补考费班", Amount=4988, OrderIndex = intId++ }
            });

            intId = 1;
            context.Cities.AddOrUpdate(new City[]
            {
                new City {Id = intId, Name = "雨花区", OrderIndex = intId++ },
                new City {Id = intId, Name = "芙蓉区", OrderIndex = intId++ },
                new City {Id = intId, Name = "天心区", OrderIndex = intId++ },
                new City {Id = intId, Name = "岳麓区", OrderIndex = intId++ }
            });

            intId = 1;
            context.Schools.AddOrUpdate(new School[]
            {
                new School {Id = intId, Name = "社会人员", OrderIndex = intId++ },
                new School {Id = intId, Name = "中南大学", OrderIndex = intId++ },
                new School {Id = intId, Name = "湖南大学", OrderIndex = intId++ }
            });

            intId = 1;
            context.Grounds.AddOrUpdate(new Ground[]
            {
                new Ground {Id=intId, Name = "武广校区", PicPath="", Address="", OrderIndex=intId++ },
                new Ground {Id=intId, Name = "农大校区", PicPath="", Address="", OrderIndex=intId++ },
                new Ground {Id=intId, Name = "一师范校区", PicPath="", Address="", OrderIndex=intId++ }
            });

            intId = 1;
            context.Coaches.AddOrUpdate(new Coach[]
            {
                new Coach {Id=intId, Name="曾勇", Sex=Sex.Male, PicPath="", Phone="15888888888", Summary="", OrderIndex=intId++ },
                new Coach {Id=intId, Name="刘峰林", Sex=Sex.Male, PicPath="", Phone="15666666666", Summary="", OrderIndex=intId++ },
                new Coach {Id=intId, Name="王胜", Sex=Sex.Male, PicPath="", Phone="15999999999", Summary="", OrderIndex=intId++ }
            });

            intId = 1;
            context.Banners.AddOrUpdate(new Banner[] {
                new Banner{Id = intId, BannerType = BannerType.MobIndex,PicPath = "/File/Banner/1.png",OrderIndex = intId++},
                new Banner{Id = intId, BannerType = BannerType.Question,PicPath = "/File/Banner/1.jpg",OrderIndex = intId++},
                new Banner{Id = intId, BannerType = BannerType.QuestionIndex,PicPath = "/File/Banner/1.jpg",OrderIndex = intId++},
            });

            intId = 1;
            context.Articles.AddOrUpdate(new Article[] {
                new Article
                {
                    Id = intId++,
                    AdminUserId = 1,
                    ArticleType = ArticleType.TopLine,
                    Title = "热烈庆祝驾校分校开业大吉",
                    Content = "<p><strong>热烈庆祝驾校分校开业大吉<span class=\"ql-cursor\">﻿</span></strong></p>",
                    UpdateTime = DateTime.Now
                }
            });

            intId = 1;
            context.WithdrawTypes.AddOrUpdate(new WithdrawType[] {
                new WithdrawType {Id=intId,Name="微信提现",OrderIndex = intId++ },
                new WithdrawType {Id=intId,Name="支付宝提现",OrderIndex = intId++ }
            });
        }
    }
}