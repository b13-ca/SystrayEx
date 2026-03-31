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
//      Usage:  C# Reflection helper
// Dependency:  none
#endregion Usage and dependency

#region History
//    History:
// v1.00 - 2026-03-19:	Initial release;
// v1.01 - 2026-03-30:	removing console dependency;
//                      switching to b13 namespae;
#endregion History

using System.Reflection;

#region b13 namespace
#pragma warning disable IDE0130
namespace b13;
#pragma warning restore IDE0130
#endregion b13 namespace

public static class ReflectionHelper {
    private static readonly BindingFlags _enmBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

    private static Assembly? _asm;
    private static Type? _resourcesType2 = null;

    internal static Type? ResourcesType {
        get {
            return _resourcesType2;
        }

        private set {
            _resourcesType2 = value;
        }
    }

    public static void Initialize(Form pobjForm) {
        if (_asm == null) {
            ArgumentNullException.ThrowIfNull(pobjForm);

            _asm = pobjForm.GetType().Assembly;
            if (_asm != null) {
                ResourcesType = _asm.GetTypes().FirstOrDefault(t => t.Name == "Resources");
            }
        }
    }

    private static Bitmap CreateDefaultImage(int plngWidth, int plngHeight, Color pcolBackground) {
        Bitmap objRet = new Bitmap(plngWidth, plngHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        using (Graphics objGraphics = Graphics.FromImage(objRet)) {
            objGraphics.Clear(pcolBackground);
        }

        return objRet;
    }

    public static Icon GetIconFromRessource(string pstrName) {
        Icon? objRet = SystemIcons.Application;

        if (ReflectionHelper.ResourcesType != null) {
            PropertyInfo? prop = ReflectionHelper.ResourcesType.GetProperty(pstrName, _enmBindingFlags);

            if (prop != null) {
                try {
                    Icon? tmpRet = prop.GetValue(null) as Icon;
                    if (tmpRet != null) {
                        objRet = tmpRet;
                    }
                } catch {
                    // Error come from that: var tmpRet = prop.GetValue(null);
                    // This seem to occur when you copy a project, rename file manually and try
                    // to run the project without erasing then rebuilding the [Ressources.resx] file.
                    // maybe it's because the [Ressources] directory moved ?
                    MessageBox.Show("[Ressources.resx] doesn't match your project", "Error");
                }
            }
        }

        return objRet;
    }

    public static Image GetImageFromRessource(string pstrName) {
        Image objRet = CreateDefaultImage(1, 1, Color.Transparent);

        if (ReflectionHelper.ResourcesType != null) {
            PropertyInfo? prop = ReflectionHelper.ResourcesType.GetProperty(pstrName, _enmBindingFlags);

            if (prop != null) {
                try {
                    Image? tmpRet = prop.GetValue(null) as Image;
                    if (tmpRet != null) {
                        objRet = tmpRet;
                    }
                } catch {
                    // Error come from that: var tmpRet = prop.GetValue(null);
                    // This seem to occur when you copy a project, rename file manually and try
                    // to run the project without erasing then rebuilding the [Ressources.resx] file.
                    // maybe it's because the [Ressources] directory moved ?
                    MessageBox.Show("[Ressources.resx] doesn't match your project", "Error");
                }
            }
        }

        return objRet;
    }

    internal static void SetClassInfo(Type type, out string pstrClassName, out string pstrBaseClass) {
        pstrClassName = "";
        pstrBaseClass = "";

        if (type != null) {
            //type.Name.ShowWhatItIs();
            //pstrClassName = $"{sCF}O{type.Name}{sCF}A";
            pstrClassName = $"{type.Name}";

            if (type.BaseType != null) {
                //pstrBaseClass = $"{sCF}L{type.BaseType.Name}{sCF}A";
                pstrBaseClass = $"{type.BaseType.Name}";
            }
        }
    }

    internal static string GetAppMutexName(bool pblnGlobal = false) {
        string strAssemblyId = "";

        if (_asm != null) {
            string strAssemblyName = _asm.GetName().Name ?? "";
            byte[] hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(strAssemblyName));
            strAssemblyId = Convert.ToHexString(hash).Substring(0, 16);
            if (pblnGlobal) {
                strAssemblyId = @"Global\" + strAssemblyId;
            }
        }

        return strAssemblyId;
    }
}
