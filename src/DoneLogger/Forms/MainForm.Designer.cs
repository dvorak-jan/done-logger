#nullable enable
namespace DoneLogger.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    private TabControl tabControl = null!;
    private TabPage tabMain = null!;
    private TabPage tabAdvanced = null!;
    private TabPage tabSummary = null!;

    // Main tab
    private Label lblStatus = null!;
    private Label lblElapsed = null!;
    private Button btnEditLog = null!;
    private Label lblCategoryLabel = null!;
    private ComboBox cboCategory = null!;
    private Button btnStartWork = null!;
    private Button btnStopWork = null!;

    // Advanced tab
    private Button btnCreateLog = null!;
    private GroupBox grpCustomTime = null!;
    private Label lblAdvCategoryLabel = null!;
    private ComboBox cboAdvCategory = null!;
    private Label lblStartTimeLabel = null!;
    private TextBox txtStartTime = null!;
    private Button btnStartWorkAdv = null!;
    private Label lblStopTimeLabel = null!;
    private TextBox txtStopTime = null!;
    private Button btnStopWorkAdv = null!;
    private GroupBox grpOpenLog = null!;
    private ComboBox cboLogDate = null!;
    private Button btnOpenLogDate = null!;

    // Summary tab
    private Label lblFrom = null!;
    private DateTimePicker dtpFrom = null!;
    private Label lblTo = null!;
    private DateTimePicker dtpTo = null!;
    private Button btnCreateSummary = null!;
    private GroupBox grpQuickSelect = null!;
    private Button btnToday = null!;
    private Button btnYesterday = null!;
    private Button btnThisWeek = null!;
    private Button btnThisMonth = null!;
    private Button btnLastWeek = null!;
    private Button btnLastMonth = null!;
    private Button btnThisYear = null!;
    private Button btnLastYear = null!;

    private System.Windows.Forms.Timer timerElapsed = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        tabControl = new TabControl();
        tabMain = new TabPage();
        tabAdvanced = new TabPage();
        tabSummary = new TabPage();

        lblStatus = new Label();
        lblElapsed = new Label();
        btnEditLog = new Button();
        lblCategoryLabel = new Label();
        cboCategory = new ComboBox();
        btnStartWork = new Button();
        btnStopWork = new Button();

        btnCreateLog = new Button();
        grpCustomTime = new GroupBox();
        lblAdvCategoryLabel = new Label();
        cboAdvCategory = new ComboBox();
        lblStartTimeLabel = new Label();
        txtStartTime = new TextBox();
        btnStartWorkAdv = new Button();
        lblStopTimeLabel = new Label();
        txtStopTime = new TextBox();
        btnStopWorkAdv = new Button();
        grpOpenLog = new GroupBox();
        cboLogDate = new ComboBox();
        btnOpenLogDate = new Button();

        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnCreateSummary = new Button();
        grpQuickSelect = new GroupBox();
        btnToday = new Button();
        btnYesterday = new Button();
        btnThisWeek = new Button();
        btnThisMonth = new Button();
        btnLastWeek = new Button();
        btnLastMonth = new Button();
        btnThisYear = new Button();
        btnLastYear = new Button();

        timerElapsed = new System.Windows.Forms.Timer(components);

        SuspendLayout();
        grpCustomTime.SuspendLayout();
        grpOpenLog.SuspendLayout();
        grpQuickSelect.SuspendLayout();
        tabMain.SuspendLayout();
        tabAdvanced.SuspendLayout();
        tabSummary.SuspendLayout();
        tabControl.SuspendLayout();

        // ── Main tab ──────────────────────────────────────────────

        lblStatus.Location = new Point(8, 10);
        lblStatus.Size = new Size(356, 20);
        lblStatus.Text = "Not tracking";

        lblElapsed.Location = new Point(8, 34);
        lblElapsed.Size = new Size(356, 22);
        lblElapsed.Font = new Font(Font.FontFamily, 10F, FontStyle.Bold);
        lblElapsed.Text = string.Empty;

        btnEditLog.Location = new Point(8, 68);
        btnEditLog.Size = new Size(356, 32);
        btnEditLog.Text = "Edit Latest Log";
        btnEditLog.Click += btnEditLog_Click;

        lblCategoryLabel.AutoSize = true;
        lblCategoryLabel.Location = new Point(8, 116);
        lblCategoryLabel.Text = "Category:";

        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.Location = new Point(80, 113);
        cboCategory.Size = new Size(276, 23);

        btnStartWork.Location = new Point(8, 148);
        btnStartWork.Size = new Size(174, 35);
        btnStartWork.Text = "Start Work";
        btnStartWork.Click += btnStartWork_Click;

        btnStopWork.Location = new Point(190, 148);
        btnStopWork.Size = new Size(174, 35);
        btnStopWork.Text = "Stop Work";
        btnStopWork.Click += btnStopWork_Click;

        tabMain.Controls.AddRange(new Control[] {
            lblStatus, lblElapsed, btnEditLog,
            lblCategoryLabel, cboCategory,
            btnStartWork, btnStopWork
        });
        tabMain.Text = "Work";

        // ── Advanced tab ──────────────────────────────────────────

        btnCreateLog.Location = new Point(8, 8);
        btnCreateLog.Size = new Size(356, 32);
        btnCreateLog.Text = "Create New Log";
        btnCreateLog.Click += btnCreateLog_Click;

        // grpCustomTime
        lblAdvCategoryLabel.AutoSize = true;
        lblAdvCategoryLabel.Location = new Point(8, 24);
        lblAdvCategoryLabel.Text = "Category:";

        cboAdvCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAdvCategory.Location = new Point(80, 20);
        cboAdvCategory.Size = new Size(268, 23);

        lblStartTimeLabel.AutoSize = true;
        lblStartTimeLabel.Location = new Point(8, 58);
        lblStartTimeLabel.Text = "Start (HH:mm):";

        txtStartTime.Location = new Point(110, 55);
        txtStartTime.Size = new Size(58, 23);
        txtStartTime.MaxLength = 5;

        btnStartWorkAdv.Location = new Point(178, 52);
        btnStartWorkAdv.Size = new Size(170, 32);
        btnStartWorkAdv.Text = "Start Work";
        btnStartWorkAdv.Click += btnStartWorkAdv_Click;

        lblStopTimeLabel.AutoSize = true;
        lblStopTimeLabel.Location = new Point(8, 94);
        lblStopTimeLabel.Text = "Stop (HH:mm):";

        txtStopTime.Location = new Point(110, 91);
        txtStopTime.Size = new Size(58, 23);
        txtStopTime.MaxLength = 5;

        btnStopWorkAdv.Location = new Point(178, 88);
        btnStopWorkAdv.Size = new Size(170, 32);
        btnStopWorkAdv.Text = "Stop Work";
        btnStopWorkAdv.Click += btnStopWorkAdv_Click;

        grpCustomTime.Location = new Point(8, 48);
        grpCustomTime.Size = new Size(356, 132);
        grpCustomTime.Text = "Custom time";
        grpCustomTime.Controls.AddRange(new Control[] {
            lblAdvCategoryLabel, cboAdvCategory,
            lblStartTimeLabel, txtStartTime, btnStartWorkAdv,
            lblStopTimeLabel, txtStopTime, btnStopWorkAdv
        });

        // grpOpenLog
        cboLogDate.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLogDate.FormattingEnabled = true;
        cboLogDate.Location = new Point(8, 22);
        cboLogDate.Size = new Size(232, 23);

        btnOpenLogDate.Location = new Point(248, 19);
        btnOpenLogDate.Size = new Size(100, 32);
        btnOpenLogDate.Text = "Open Log";
        btnOpenLogDate.Click += btnOpenLogDate_Click;

        grpOpenLog.Location = new Point(8, 188);
        grpOpenLog.Size = new Size(356, 62);
        grpOpenLog.Text = "Open log by date";
        grpOpenLog.Controls.AddRange(new Control[] { cboLogDate, btnOpenLogDate });

        tabAdvanced.Controls.AddRange(new Control[] {
            btnCreateLog, grpCustomTime, grpOpenLog
        });
        tabAdvanced.Text = "Advanced";

        // ── Summary tab ───────────────────────────────────────────

        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(8, 12);
        lblFrom.Text = "From:";

        dtpFrom.Format = DateTimePickerFormat.Custom;
        dtpFrom.CustomFormat = "yyyy-MM-dd";
        dtpFrom.Location = new Point(52, 8);
        dtpFrom.Size = new Size(308, 23);
        dtpFrom.Value = DateTime.Today;

        lblTo.AutoSize = true;
        lblTo.Location = new Point(8, 42);
        lblTo.Text = "To:";

        dtpTo.Format = DateTimePickerFormat.Custom;
        dtpTo.CustomFormat = "yyyy-MM-dd";
        dtpTo.Location = new Point(52, 38);
        dtpTo.Size = new Size(308, 23);
        dtpTo.Value = DateTime.Today;

        btnCreateSummary.Location = new Point(8, 70);
        btnCreateSummary.Size = new Size(356, 32);
        btnCreateSummary.Text = "Generate Summary";
        btnCreateSummary.Click += btnCreateSummary_Click;

        // grpQuickSelect — 4 columns, 2 rows, 76px per button, 10px gaps, 10px left/right margin
        btnToday.Location = new Point(10, 22);
        btnToday.Size = new Size(76, 28);
        btnToday.Text = "Today";
        btnToday.Click += btnToday_Click;

        btnYesterday.Location = new Point(96, 22);
        btnYesterday.Size = new Size(76, 28);
        btnYesterday.Text = "Yesterday";
        btnYesterday.Click += btnYesterday_Click;

        btnThisWeek.Location = new Point(182, 22);
        btnThisWeek.Size = new Size(76, 28);
        btnThisWeek.Text = "This Week";
        btnThisWeek.Click += btnThisWeek_Click;

        btnThisMonth.Location = new Point(268, 22);
        btnThisMonth.Size = new Size(76, 28);
        btnThisMonth.Text = "This Month";
        btnThisMonth.Click += btnThisMonth_Click;

        btnLastWeek.Location = new Point(10, 56);
        btnLastWeek.Size = new Size(76, 28);
        btnLastWeek.Text = "Last Week";
        btnLastWeek.Click += btnLastWeek_Click;

        btnLastMonth.Location = new Point(96, 56);
        btnLastMonth.Size = new Size(76, 28);
        btnLastMonth.Text = "Last Month";
        btnLastMonth.Click += btnLastMonth_Click;

        btnThisYear.Location = new Point(182, 56);
        btnThisYear.Size = new Size(76, 28);
        btnThisYear.Text = "This Year";
        btnThisYear.Click += btnThisYear_Click;

        btnLastYear.Location = new Point(268, 56);
        btnLastYear.Size = new Size(76, 28);
        btnLastYear.Text = "Last Year";
        btnLastYear.Click += btnLastYear_Click;

        grpQuickSelect.Location = new Point(8, 112);
        grpQuickSelect.Size = new Size(356, 96);
        grpQuickSelect.Text = "Quick select";
        grpQuickSelect.Controls.AddRange(new Control[] {
            btnToday, btnYesterday, btnThisWeek, btnThisMonth,
            btnLastWeek, btnLastMonth, btnThisYear, btnLastYear
        });

        tabSummary.Controls.AddRange(new Control[] {
            lblFrom, dtpFrom, lblTo, dtpTo, btnCreateSummary, grpQuickSelect
        });
        tabSummary.Text = "Summary";

        // ── TabControl ───────────────────────────────────────────

        tabControl.Dock = DockStyle.Fill;
        tabControl.Controls.AddRange(new TabPage[] { tabMain, tabAdvanced, tabSummary });
        tabControl.Selected += tabControl_Selected;

        // ── Timer ────────────────────────────────────────────────

        timerElapsed.Interval = 1000;
        timerElapsed.Tick += timerElapsed_Tick;

        // ── Form ─────────────────────────────────────────────────

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(380, 280);
        Controls.Add(tabControl);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "done-logger";

        grpCustomTime.ResumeLayout(false);
        grpCustomTime.PerformLayout();
        grpOpenLog.ResumeLayout(false);
        grpQuickSelect.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tabMain.PerformLayout();
        tabAdvanced.ResumeLayout(false);
        tabSummary.ResumeLayout(false);
        tabControl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
