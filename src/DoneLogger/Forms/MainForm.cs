namespace DoneLogger.Forms;

using DoneLogger.Models;
using DoneLogger.Services;

public partial class MainForm : Form
{
    private readonly AppConfig _config;
    private readonly LogService _logService;
    private readonly TimeTrackingService _trackingService;

    public MainForm(AppConfig config, LogService logService, TimeTrackingService trackingService)
    {
        _config = config;
        _logService = logService;
        _trackingService = trackingService;

        InitializeComponent();

        using var iconStream = GetType().Assembly.GetManifestResourceStream("DoneLogger.donelogger.ico");
        if (iconStream != null)
            Icon = new Icon(iconStream);

        foreach (var cat in config.Categories)
            cboCategory.Items.Add(cat.Name);

        var defaultCat = config.Categories.FirstOrDefault(c => c.IsDefault);
        cboCategory.SelectedItem = defaultCat?.Name ?? config.Categories[0].Name;

        cboLogDate.Format += (s, e) => { if (e.ListItem is DateTime d) e.Value = d.ToString("yyyy-MM-dd"); };
        RefreshLogDates();
        RefreshState();
    }

    private void RefreshState()
    {
        var state = _trackingService.LoadState();
        bool tracking = state?.Active == true;

        btnStartWork.Enabled = !tracking;
        btnStopWork.Enabled = tracking;
        cboCategory.Enabled = !tracking;
        btnStartWorkAdv.Enabled = !tracking;
        txtStartTime.Enabled = !tracking;
        btnStopWorkAdv.Enabled = tracking;
        txtStopTime.Enabled = tracking;

        if (tracking && state != null)
        {
            lblStatus.Text = $"Tracking: {state.Category}  (started {state.StartTime:HH:mm})";
            UpdateElapsed(state.StartTime);
            timerElapsed.Start();
        }
        else
        {
            lblStatus.Text = "Not tracking";
            lblElapsed.Text = string.Empty;
            timerElapsed.Stop();
        }
    }

    private void RefreshLogDates()
    {
        cboLogDate.Items.Clear();
        foreach (var date in _logService.GetAllLogDates())
            cboLogDate.Items.Add(date);
        if (cboLogDate.Items.Count > 0)
            cboLogDate.SelectedIndex = 0;
    }

    private void UpdateElapsed(DateTime startTime)
    {
        var elapsed = DateTime.Now - startTime;
        lblElapsed.Text = $"Elapsed: {(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
    }

    private void timerElapsed_Tick(object? sender, EventArgs e)
    {
        var state = _trackingService.LoadState();
        if (state?.Active == true)
            UpdateElapsed(state.StartTime);
    }

    private void tabControl_Selected(object? sender, TabControlEventArgs e)
    {
        if (e.TabPage == tabAdvanced)
            RefreshLogDates();
    }

    private void btnCreateLog_Click(object? sender, EventArgs e)
    {
        try
        {
            _logService.CreateLog(DateTime.Today);
            RefreshLogDates();
            MessageBox.Show($"Log created for {DateTime.Today:yyyy-MM-dd}.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Cannot Create Log",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnEditLog_Click(object? sender, EventArgs e) =>
        OpenInEditor(_logService.FindMostRecentLogPath());

    private void btnOpenLogDate_Click(object? sender, EventArgs e)
    {
        if (cboLogDate.SelectedItem is DateTime date)
            OpenInEditor(_logService.GetLogPath(date));
    }

    private void OpenInEditor(string? logPath)
    {
        try
        {
            if (logPath == null)
            {
                MessageBox.Show("No log files found.", "Nothing to Open",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = _config.Editor,
                ArgumentList = { logPath },
                UseShellExecute = false
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open editor: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnStartWork_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_logService.LogExists(DateTime.Today))
                _logService.CreateLog(DateTime.Today);

            string category = cboCategory.SelectedItem?.ToString() ?? _config.Categories[0].Name;
            _trackingService.StartTracking(category);
            RefreshState();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnStopWork_Click(object? sender, EventArgs e)
    {
        try
        {
            string? midnightMessage = _trackingService.StopTracking();
            RefreshState();

            if (midnightMessage != null)
                MessageBox.Show(midnightMessage, "Midnight Crossing",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnStartWorkAdv_Click(object? sender, EventArgs e)
    {
        if (!TryParseTime(txtStartTime.Text, out DateTime customTime))
        {
            MessageBox.Show("Enter a valid time in HH:mm format.", "Invalid Time",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            if (!_logService.LogExists(customTime.Date))
                _logService.CreateLog(customTime.Date);

            string category = cboCategory.SelectedItem?.ToString() ?? _config.Categories[0].Name;
            _trackingService.StartTracking(category, customTime);
            txtStartTime.Text = string.Empty;
            RefreshState();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnStopWorkAdv_Click(object? sender, EventArgs e)
    {
        if (!TryParseTime(txtStopTime.Text, out DateTime customTime))
        {
            MessageBox.Show("Enter a valid time in HH:mm format.", "Invalid Time",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            string? midnightMessage = _trackingService.StopTracking(customTime);
            txtStopTime.Text = string.Empty;
            RefreshState();

            if (midnightMessage != null)
                MessageBox.Show(midnightMessage, "Midnight Crossing",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool TryParseTime(string text, out DateTime result)
    {
        if (DateTime.TryParseExact(text.Trim(), "HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var time))
        {
            result = DateTime.Today.Add(time.TimeOfDay);
            return true;
        }
        result = default;
        return false;
    }
}
