namespace DoneLogger.Forms;

partial class SplashForm
{
    private System.Windows.Forms.PictureBox pictureBox;

    private void InitializeComponent()
    {
        pictureBox = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        SuspendLayout();

        pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
        pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        pictureBox.TabStop = false;

        ClientSize = new System.Drawing.Size(400, 300);
        Controls.Add(pictureBox);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        Name = "SplashForm";
        ShowInTaskbar = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        TopMost = true;

        ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
        ResumeLayout(false);
    }
}
