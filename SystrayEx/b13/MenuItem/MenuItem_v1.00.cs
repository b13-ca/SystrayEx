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
// Dependency:  MenuItemData, ReflectionHelper, RessourceHelperEx
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

public class MenuItemEx : IDisposable {
    private bool _disposed = false;

    internal event EventHandler<string>? OnMenuItem_Click;
    internal const int MAX_ICON_HEIGHT = 32;

    private readonly Form _objForm;
    private readonly List<MenuItemData> _lstMenuItem;
    private readonly Dictionary<char, MenuItemData> _dicCommand;

    //Constructor
    public MenuItemEx(Form pobjForm, string pstrBackgroundColor = "#F0F0F0", int pintBorderWidth = 3, string pstrBorderColor = "#000080", int pintMenuDeltaY = -1, int pintMenuSpaceDeltaY = 2, string pstrMenuTextColor = "#000080", string pstrMenuHoverColor = "#000080") {
        this._lstMenuItem = [];
        this._dicCommand = [];

        this._objForm = pobjForm;
        this.BackgroundColor = GetHtmlColor(pstrBackgroundColor, SystemColors.Control);
        this._objForm.BackColor = this.BackgroundColor;

        this.BorderHeight = pintBorderWidth;
        this.BorderColor = GetHtmlColor(pstrBorderColor, Color.Navy);
        this.MenuDeltaY = pintMenuDeltaY;
        this.MenuSpaceDeltaY = pintMenuSpaceDeltaY;

        this.MenuTextColor = GetHtmlColor(pstrMenuTextColor, Color.Red);
        this.MenuHoverColor = GetHtmlColor(pstrMenuHoverColor, Color.Red);
    }

    //Deconstructor
    //because we ovverride and not just virtual:  protected override void Dispose(bool disposing) {
    //we don't need a dispose()
    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    //override  virtual if not heriting Form
    protected virtual void Dispose(bool disposing) {
        if (_disposed) {
            return;
        }

        if (disposing) {
            // Dispose managed objects
            foreach (MenuItemData objMenuItem in _lstMenuItem) {
                objMenuItem.PictureBox.Image = null;    //avoid disposing of image;
                objMenuItem.PictureBox.Dispose();
                objMenuItem.Label.Dispose();
            }
        }

        // Dispose unmanaged objects here (if any)

        // when Override (and not just Virual)
        // Always call the base to let the Form clean up its own handles
        //base.Dispose(disposing);

        _disposed = true;
    }

    internal void RaiseMenuItemClick(string pstrCommand) {
        if (!string.IsNullOrWhiteSpace(pstrCommand)) {
            OnMenuItem_Click?.Invoke(this, pstrCommand);
        }
    }

    private static Color GetHtmlColor(string pstrHtmlColor, Color pcolDefault) {
        Color colRet;

        try {
            colRet = ColorTranslator.FromHtml(pstrHtmlColor);
        } catch {
            colRet = pcolDefault;
        }

        return colRet;
    }

    //internal void Add(string pstrText) {
    internal void Add(Image pobjIcon, string pstrText) {
        if (pobjIcon.Width > MAX_ICON_HEIGHT || pobjIcon.Height > MAX_ICON_HEIGHT) {
            throw new ArgumentException("invalid Icon image passed");
        }

        PictureBox objPictureBox = new() {
            SizeMode = PictureBoxSizeMode.Zoom,
            Size = new Size(MAX_ICON_HEIGHT, MAX_ICON_HEIGHT),
            Image = pobjIcon
        };

        Label objLabel = new Label() {
            //Font = new Font(this._objForm.Font, FontStyle.Italic | FontStyle.Bold),
            Font = new Font(this._objForm.Font, FontStyle.Italic),
            AutoSize = false,
            ForeColor = this.MenuTextColor,
            TextAlign = ContentAlignment.MiddleLeft
        };

        char chrShortcutKey = GetMnemonicChar(ref pstrText);
        MenuItemData tmpMenuItemData = new MenuItemData(this, objPictureBox, objLabel, pstrText, chrShortcutKey);
        if (chrShortcutKey != '\0') {
            //string strCommand = tmpMenuItemData.Command;
            this._dicCommand[chrShortcutKey] = tmpMenuItemData;
        }

        if (this._lstMenuItem.Count == 0) {
            tmpMenuItemData.LockedMenu = true;
        }
        this._lstMenuItem.Add(tmpMenuItemData);
    }

