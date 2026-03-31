namespace s3Bucket;

partial class FrmMenu {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenu));
        this.imageListIcon = new ImageList(this.components);
        this.picBackground = new PictureBox();
        this.cmdHide = new Button();
        this.progressBar1 = new ProgressBar();
        ((System.ComponentModel.ISupportInitialize)this.picBackground).BeginInit();
        this.SuspendLayout();
        // 
        // imageListIcon
        // 
        this.imageListIcon.ColorDepth = ColorDepth.Depth32Bit;
        this.imageListIcon.ImageStream = (ImageListStreamer)resources.GetObject("imageListIcon.ImageStream");
        this.imageListIcon.TransparentColor = Color.Transparent;
        this.imageListIcon.Images.SetKeyName(0, "00a.png");
        this.imageListIcon.Images.SetKeyName(1, "01a.png");
        this.imageListIcon.Images.SetKeyName(2, "02a.png");
        this.imageListIcon.Images.SetKeyName(3, "02b.png");
        this.imageListIcon.Images.SetKeyName(4, "02c.png");
        // 
        // picBackground
        // 
        this.picBackground.Dock = DockStyle.Fill;
        this.picBackground.Enabled = false;
        this.picBackground.Image = (Image)resources.GetObject("picBackground.Image");
        this.picBackground.Location = new Point(0, 0);
        this.picBackground.Margin = new Padding(0);
        this.picBackground.Name = "picBackground";
        this.picBackground.Size = new Size(221, 142);
        this.picBackground.TabIndex = 0;
        this.picBackground.TabStop = false;
        this.picBackground.Visible = false;
        // 
        // cmdHide
        // 
        this.cmdHide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.cmdHide.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        this.cmdHide.Location = new Point(187, 4);
        this.cmdHide.Name = "cmdHide";
        this.cmdHide.Size = new Size(30, 28);
        this.cmdHide.TabIndex = 1;
        this.cmdHide.Text = "X";
        this.cmdHide.UseVisualStyleBackColor = true;
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new Point(39, 5);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new Size(148, 26);
        this.progressBar1.TabIndex = 2;
        // 
        // FrmMenu
        // 
        this.AutoScaleMode = AutoScaleMode.None;
        this.ClientSize = new Size(221, 142);
        this.ControlBox = false;
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.cmdHide);
        this.Controls.Add(this.picBackground);
        this.DoubleBuffered = true;
        this.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        this.FormBorderStyle = FormBorderStyle.None;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.KeyPreview = true;
        this.MaximizeBox = false;
        this.Name = "FrmMenu";
        this.StartPosition = FormStartPosition.Manual;
        this.TopMost = true;
        ((System.ComponentModel.ISupportInitialize)this.picBackground).EndInit();
        this.ResumeLayout(false);
    }

    #endregion
    private ImageList imageListIcon;
    private PictureBox picBackground;
    private Button cmdHide;
    private ProgressBar progressBar1;
}
