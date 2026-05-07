#nullable enable
namespace DoneLogger.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    private TabControl tabControl = null!;
    private TabPage tabMain = null!;
    private TabPage tabAdvanced = null!;

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
    private Label lblStartTimeLabel = null!;
    private TextBox txtStartTime = null!;
    private Button btnStartWorkAdv = null!;
    private Label lblStopTimeLabel = null!;
    private TextBox txtStopTime = null!;
    private Button btnStopWorkAdv = null!;
    private GroupBox grpOpenLog = null!;
    private ComboBox cboLogDate = null!;
    private Button btnOpenLogDate = null!;

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

        lblStatus = new Label();
        lblElapsed = new Label();
        btnEditLog = new Button();
        lblCategoryLabel = new Label();
        cboCategory = new ComboBox();
        btnStartWork = new Button();
        btnStopWork = new Button();

        btnCreateLog = new Button();
        grpCustomTime = new GroupBox();
        lblStartTimeLabel = new Label();
        txtStartTime = new TextBox();
        btnStartWorkAdv = new Button();
        lblStopTimeLabel = new Label();
        txtStopTime = new TextBox();
        btnStopWorkAdv = new Button();
        grpOpenLog = new GroupBox();
        cboLogDate = new ComboBox();
        btnOpenLogDate = new Button();

        timerElapsed = new System.Windows.Forms.Timer(components);

        SuspendLayout();
        grpCustomTime.SuspendLayout();
        grpOpenLog.SuspendLayout();
        tabMain.SuspendLayout();
        tabAdvanced.SuspendLayout();
        tabControl.SuspendLayout();

        // ── Main tab ──────────────────────────────────────────────

        lblStatus.Location = new Point(8, 10);
        lblStatus.Size = new Size(336, 20);
        lblStatus.Text = "Not tracking";

        lblElapsed.Location = new Point(8, 34);
        lblElapsed.Size = new Size(336, 22);
        lblElapsed.Font = new Font(Font.FontFamily, 10F, FontStyle.Bold);
        lblElapsed.Text = string.Empty;

        btnEditLog.Location = new Point(8, 68);
        btnEditLog.Size = new Size(336, 32);
        btnEditLog.Text = "Edit Latest Log";
        btnEditLog.Click += btnEditLog_Click;

        lblCategoryLabel.AutoSize = true;
        lblCategoryLabel.Location = new Point(8, 116);
        lblCategoryLabel.Text = "Category:";

        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.Location = new Point(80, 113);
        cboCategory.Size = new Size(256, 23);

        btnStartWork.Location = new Point(8, 148);
        btnStartWork.Size = new Size(160, 35);
        btnStartWork.Text = "Start Work";
        btnStartWork.Click += btnStartWork_Click;

        btnStopWork.Location = new Point(176, 148);
        btnStopWork.Size = new Size(160, 35);
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
        btnCreateLog.Size = new Size(334, 32);
        btnCreateLog.Text = "Create New Log";
        btnCreateLog.Click += btnCreateLog_Click;

        // grpCustomTime
        lblStartTimeLabel.AutoSize = true;
        lblStartTimeLabel.Location = new Point(8, 22);
        lblStartTimeLabel.Text = "Start (HH:mm):";

        txtStartTime.Location = new Point(110, 19);
        txtStartTime.Size = new Size(58, 23);
        txtStartTime.MaxLength = 5;

        btnStartWorkAdv.Location = new Point(178, 16);
        btnStartWorkAdv.Size = new Size(148, 32);
        btnStartWorkAdv.Text = "Start Work";
        btnStartWorkAdv.Click += btnStartWorkAdv_Click;

        lblStopTimeLabel.AutoSize = true;
        lblStopTimeLabel.Location = new Point(8, 58);
        lblStopTimeLabel.Text = "Stop (HH:mm):";

        txtStopTime.Location = new Point(110, 55);
        txtStopTime.Size = new Size(58, 23);
        txtStopTime.MaxLength = 5;

        btnStopWorkAdv.Location = new Point(178, 52);
        btnStopWorkAdv.Size = new Size(148, 32);
        btnStopWorkAdv.Text = "Stop Work";
        btnStopWorkAdv.Click += btnStopWorkAdv_Click;

        grpCustomTime.Location = new Point(8, 48);
        grpCustomTime.Size = new Size(334, 96);
        grpCustomTime.Text = "Custom time";
        grpCustomTime.Controls.AddRange(new Control[] {
            lblStartTimeLabel, txtStartTime, btnStartWorkAdv,
            lblStopTimeLabel, txtStopTime, btnStopWorkAdv
        });

        // grpOpenLog
        cboLogDate.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLogDate.FormattingEnabled = true;
        cboLogDate.Location = new Point(8, 22);
        cboLogDate.Size = new Size(210, 23);

        btnOpenLogDate.Location = new Point(226, 19);
        btnOpenLogDate.Size = new Size(100, 32);
        btnOpenLogDate.Text = "Open Log";
        btnOpenLogDate.Click += btnOpenLogDate_Click;

        grpOpenLog.Location = new Point(8, 152);
        grpOpenLog.Size = new Size(334, 62);
        grpOpenLog.Text = "Open log by date";
        grpOpenLog.Controls.AddRange(new Control[] { cboLogDate, btnOpenLogDate });

        tabAdvanced.Controls.AddRange(new Control[] {
            btnCreateLog, grpCustomTime, grpOpenLog
        });
        tabAdvanced.Text = "Advanced";

        // ── TabControl ───────────────────────────────────────────

        tabControl.Dock = DockStyle.Fill;
        tabControl.Controls.AddRange(new TabPage[] { tabMain, tabAdvanced });
        tabControl.Selected += tabControl_Selected;

        // ── Timer ────────────────────────────────────────────────

        timerElapsed.Interval = 1000;
        timerElapsed.Tick += timerElapsed_Tick;

        // ── Form ─────────────────────────────────────────────────

        ClientSize = new Size(356, 252);
        Controls.Add(tabControl);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "done-logger";

        grpCustomTime.ResumeLayout(false);
        grpCustomTime.PerformLayout();
        grpOpenLog.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tabMain.PerformLayout();
        tabAdvanced.ResumeLayout(false);
        tabControl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
