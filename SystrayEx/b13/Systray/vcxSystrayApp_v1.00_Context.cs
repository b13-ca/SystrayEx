// Copyright 2026, Patrice CHARBONNEAU
//                 a.k.a. Sigma3Wolf
//                 oIId: v2.00/2032/160e0e6a3176a8c4235332aa8e0d422c
//                 All rights reserved.
//                 https://b13.ca/
//
// This source code is licensed under the [BSD 3-Clause "New" or "Revised" License] found
// in the LICENSE file in the root directory of this source tree.

#region Usage and dependency
//*************************************************************************************************//
//** WARNING: If you modify this file, you MUST rename it to exclude the version number :WARNING **//
//*************************************************************************************************//
//      Usage:  Adding Ststray icon capability to Windows Form App
// Dependency:  ReflectionHelper
#endregion Usage and dependency

#region History
//    History:
// v1.00 - 2026-03-27:	Initial release;
#endregion History

#region b13 namespace
#pragma warning disable IDE0130
namespace b13;
#pragma warning restore IDE0130
#endregion b13 namespace

public class SystrayAppContext : ApplicationContext, ISystrayService {
    #region Declaration
    // To detect redundant calls
    private volatile bool _disposed = false;

    // Declare disposable object
    private readonly Form _mainForm;
    private Icon[] IconArray = new Icon[3];
    private readonly NotifyIcon _trayIcon;
    #endregion Declaration

    #region Constructor
    public SystrayAppContext(Form pobjform) {
        // Instantiate disposable object.
        _mainForm = pobjform;
        _mainForm.ShowInTaskbar = true;  //To avoid a bug in Minimize/Restore

        // 1. Right‑click your project → Properties
        // 2. Go to the Resources tab
        // 3. At the top, choose Icons (or Images)
        // 4. Click Add Resource → Add Existing File…
        // 5. Select your .ico file (must be systray1.ico, systray2.ico, systray3.ico)
        this.IconArray[0] = ReflectionHelper.GetIconFromRessource("systrayIcon1");  //systrayIcon1.ico
        this.IconArray[1] = ReflectionHelper.GetIconFromRessource("systrayIcon2");  //systrayIcon2.ico
        this.IconArray[2] = ReflectionHelper.GetIconFromRessource("systrayIcon3");  //systrayIcon3.ico

        // Initialize Tray Icon
        this._trayIcon = new NotifyIcon() {
            Icon = this.IconArray[0],
            Visible = true
        };

        this._mainForm.Visible = false;
        this._trayIcon.MouseDown += this.TrayIcon_MouseDown;
        this._trayIcon.DoubleClick += this.TrayIcon_DoubleClick;
        this._mainForm.Resize += this.MainForm_Resize;
        this._mainForm.FormClosing += this.MainForm_Closing;
    }
    #endregion Constructor

    #region Deconstructor/Disposing
    //Protected implementation of Dispose pattern.
    //public void Dispose() {
    //    Dispose(true);
    //    GC.SuppressFinalize(this);
    //}

    protected override void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                // Dispose managed objects
                if (this._trayIcon != null) {
                    this._trayIcon.Visible = false;
                    this._trayIcon.Dispose();
                }

                if (this.IconArray != null) {
                    foreach (Icon objIcon in IconArray) {
                        objIcon.Dispose();
                    }
                    IconArray = [];
                }
            }

            // Dispose unmanaged objects here (if any)

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    //~SystrayAppContext() {
    //    this.Dispose(false);
    //}
    #endregion Deconstructor/Disposing

    private void MainForm_Closing(object? sender, FormClosingEventArgs e) {
        if (e.CloseReason == CloseReason.UserClosing) {
            //Prevent Form from closing:
            //_mainForm.Hide();
            //e.Cancel = true;

            this.Exit();
        }
    }

    private void MainForm_Resize(object? sender, EventArgs e) {
        //prevent unwanted event
        if (_mainForm.WindowState == FormWindowState.Minimized) {
            _mainForm.Visible = false;
            _mainForm.WindowState = FormWindowState.Normal;
        }
    }

    //doubleclick
    private void TrayIcon_DoubleClick(object? sender, EventArgs e) {
        this.ShowMenu();
    }

    private void TrayIcon_MouseDown(object? sender, MouseEventArgs e) {
        switch (e.Button) {
            // Right click to reactivate
            case MouseButtons.Right:
            case MouseButtons.Left:
                this.ShowMenu();
                break;
        }
    }

    public void ChangeIcon(int plngIconIndex, string pstrMessage = "") {
        //http://www.icons-land.com/vista-base-software-icons.php
        //this._trayIcon.Icon = Icon.FromHandle(((Bitmap)imageList1.Images["Shield_Red.ico"]).GetHicon());

        if (plngIconIndex >= 0 && plngIconIndex <= 2) {
            this._trayIcon.Icon = IconArray[plngIconIndex];
        }

        if (pstrMessage.Length > 0) {
            this._trayIcon.BalloonTipTitle = "Message";
            this._trayIcon.BalloonTipText = pstrMessage;
            this._trayIcon.ShowBalloonTip(2500);
        }
    }

    public void ShowMenu() {
        if (this._mainForm.Visible) {
            this._mainForm.Hide();
        } else {
            //Show Form Menu at proper position
            Point pntPos = Cursor.Position;
            Rectangle recScreen = Screen.GetWorkingArea(pntPos);
            // X clamp
            if ((pntPos.X + this._mainForm.Width) > recScreen.Right) {
                pntPos.X = recScreen.Right - this._mainForm.Width;
            }

            if (pntPos.X < recScreen.Left) {
                pntPos.X = recScreen.Left;
            }

            // Y positioning (smart)
            if (pntPos.Y > recScreen.Top + recScreen.Height / 2) {
                // bottom half → show above
                pntPos.Y = pntPos.Y - this._mainForm.Height;
            } else {
                // top half → show below
                pntPos.Y = pntPos.Y;
            }

            // Y clamp
            if ((pntPos.Y + this._mainForm.Height) > recScreen.Bottom) {
                pntPos.Y = recScreen.Bottom - this._mainForm.Height;
            }

            if (pntPos.Y < recScreen.Top) {
                pntPos.Y = recScreen.Top;
            }

            this._mainForm.Location = pntPos;
            this._mainForm.TopMost = true;
            this._mainForm.Show();
        }
    }

    public void Exit() {
        this._trayIcon.Visible = false;
        Application.Exit();
    }
}