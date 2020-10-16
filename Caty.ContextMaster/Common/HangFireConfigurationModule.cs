using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.HttpJob;
using Hangfire.SqlServer;
using Hangfire.Tags.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TimeZoneConverter;

namespace Caty.ContextMaster.Common
{
    public static class HangFireConfigurationModule
    {
        public static IServiceCollection AddHangFireModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHangfire(x => x.UseSqlServerStorage(
                    configuration.GetSection("HangfireSqlserverConnectionString").Get<string>(),
                    new SqlServerStorageOptions()
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    })
                .UseTagsWithSql()
                .UseConsole(new ConsoleOptions()
                {
                    BackgroundColor = "#000079"
                })
                .UseHangfireHttpJob(new HangfireHttpJobOptions
                {
                    MailOption = new MailOption
                    {
                        Server = configuration.GetSection("HangfireMail:Server").Get<string>(),
                        Port = configuration.GetSection("HangfireMail:Port").Get<int>(),
                        UseSsl = configuration.GetSection("HangfireMail:UseSsl").Get<bool>(),
                        User = configuration.GetSection("HangfireMail:User").Get<string>(),
                        Password = configuration.GetSection("HangfireMail:Password").Get<string>()
                    },
                    DefaultRecurringQueueName = configuration.GetSection("DefaultRecurringQueueName").Get<string>(),
                    DefaultBackGroundJobQueueName = "DEFAULT",
                    RecurringJobTimeZone = TZConvert.GetTimeZoneInfo("Asia/Shanghai")
                }));
            return service;
        }

        public static IApplicationBuilder AddHangFireServer(this IApplicationBuilder app, IConfiguration configuration)
        {
            #region 强制显示中文

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh")
            };

            app.UseRequestLocalization(options);

            //强制显示中文
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh");

            #endregion 强制显示中文

            var queues = configuration.GetSection("HangfireQueues").Get<List<string>>().ToArray();
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerTimeout = TimeSpan.FromMinutes(4),
                SchedulePollingInterval = TimeSpan.FromSeconds(15), //秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                ShutdownTimeout = TimeSpan.FromMinutes(30), //超时时间
                Queues = queues, //队列
                WorkerCount = Math.Max(Environment.ProcessorCount, 40) //工作线程数，当前允许的最大线程，默认20
            });

            var hangfireStartUpPath = configuration.GetSection("HangfireStartUpPath").Get<string>();
            if (string.IsNullOrWhiteSpace(hangfireStartUpPath)) hangfireStartUpPath = "/job";

            var dashboardConfig = new DashboardOptions
            {
                AppPath = "#",
                DisplayStorageConnectionString = false,
                IsReadOnlyFunc = context => false
            };
            var dashboardUserName = configuration.GetSection("HangfireUserName").Get<string>();
            var dashboardPwd = configuration.GetSection("HangfirePwd").Get<string>();
            if (!string.IsNullOrEmpty(dashboardPwd) && !string.IsNullOrEmpty(dashboardUserName))
            {
                dashboardConfig.Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = dashboardUserName,
                                PasswordClear = dashboardPwd
                            }
                        }
                    })
                };
            }
            app.UseHangfireDashboard(hangfireStartUpPath, dashboardConfig);

            var hangfireReadOnlyPath = configuration.GetSection("HangfireReadOnlyPath").Get<string>();
            if (!string.IsNullOrWhiteSpace(hangfireReadOnlyPath))
            {
                //只读面板，只能读取不能操作
                app.UseHangfireDashboard(hangfireReadOnlyPath, new DashboardOptions
                {
                    IgnoreAntiforgeryToken = true,
                    AppPath = hangfireStartUpPath, //返回时跳转的地址
                    DisplayStorageConnectionString = false, //是否显示数据库连接信息
                    IsReadOnlyFunc = context => true
                });
            }
            return app;
        }
    }
}