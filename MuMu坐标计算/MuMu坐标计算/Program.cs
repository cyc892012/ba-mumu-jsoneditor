using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogService.Initialize();

            Application.ThreadException += (sender, e) =>
            {
                LogService.Error("Application", e.Exception, "UI线程未处理异常");
                MessageBox.Show(
                    "程序发生未处理的错误，详细信息已记录到日志文件。\n\n" +
                    "错误: " + e.Exception.Message,
                    "程序错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                    LogService.Error("Application", ex, "AppDomain未处理异常");
                else
                    LogService.Error("Application", "AppDomain未处理异常: " + (e.ExceptionObject?.ToString() ?? "未知"));
            };

            Application.ApplicationExit += (sender, e) =>
            {
                LogService.Shutdown();
            };

            LogService.Info("Application", "程序启动，版本: " + Application.ProductVersion);

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                LogService.Error("Application", ex, "程序主循环异常退出");
                MessageBox.Show(
                    "程序发生致命错误，即将退出。\n详细信息已记录到日志文件。\n\n" +
                    "错误: " + ex.Message,
                    "致命错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
