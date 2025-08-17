using LMS_Quadra.Domain;
using LMS_Quadra.Domain.Entities;
using LMS_Quadra.Domain.Repositories.Abstract;
using LMS_Quadra.Domain.Repositories.EntityFramework;
using LMS_Quadra.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LMS_Quadra
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //подключение БД
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.Database.ConnectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddTransient<IAnswerRepository, EFAnswerRepository>();
            builder.Services.AddTransient<ICourseCompletionRepository, EFCourseCompletionRepository>();
            builder.Services.AddTransient<ICourseContentRepository, EFCourseContentRepository>();
            builder.Services.AddTransient<ICourseRepository, EFCourseRepository>();
            builder.Services.AddTransient<IDepartmentRepository, EFDepartmentRepository>();
            builder.Services.AddTransient<IQuestionRepository, EFQuestionRepository>();
            builder.Services.AddTransient<IWorkerPositionRepository, EFWorkerPositionsRepository>();
            builder.Services.AddTransient<IWorkerRepository, EFWorkerRepository>();
            builder.Services.AddTransient<DataManager>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "authCoockie";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;

                options.Events.OnRedirectToLogin = context =>
                {
                    if (!context.Request.Path.StartsWithSegments("/Account/Login"))
                    {
                        context.Response.Redirect($"/Account/Login?returnUrl={context.Request.Path}");
                    }
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            });

            builder.Services.AddControllersWithViews();
            WebApplication app = builder.Build();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "course_content",
                pattern: "Course/{courseId}/CourseContent/{action=Index}/{id?}",
                defaults: new { controller = "CourseContent" });

            app.MapControllerRoute(
                name: "answers",
                pattern: "Course/{courseId}/Question/{questionId}/Answer/{action=Create}/{answerId?}",
                defaults: new { controller = "Answer" });

            app.MapControllerRoute(
                name: "questions",
                pattern: "Course/{courseId}/Question/{action=Index}/{id?}",
                defaults: new { controller = "Question" });

            app.MapControllerRoute("default", "{controller=Account}/{action=Login}/{id?}");

            await app.RunAsync();
        }

    }
}
