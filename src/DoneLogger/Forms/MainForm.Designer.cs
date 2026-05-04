#nullable enable
namespace DoneLogger.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label lblStatus = null!;
    private Label lblElapsed = null!;
    private Label lblCategoryLabel = null!;
    private ComboBox cboCategory = null!;
    private Button btnCreateLog = null!;
    private Button btnEditLog = null!;
    private Button btnStartWork = null!;
    private Button btnStopWork = null!;
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

        lblStatus = new Label();
        lblElapsed = new Label();
        lblCategoryLabel = new Label();
        cboCategory = new ComboBox();
        btnCreateLog = new Button();
        btnEditLog = new Button();
        btnStartWork = new Button();
        btnStopWork = new Button();
        timerElapsed = new System.Windows.Forms.Timer(components);

        SuspendLayout();

        // lblStatus
        lblStatus.Location = new Point(10, 12);
        lblStatus.Size = new Size(330, 20);
        lblStatus.Text = "Not tracking";

        // lblElapsed
        lblElapsed.Location = new Point(10, 36);
        lblElapsed.Size = new Size(330, 22);
        lblElapsed.Font = new Font(Font.FontFamily, 10F, FontStyle.Bold);
        lblElapsed.Text = string.Empty;

        // lblCategoryLabel
        lblCategoryLabel.AutoSize = true;
        lblCategoryLabel.Location = new Point(10, 70);
        lblCategoryLabel.Text = "Category:";

        // cboCategory
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.Location = new Point(80, 67);
        cboCategory.Size = new Size(260, 23);

        // btnCreateLog
        btnCreateLog.Location = new Point(10, 104);
        btnCreateLog.Size = new Size(160, 32);
        btnCreateLog.Text = "Create New Log";
        btnCreateLog.Click += btnCreateLog_Click;

        // btnEditLog
        btnEditLog.Location = new Point(180, 104);
        btnEditLog.Size = new Size(160, 32);
        btnEditLog.Text = "Edit Latest Log";
        btnEditLog.Click += btnEditLog_Click;

        // btnStartWork
        btnStartWork.Location = new Point(10, 146);
        btnStartWork.Size = new Size(160, 35);
        btnStartWork.Text = "Start Work";
        btnStartWork.Click += btnStartWork_Click;

        // btnStopWork
        btnStopWork.Location = new Point(180, 146);
        btnStopWork.Size = new Size(160, 35);
        btnStopWork.Text = "Stop Work";
        btnStopWork.Click += btnStopWork_Click;

        // timerElapsed
        timerElapsed.Interval = 1000;
        timerElapsed.Tick += timerElapsed_Tick;

        // Form
        ClientSize = new Size(350, 193);
        Controls.AddRange(new Control[]
        {
            lblStatus, lblElapsed, lblCategoryLabel, cboCategory,
            btnCreateLog, btnEditLog, btnStartWork, btnStopWork
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "done-logger";

        ResumeLayout(false);
        PerformLayout();
    }
}
