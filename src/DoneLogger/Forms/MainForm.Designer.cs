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

        timerElapsed = new System.Windows.Forms.Timer(components);

        SuspendLayout();
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

        tabAdvanced.Controls.Add(btnCreateLog);
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

        tabMain.ResumeLayout(false);
        tabMain.PerformLayout();
        tabAdvanced.ResumeLayout(false);
        tabControl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
