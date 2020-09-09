using Caty.ContextMaster.Common;
using Caty.ContextMaster.Common.Mail;
using Caty.ContextMaster.Models;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Caty.ContextMaster
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHangfire(x => x.UseSqlServerStorage("ConnectionStrings"));
            services.AddHangFireModule(Configuration);

            #region MailKit
            services.Configure<SmtpSettings>(Configuration.GetSection("MailKit"));
            services.AddSingleton<IMailer, Mailer>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            #region Hangfire

            app.UseHangfireServer(HangFireConfigurationModule.JobOptions(Configuration));
            app.UseHangfireDashboard("/TaskManager", HangFireConfigurationModule.HfDispose(Configuration));
            HangFireConfigurationModule.HangfireService();

            #endregion Hangfire

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}