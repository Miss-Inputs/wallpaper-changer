using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

static class GraphicsStuff {
	
	public static Dictionary<string, string> supportedFileTypes {
		get {
			ImageCodecInfo[] icis = ImageCodecInfo.GetImageDecoders();
			Dictionary<string, string> d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			foreach (ImageCodecInfo ici in icis){
				foreach (string ext in ici.FilenameExtension.Split(';')){
					//ici.FileExtension is a semicolon delimited list, we want individual items
					d.Add(ext.TrimStart('*'), ici.CodecName);
				}
			}
			return d;
		}
	}
	
	private static string rectToString(Rectangle r){
		return string.Format("Left: {0}, Top: {1}, Right: {2}, Bottom: {3}, X: {4}, Y: {5}, Width: {6}, Height: {7}", r.Left, r.Top, r.Right, r.Bottom, r.X, r.Y, r.Width, r.Height);
	}
	
	public static void concatPicturesAllScreens(HashSet<FileInfo> pictureFiles, FileInfo outputFilename){
		Rectangle virtualScreen = SystemInformation.VirtualScreen;
		Rectangle primaryScreen = Screen.PrimaryScreen.Bounds;
		
		Screen[] screens = Screen.AllScreens;
		FileInfo[] images = new FileInfo[screens.Length];
		pictureFiles.CopyTo(images);
		
		using(Bitmap outputBitmap = new Bitmap(virtualScreen.Width, virtualScreen.Height))
		using(Graphics g = Graphics.FromImage(outputBitmap)){
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			
			for(int i = 0; i < screens.Length; ++i){
				using(Bitmap bitmap = new Bitmap(images[i].FullName)){
					Rectangle r = screens[i].Bounds;
					if (Logging.loggingEnabled){
						Logging.logText(rectToString(r));
					}
					g.DrawImage(bitmap, Math.Abs(r.Left - primaryScreen.Left), Math.Abs(r.Top - primaryScreen.Top), r.Width, r.Height);
				}
			}
			outputBitmap.Save(outputFilename.FullName, ImageFormat.Bmp);
		}
	}
}