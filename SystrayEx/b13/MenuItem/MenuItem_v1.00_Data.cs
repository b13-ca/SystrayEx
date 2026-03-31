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
// Dependency:  MenuItem
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

internal class MenuItemData {
    private readonly MenuItemEx _parent;

    internal MenuItemData(MenuItemEx pobjParent, PictureBox pobjPictureBox, Label pobjLabel, string pstrText, char pchrShortcutKey = '\0') {
        if (string.IsNullOrWhiteSpace(pstrText)) {
            throw new ArgumentException("text parameter is not valid");
        }

        this._parent = pobjParent;
        this.MenuTextColor = this._parent.MenuTextColor;
        this.MenuHoverColor = this._parent.MenuHoverColor;

        this.PictureBox = pobjPictureBox;
        this.Label = pobjLabel;
        this.ShortKey = pchrShortcutKey;
        this.Text = pstrText;

        this.PictureBox.Click += this.MenuItemClick_Click;
        this.Label.Click += this.MenuItemClick_Click;

        this.PictureBox.MouseEnter += this.MenuItem_MouseEnter;
        this.Label.MouseEnter += this.MenuItem_MouseEnter;

        this.PictureBox.MouseLeave += this.MenuItem_MouseLeave;
        this.Label.MouseLeave += this.MenuItem_MouseLeave;
    }

    private void MenuItemClick_Click(object? sender, EventArgs e) {
        if (!this.Disable) {
            this._parent.RaiseMenuItemClick(this.Command);
        }
    }

    private void MenuItem_MouseEnter(object? sender, EventArgs e) {
        if (!this.Hovering) {
            if (!this.Disable) {
                this.Hovering = true;

                this.PictureBox.BackColor = this.MenuHoverColor;
                this.Label.BackColor = this.MenuHoverColor;
                this.Label.ForeColor = Color.White;
            }
        }
    }

    private void MenuItem_MouseLeave(object? sender, EventArgs e) {
        if (this.Hovering) {
            this.Hovering = false;

            this.PictureBox.BackColor = Color.Transparent;
            this.Label.BackColor = Color.Transparent;
            this.Label.ForeColor = this.MenuTextColor;
        }
    }

    internal string Text {
        get {
            return this.Label.Text;
        }

        set {
            string strTextNoMnemonic = value.Replace("&", "").Trim();
            string strTextMnemonic = strTextNoMnemonic;
            char chrMnemonic = this.ShortKey;
            if (this.ShortKey != '\0') {
                strTextMnemonic = InjectMnemonic(strTextNoMnemonic, chrMnemonic);
                if (strTextNoMnemonic == strTextMnemonic) {
                    this.ShortKey = '\0';
                }
            }

            this.Label.Text = strTextMnemonic;
            //this could be an option we add as parameter. add a DeltaX
            //this.Label.Text = " " + strTextMnemonic;

            this.Command = strTextNoMnemonic;
        }
    }

    private static string InjectMnemonic(string pstrData, char pchrTarget) {
        string strRet = pstrData;

        if (!string.IsNullOrWhiteSpace(pstrData)) {
            // JIT: Find the first occurrence regardless of case
            int intIndex = pstrData.IndexOf(pchrTarget.ToString(), StringComparison.OrdinalIgnoreCase);

            if (intIndex >= 0) {
                // JIT: Create the final string in a single operation
                strRet = pstrData.Insert(intIndex, "&");
            }
        }

        return strRet;
    }

    private string _Command = string.Empty;
    internal string Command {
        get {
            return this._Command;
        }

        private set {
            string strText = value.Replace("&", "").Trim().ToUpper();
            this._Command = strText;
        }
    }

    private bool Hovering { get; set; } = false;

    private bool _Disable = false;
    internal bool Disable {
        get {
            return this._Disable;
        }

        set {
            bool blnValue = value;
            if (!blnValue) {
                this.Label.ForeColor = this.MenuTextColor;

                this._Disable = blnValue;
            } else {
                bool blnLocked = this.LockedMenu;
                if (!blnLocked) {
                    this.Label.ForeColor = SystemColors.GrayText;
                    //this.Label.ForeColor = Color.Gray;

                    //To Change Font:
                    //Font fntOld = this.Label.Font;
                    //this.Label.Font = new Font(fntOld, FontStyle.Italic);
                    //fntOld.Dispose();

                    this._Disable = blnValue;
                }
            }
        }
    }

    internal char ShortKey {
        get; set;
    } = '\0';

    internal PictureBox PictureBox {
        get; set;
    }

    //LockedMenu cannot be disabled
    internal bool LockedMenu {
        get; set;
    } = false;

    internal Label Label {
        get; private set;
    }

    public override string ToString() {
        return this.Text;
    }

    internal Color MenuTextColor {
        get; private set;
    } = Color.Navy;

    internal Color MenuHoverColor {
        get; private set;
    } = Color.Navy;
}
