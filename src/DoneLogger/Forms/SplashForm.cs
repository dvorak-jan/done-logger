namespace DoneLogger.Forms;

public partial class SplashForm : Form
{
    private readonly System.Windows.Forms.Timer _timer;
    private int _displayTicks;

    // ~2500ms display at 30ms/tick, then 0.05 opacity steps to fade (~600ms)
    private const int DisplayDurationTicks = 83;
    private const double FadeStep = 0.05;

    public event Action? SplashComplete;

    public SplashForm(string? imagePath)
    {
        InitializeComponent();

        if (imagePath != null && File.Exists(imagePath))
            pictureBox.Image = Image.FromFile(imagePath);

        _timer = new System.Windows.Forms.Timer { Interval = 30 };
        _timer.Tick += OnTick;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _timer.Start();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_displayTicks < DisplayDurationTicks)
        {
            _displayTicks++;
            return;
        }

        Opacity -= FadeStep;
        if (Opacity <= 0)
        {
            _timer.Stop();
            SplashComplete?.Invoke();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
            pictureBox.Image?.Dispose();
        }
        base.Dispose(disposing);
    }
}
