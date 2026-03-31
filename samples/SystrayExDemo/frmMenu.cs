using b13;

namespace SystrayExDemo;

//don't forget to make the change to Program.cs:
//  Application.Run(new FrmMenu());
//  SystrayApp.Run(new FrmMenu());
internal partial class FrmMenu : BaseFormEx {
    private readonly MenuItemEx _MenuItemEx;
    private readonly List<Image> _lstImageIcon;
    private Image? _imgBackground;

    protected override MenuItemEx? CommandHandler => this._MenuItemEx;

    public FrmMenu() {
        this.InitializeComponent();
        ReflectionHelper.Initialize(this);

        //Retrieve Images for MenuItems
        this._lstImageIcon = [];

        this._lstImageIcon.Add(RessourceHelperEx.GetImageFromList(this.imageListIcon, "00a.png"));
        this._lstImageIcon.Add(RessourceHelperEx.GetImageFromList(this.imageListIcon, "01a.png"));
        this._lstImageIcon.Add(RessourceHelperEx.GetImageFromList(this.imageListIcon, "02a.png"));

        this._MenuItemEx = new(this, "#F0F0F0", 3, "#000080", -1, 2, "#000080", "#a0a080");
        this._MenuItemEx.Add(this._lstImageIcon[0], "E&xit");
        this._MenuItemEx.Add(this._lstImageIcon[1], "&Settings");
        this._MenuItemEx.Add(this._lstImageIcon[2], "Execute &backup");
        this._MenuItemEx.OnMenuItem_Click += this.OnMenuItem_Click;

        this.BackgroundImageLayout = ImageLayout.None;
        this.Paint += this.FrmConfig_Paint;
        this.Load += this.FrmConfig_Load;
        this.cmdHide.Click += this.CmdHide_Click;

        this.progressBar1.Enabled = false;
        this.progressBar1.Minimum = 0;
        this.progressBar1.Maximum = 100;
        this.progressBar1.MarqueeAnimationSpeed = 50;  //smaller is faster
        this.progressBar1.Style = ProgressBarStyle.Marquee;
        //this.progressBar1.Style = ProgressBarStyle.Continuous;
        this.progressBar1.Value = 0;
    }

    private void CmdHide_Click(object? sender, EventArgs e) {
        this.Hide();
    }

    private void OnMenuItem_Click(object? sender, string e) {
        System.Diagnostics.Debug.WriteLine($"MenuItem clicked: {e}");
        switch (e) {
            case "EXIT":
                Application.Exit();
                break;

            case "SETTINGS":
                SystrayApp.Context.ChangeIcon(1);
                break;

            default:
                break;
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e) {
        //disposing here
        this._MenuItemEx.Dispose();

        foreach (Image objTmp in _lstImageIcon) {
            objTmp.Dispose();
        }
        this._lstImageIcon.Clear();

        this._imgBackground?.Dispose();

        base.OnFormClosed(e);
    }

    private void FrmConfig_Load(object? sender, EventArgs e) {
        this._imgBackground = this._MenuItemEx.CreateUI();
    }

    private void FrmConfig_Paint(object? sender, PaintEventArgs e) {
        if (this._imgBackground != null) {
            e.Graphics.DrawImage(this._imgBackground, 0, 0);
        }
    }
}
