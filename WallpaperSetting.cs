using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

class WallpaperSetting {
	
	[DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool SystemParametersInfo (SysParamInf uiAction, uint uiParam, StringBuilder pvParam, SPIUpdateProfileFlags fWinIni);
	
	private enum SysParamInf : uint {
		SPI_GETDESKWALLPAPER = 0x73,
		SPI_SETDESKWALLPAPER = 0x14
	}
	
	private enum SPIUpdateProfileFlags : uint {
		SPIF_DONTUPDATE = 0,
		SPIF_UPDATEINIFILE = 1,
		SPIF_SENDWININICHANGE = 2
	}
	
	private const uint MAX_PATH = 260;
	
	public static FileInfo wallpaperFilename {
		get {
			StringBuilder sb = new StringBuilder((int)MAX_PATH);
			if(!SystemParametersInfo(SysParamInf.SPI_GETDESKWALLPAPER, (uint)sb.Capacity, sb, 0)){
				throw new System.ComponentModel.Win32Exception();
			} else {
				if(sb.Length == 0){
					return null;
				}
				return new FileInfo(sb.ToString());
			}
		}
		set {
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\", true);
			rk.SetValue("TileWallpaper", 1.ToString());
			SystemParametersInfo(SysParamInf.SPI_SETDESKWALLPAPER, 0, new StringBuilder(value.FullName), SPIUpdateProfileFlags.SPIF_UPDATEINIFILE | SPIUpdateProfileFlags.SPIF_SENDWININICHANGE);
		}
	}
	
	private static string joinHashSet<T>(HashSet<T> hs, string joiner){
		StringBuilder sb = new StringBuilder();
		T[] array = new T[hs.Count];
		hs.CopyTo(array);
		for(int i = 0; i < array.Length - 1; ++i){
			sb.Append(array[i].ToString()).Append(joiner);
		}
		sb.Append(array[array.Length - 1]);
		return sb.ToString();
	}
	
	public static void pickRandomMultiScreenWallpaper(DirectoryInfo sourceFolder){
		HashSet<FileInfo> hs = FileChooserStuff.getRandomPictureFiles(FileChooserStuff.sourceDir, System.Windows.Forms.Screen.AllScreens.Length);
		GraphicsStuff.concatPicturesAllScreens(hs, outputFilename);
		if (Logging.loggingEnabled){
			Logging.logText(DateTime.Now.ToString() + " " + joinHashSet(hs, ", "));
		}
		
		wallpaperFilename = outputFilename;
		
	}
	
	private static FileInfo outputFilename {
		get {
			DirectoryInfo appData = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			DirectoryInfo outputDir = new DirectoryInfo(Path.Combine(appData.FullName, System.Windows.Forms.Application.ProductName));
			if(!outputDir.Exists){
				outputDir.Create();
			}
			return new FileInfo(Path.Combine(outputDir.FullName, "out.bmp"));
		}
	}
}