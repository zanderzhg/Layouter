using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using Layouter.Logs;
using Layouter.Services;
using Layouter.ViewModels;
using Layouter.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Layouter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        private TrayIconService trayIconService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LogConfig.Init();

            // 配置服务
            var services = new ServiceCollection();
            ConfigureServices(services);

            //添加Serilog到依赖注入容器
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            serviceProvider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider);

            // 初始化自启动设置
            GeneralSettingsService.Instance.InitializeAutoStart();
            // 启动托盘图标
            TrayIconService.Instance.Initialize();

            // 尝试恢复上一次的分区配置
            RestorePartitionsFromLastSession();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 注册服务
            services.AddSingleton<TrayIconService>();
            services.AddSingleton<TrayMenuService>();
            services.AddSingleton<GeneralSettingsService>();
            services.AddSingleton<DesktopIconService>();
            services.AddSingleton<PartitionDataService>();
            services.AddSingleton<WindowManagerService>();

            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<DesktopManagerViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 保存所有分区配置
            SaveAllPartitions();

            // 清理资源
            trayIconService?.Dispose();
            serviceProvider?.Dispose();

            Log.CloseAndFlush();

            base.OnExit(e);
        }

        private void RestorePartitionsFromLastSession()
        {
            try
            {
                // 使用PartitionDataService恢复上次的分区配置
                PartitionDataService.Instance.RestoreWindows();
            }
            catch (System.Exception ex)
            {
                Log.Information($"恢复分区配置失败: {ex.Message}");

                var window = new DesktopManagerWindow();
                window.Show();
            }
        }

        private void SaveAllPartitions()
        {
            try
            {
                // 保存所有分区配置
                PartitionDataService.Instance.SaveAllPartitions();
            }
            catch (System.Exception ex)
            {
                Log.Information($"保存分区配置失败: {ex.Message}");
            }
        }

    }
}