    internal static char GetMnemonicChar(ref string pstrText) {
        string strRet = pstrText.Replace("&", "").Trim();

        char chrMnemonic = '\0'; // Variable de retour initialized to default
        if (!string.IsNullOrWhiteSpace(pstrText)) {
            ReadOnlySpan<char> span = pstrText.AsSpan().Trim().TrimEnd('&');
            int intIndex = span.IndexOf('&');
            if (intIndex >= 0 && intIndex < span.Length - 1) {
                char chrNewMnemonic = char.ToUpper(span[intIndex + 1]);
                if (chrNewMnemonic != '&') {
                    chrMnemonic = chrNewMnemonic;
                }
            }
        }

        pstrText = strRet;
        return chrMnemonic;
    }

    //internal bool DoProcessCmdKey(ref Message msg, Keys keyData) {
    internal bool DoProcessCmdKey(Keys keyData) {
        bool blnRet = false;

        // 1. Isolate the modifier and the actual key code
        bool blnAltPressed = (keyData & Keys.Modifiers) == Keys.Alt;
        if (blnAltPressed) {
            // 2. Check we're not alone (just Alt)
            Keys enmKey = keyData & Keys.KeyCode;
            if (enmKey != Keys.Menu) {
                // 2. Extract the character (it will be uppercase, e.g., 88 for 'X')
                char chrKey = (char)enmKey;
                //System.Diagnostics.Debug.WriteLine($"Alt+{chrKey} pressed");

                // 3. Search in your dictionary (case-insensitive usually best)
                string strCommand = this.GetCommand(char.ToUpper(chrKey));
                if (strCommand.Length > 0) {
                    // Use the verified command
                    this.RaiseMenuItemClick(strCommand);
                    blnRet = true;
                }
            }
        }

        return blnRet;
    }

    internal MenuItemData this[int pintIndex] {
        get {
            return this._lstMenuItem[pintIndex];
        }
    }

    //In order to expose [IEnumerator<MenuItemData>] you need a signature like:
    //public class MenuItemEx : IEnumerable<MenuItemData>, IDisposable {

    //but then, [MenuItemData] class need to be public, I don't think in a Nuget Package it
    //would be a good idea. Therefore, I remove the [GetEnumerator] possibility
    //public IEnumerator<MenuItemData> GetEnumerator() {
    //    return this._lstMenuItem.GetEnumerator();
    //}

    //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
    //    return this._lstMenuItem.GetEnumerator();
    //}

    internal int Count {
        get {
            return this._lstMenuItem.Count;
        }
    }

    //we want thoses properties to be readonly, set upon constructor creation only
    internal int BorderHeight {
        get; private set;
    } = 3;

    internal Color MenuTextColor {
        get; private set;
    } = Color.Navy;

    internal Color MenuHoverColor {
        get; private set;
    } = Color.Navy;

    internal Color BackgroundColor {
        get; private set;
    } = SystemColors.Control;

    internal Color BorderColor {
        get; private set;
    } = Color.Navy;

    internal int MenuSpaceDeltaY {
        get; private set;
    } = 2;

    internal int MenuDeltaY {
        get; private set;
    } = -1;

    internal int GetMenuHeight() {
        int intPenDeltaY = this.BorderHeight + 1;
        int intMenuDeltaY = this.MenuSpaceDeltaY;

        int intRet = intPenDeltaY + (this.Count + 1) * (MAX_ICON_HEIGHT + intMenuDeltaY);
        intRet = intRet - intMenuDeltaY + intPenDeltaY;

        return intRet;
    }

    internal int GetMenuItemWidth() {
        int intRet = 0;

        foreach (MenuItemData objMenuItem in this._lstMenuItem) {
            Size szText = GetStringSize(objMenuItem.Label.Text, objMenuItem.Label.Font);
            if (intRet < szText.Width) {
                intRet = szText.Width;
            }
        }

        return intRet;
    }

