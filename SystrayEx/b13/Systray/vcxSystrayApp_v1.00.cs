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
// Dependency:  SystrayAppContext, MenuItemEx
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

//in Program.cs, replace [Application.Run] by [SystrayApp.Run]
//You must embed [systrayIcon1.ico], [systrayIcon2.ico] and [systrayIcon3.ico] in [Ressources.resx]
internal static class SystrayApp {
    internal static void Run(Form pobjForm) {
        ReflectionHelper.Initialize(pobjForm);

        bool blnError = true;
        if (_SystrayContext == null) {   //act as a Singleton
            //https://odetocode.com/blogs/scott/archive/2004/08/20/the-misunderstood-mutex.aspx
            //https://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
            string objAppMutex = ReflectionHelper.GetAppMutexName();
            if (objAppMutex.Length > 0) {
                using (Mutex objMutex = new Mutex(false, objAppMutex)) {
                    if (objMutex.WaitOne(0, false)) {
                        _SystrayContext = new SystrayAppContext(pobjForm);

                        if (pobjForm is ISystrayAware objAware) {
                            objAware.SetSystrayService(_SystrayContext);
                        }
                        Application.Run(_SystrayContext);
                        blnError = false;
                    }
                }
            } else {
                throw new Exception("unable to get assembly name");
            }
        }

        if (blnError) {
            MessageBox.Show("an instance is already running", "Error");
        }
    }

    internal static SystrayAppContext? _SystrayContext = null;
    internal static SystrayAppContext Context {
        get {
            return _SystrayContext!;
        }
    }
}

public class BaseFormEx : Form {
    // This is the "Hook" your specific Forms will fill
    protected virtual MenuItemEx? CommandHandler => null;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
        bool blnRet = false; // Variable de retour

        // JIT: Check if the inheriting form provided a handler
        MenuItemEx? objHandler = this.CommandHandler;

        if (objHandler != null) {
            //blnRet = objHandler.DoProcessCmdKey(ref msg, keyData);
            blnRet = objHandler.DoProcessCmdKey(keyData);
        }

        // Fallback to OS logic if not handled
        if (!blnRet) {
            blnRet = base.ProcessCmdKey(ref msg, keyData);
        }

        return blnRet;
    }
}

//https://www.c-sharpcorner.com/UploadFile/f9f215/how-to-minimize-your-application-to-system-tray-in-C-Sharp/
//https://social.msdn.microsoft.com/Forums/vstudio/en-US/0913ae1a-7efc-4d7f-a7f7-58f112c69f66/c-application-system-tray-icon?forum=csharpgeneral
//https://www.codeproject.com/Articles/18683/Creating-a-Tasktray-Application
//https://stackoverflow.com/questions/995195/how-can-i-make-a-net-windows-forms-application-that-only-runs-in-the-system-tra

//Regular usage of Context Menu for Systray App:
//private void Form1_Load(object sender, EventArgs e) {
//    this.menuItem1 = new System.Windows.Forms.MenuItem();
//    this.contextMenu1 = new System.Windows.Forms.ContextMenu();

//    // Initialize menuItem1
//    this.menuItem1.Index = 0;
//    this.menuItem1.Text = "E&xit";
//    this.menuItem1.Click += this.MenuItem1_Click;

//    // Initialize contextMenu1
//    this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1 });

//    // The ContextMenu property sets the menu that will
//    // appear when the systray icon is right clicked.
//    notifyIcon1.ContextMenu = this.contextMenu1;
//}

//private void MenuItem1_Click(object sender, EventArgs e) {
//    // Close the form, which closes the application.
//    Application.Exit();
//}