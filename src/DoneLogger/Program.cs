namespace DoneLogger;

using DoneLogger.Forms;
using DoneLogger.Services;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            var config = ConfigService.Load();
            var logService = new LogService(config);
            var trackingService = new TimeTrackingService(logService);
            var summaryService = new SummaryService(logService, config, trackingService);

            var splash = new SplashForm();
            var context = new ApplicationContext();

            splash.SplashComplete += () =>
            {
                var mainForm = new MainForm(config, logService, trackingService, summaryService);
                mainForm.FormClosed += (s, e) => context.ExitThread();
                mainForm.Show();
                splash.Close();
            };

            splash.Show();
            Application.Run(context);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
