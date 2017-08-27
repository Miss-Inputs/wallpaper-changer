using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyProduct("Megan's wallpaper changer"), AssemblyVersion("2.2"), System.Runtime.InteropServices.ComVisible(true), CLSCompliant(true)]
static class MainClass {

	public static readonly string SUBKEY = "Software\\" + Application.ProductName;

	private static System.Threading.Timer tim;
	
	[System.STAThread]
	static void Main(){
		try {
			new TrayIcon().setupIcon();
			tim = new System.Threading.Timer(scheduledWallChange, null, 0, (int)TimeSpan.FromMinutes(interval).TotalMilliseconds);
			SystemEvents.DisplaySettingsChanged += new EventHandler(delegate(object unused, EventArgs alsoUnused){scheduledWallChange(unused);});
			Application.Run();
		} catch (System.Exception ex) {
			MessageBox.Show(ex.ToString());
		} finally {
			SystemEvents.DisplaySettingsChanged -= new EventHandler(delegate(object unused, EventArgs alsoUnused){scheduledWallChange(unused);});
		}
	}
	
	public static void scheduledWallChange(object unused){
		if(FileChooserStuff.sourceDir != null){
			WallpaperSetting.pickRandomMultiScreenWallpaper(FileChooserStuff.sourceDir);
		}
	}
	
	static public uint interval {
		get {
			const uint DEFAULT = 5;
			RegistryKey rk = Registry.CurrentUser.OpenSubKey(MainClass.SUBKEY);
			if(rk == null){
				return DEFAULT;
			}
			try {
				return (uint)rk.GetValue("interval", DEFAULT);
			} catch (InvalidCastException) {
				return DEFAULT;
			}
		}
		set {
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(MainClass.SUBKEY);
			rk.SetValue("interval", value);
			rk.Close();
		}
	}
}
