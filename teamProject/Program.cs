using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using teamProject.Data;

namespace teamProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();



            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2U1hhQlJBfVddXmJWfFN0QXNYfVR0cF9GYkwxOX1dQl9nSXZTd0RhXHpccXJWRWY=;Mgo+DSMBPh8sVXJyS0d+X1RPfkBBXHxLflFyVWJbdVp0flVEcDwsT3RfQFljQX5Sd0dmXH1fdndWQA==");


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Add this line to redirect the root URL to /Dashboard/Index
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/Dashboard/Index");
                    return System.Threading.Tasks.Task.CompletedTask;
                });

                endpoints.MapFallbackToPage("/Dashboard/Index");
            });

            app.Run();
        }
    }
}
