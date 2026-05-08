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
            var summaryService = new SummaryService(logService, config);
            Application.Run(new MainForm(config, logService, trackingService, summaryService));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