    internal int GetMenuTop() {
        int intRet = this.BorderHeight + MAX_ICON_HEIGHT + 3;

        return intRet;
    }

    internal Image CreateUI() {
        int intPenSize = this.BorderHeight;

        int intMenuItemWidth = this.GetMenuItemWidth() + 10;
        int intClientWidth = intMenuItemWidth + 2 + MAX_ICON_HEIGHT + (2 * intPenSize);

        int intDeltaY = this.MenuSpaceDeltaY;
        int intIconPosX = intPenSize + 1;

        this._objForm.Height = this.GetMenuHeight();
        int intClientHeight = this._objForm.ClientSize.Height;

        Image imgLeftBgImage = ReflectionHelper.GetImageFromRessource("background");

        //this is used for debugging
        int IsValid = imgLeftBgImage.Width;
        //IsValid = 1;

        if (IsValid > 1) {
            //it enter here only if the imgLeftBgImage is valid
            int intTargetHeight = intClientHeight - (2 * intPenSize) - 2;
            Size szNew = RessourceHelperEx.ScaleSize(imgLeftBgImage.Width, imgLeftBgImage.Height, 0, intTargetHeight);

            Image imgTmpLeftMenu = RessourceHelperEx.ResizeImage(imgLeftBgImage, szNew.Width, szNew.Height);

            imgLeftBgImage.Dispose();
            imgLeftBgImage = imgTmpLeftMenu;

            int intLeftImageNewWidth = imgLeftBgImage.Width;
            intClientWidth = intClientWidth + intLeftImageNewWidth + 1;
            intIconPosX = intIconPosX + intLeftImageNewWidth + 1;
        }
        this._objForm.Width = intClientWidth;

        using Image imgBase = RessourceHelperEx.CreateDefaultImage(intClientWidth, intClientHeight, this.BackgroundColor);
        using Image imgBordered = RessourceHelperEx.AddRectangleBorder(imgBase, this.BorderColor, intPenSize);

        Image imgRet;
        if (IsValid > 1) {
            imgRet = RessourceHelperEx.CombineImages(imgBordered, imgLeftBgImage, intPenSize + 1, intPenSize + 1);
        } else {
            imgRet = (Image)imgBordered.Clone();
        }
        imgLeftBgImage.Dispose();

        int intMenuTop = this.GetMenuTop();
        for (int intIndex = this.Count - 1; intIndex >= 0; intIndex--) {
            MenuItemData objMenuItemData = this[intIndex];

            objMenuItemData.PictureBox.Location = new Point(intIconPosX, intMenuTop);
            objMenuItemData.Label.Location = new Point(intIconPosX + MAX_ICON_HEIGHT, intMenuTop);
            objMenuItemData.Label.Size = new Size(intMenuItemWidth, MAX_ICON_HEIGHT);

            //intIconPosX ************** DEBUGGING
            //objMenuItemData.PictureBox.BackColor = Color.Cyan;
            //objMenuItemData.Label.BackColor = Color.Red;

            this._objForm.Controls.Add(objMenuItemData.PictureBox);
            this._objForm.Controls.Add(objMenuItemData.Label);

            intMenuTop = intMenuTop + MAX_ICON_HEIGHT + intDeltaY;
        }

        return imgRet;
    }

    internal string GetCommand(char pchrMnemonic) {
        string strRet = "";

        _ = this._dicCommand.TryGetValue(pchrMnemonic, out MenuItemData? objMenuItemData);
        if (objMenuItemData != null) {
            if (!objMenuItemData.Disable) {
                strRet = objMenuItemData.Command;
            }
        }

        return strRet;
    }

    private static Size GetStringSize(string pstrText, Font pobjFont) {
        // Initialisation JIT de la variable de retour avec une valeur par défaut
        Size szRet = Size.Empty;

        if (!string.IsNullOrWhiteSpace(pstrText) && pobjFont != null) {
            // Mesure GDI (plus précise pour le rendu WinForms standard)
            szRet = TextRenderer.MeasureText(pstrText, pobjFont);
        }

        return szRet;
    }
}
