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
//      Usage:  Ressource Image handling
// Dependency:  none
#endregion Usage and dependency

#region History
//    History:
// v1.00 - 2026-03-30:	Initial release;
#endregion History

#region b13 namespace
#pragma warning disable IDE0130
namespace b13;
#pragma warning restore IDE0130
#endregion b13 namespace

public static class RessourceHelperEx {
    public static Size ScaleSize(int pintWidth, int pintHeight, int pintNewWidth = 0, int pintNewHeight = 0) {
        int intNewWidth = pintNewWidth;
        int intNewHeight = pintNewHeight;

        float sngProportion;
        if (pintWidth > 0 && pintHeight > 0) {
            sngProportion = (float)(pintHeight * 1.0 / pintWidth);
            if (pintNewWidth > 0) {
                intNewHeight = (int)Math.Ceiling(pintNewWidth / sngProportion);
            } else if (pintNewHeight > 0) {
                intNewWidth = (int)Math.Ceiling(pintNewHeight / sngProportion);
            }
        }

        Size szRet = new Size(intNewWidth, intNewHeight);
        return szRet;
    }

    public static Bitmap CreateDefaultImage(int plngWidth, int plngHeight, Color pcolBackground) {
        Bitmap objRet = new Bitmap(plngWidth, plngHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        using (Graphics objGraphics = Graphics.FromImage(objRet)) {
            objGraphics.Clear(pcolBackground);
        }

        return objRet;
    }

    public static Bitmap CombineImages(Image pobjImage, Image pobjOverlay, int pintX, int pintY) {
        Bitmap objRet = CreateDefaultImage(pobjImage.Width, pobjImage.Height, Color.Transparent);

        using (Graphics objGraphics = Graphics.FromImage(objRet)) {
            // draw background first
            objGraphics.DrawImage(pobjImage, 0, 0, pobjImage.Width, pobjImage.Height);

            // draw overlay at specific position
            objGraphics.SetClip(new Rectangle(0, 0, objRet.Width, objRet.Height));
            objGraphics.DrawImage(pobjOverlay, pintX, pintY, pobjOverlay.Width, pobjOverlay.Height);
        }

        return objRet;
    }

    public static Bitmap AddRectangleBorder(Image pobjImage, Color pcolBorder, int pintPenSize = 1) {
        Bitmap objRet = CreateDefaultImage(pobjImage.Width, pobjImage.Height, Color.Transparent);

        using (Graphics objGraphics = Graphics.FromImage(objRet)) {
            //objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //objGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            objGraphics.DrawImage(pobjImage, 0, 0);

            //Fixed position
            using (Pen objPen = new Pen(pcolBorder, pintPenSize)) {
                int intPen = pintPenSize;
                int intHalf = intPen / 2;
                objGraphics.DrawRectangle(
                    objPen,
                    intHalf,
                    intHalf,
                    pobjImage.Width - intPen,
                    pobjImage.Height - intPen
                );
            }

            //Bleed position
            //int intPenSize = pintPenSize * 2;
            //using (Pen objPen = new Pen(pcolBorder, intPenSize)) {
            //    objGraphics.DrawRectangle(objPen, 0, 0, pobjImage.Width - 1, pobjImage.Height - 1);
            //}
        }

        return objRet;
    }

    public static Image GetImageFromList(ImageList pimgList, string pstrName, int pintResizedHeight = 32) {
        Image objRet;

        Image? tmpIcon = pimgList.Images[pstrName];
        if (tmpIcon != null) {
            if (pintResizedHeight > 0) {
                //Resize Asked
                Size szNew = RessourceHelperEx.ScaleSize(tmpIcon.Width, tmpIcon.Height, 0, pintResizedHeight);
                objRet = RessourceHelperEx.ResizeImage(tmpIcon, szNew.Width, szNew.Height);
                tmpIcon.Dispose();
            } else {
                objRet = tmpIcon;
            }
        } else {
            objRet = CreateDefaultImage(1, 1, Color.Transparent);
        }

        return objRet;
    }

    public static Bitmap ResizeImage(Image pobjImage, int pintNewWidth, int pintNewHeight) {
        Bitmap objRet = CreateDefaultImage(pintNewWidth, pintNewHeight, Color.Transparent);

        using (Graphics objGraphics = Graphics.FromImage(objRet)) {
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            objGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            objGraphics.DrawImage(pobjImage, 0, 0, pintNewWidth, pintNewHeight);
        }

        return objRet;
    }
}