using Caty.ContextMaster.Services;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Caty.ContextMaster.Common
{
    public static class HangFireConfigurationModule
    {
        public static IServiceCollection AddHangFireModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(configuration["Hangfire:StorageConnectionStr"]);
            });
            return services;
        }

        /// <summary>
        /// 配置启动
        /// </summary>
        /// <returns></returns>
        public static BackgroundJobServerOptions JobOptions(IConfiguration configuration)
        {
            var queueName = configuration.GetSection("Hangfire:JobOptionSetting:Queues")?.GetChildren()?.ToArray().Select(c => c.Value).ToArray();
            return new BackgroundJobServerOptions
            {
                Queues = queueName,//队列名称，只能为小写
                WorkerCount = Environment.ProcessorCount * Convert.ToInt32(value: configuration["Hangfire:JobOptionSetting:WorkerCount"]), //并发任务
                ServerName = configuration["Hangfire:JobOptionSetting:ServerName"], //代表服务名称
            };
        }

        /// <summary>
        /// 配置可视化账号
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DashboardOptions HfDispose(IConfiguration configuration)
        {
            var filter = new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            {
                SslRedirect = false,
                RequireSsl = false,
                LoginCaseSensitive = false,
                Users = new[] {
                    new BasicAuthAuthorizationUser
                    {
                        Login = configuration["Hangfire:DashboardOptionsSetting:HangfireLoginName"], //可视化的登陆账号
                        PasswordClear = configuration["Hangfire:DashboardOptionsSetting:HangfireLoginPassword"] //可视化的密码
                    }
                }
            });

            return new DashboardOptions
            {
                Authorization = new[] { filter }
            };
        }

        /// <summary>
        /// 配置定时任务
        /// </summary>
        public static void HangfireService()
        {
            #region 更新Rss

            // 每天的凌晨0:00分执行一次---支持异步方法
            RecurringJob.AddOrUpdate<RssService>(s => s.GetRssFeedListAsync(), "0 0 8,20 * * ?", TimeZoneInfo.Local);

            #endregion 更新Rss
        }
    }
}