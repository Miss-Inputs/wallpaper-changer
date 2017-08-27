using System;
using System.IO;
using Microsoft.Win32;

static class Logging {

	public static bool loggingEnabled {
		get {
			RegistryKey rk = Registry.CurrentUser.OpenSubKey(MainClass.SUBKEY);
			if(rk == null){
				return false;
			}
			try {
				return Convert.ToBoolean(rk.GetValue("loggingEnabled", false));
			} catch (InvalidCastException) {
				return false;
			}
		}
		set {
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(MainClass.SUBKEY);
			rk.SetValue("loggingEnabled", value);
			rk.Close();
		}
	}

	public static void logText(string s){
		if(!loggingEnabled){
			return;
		}
		
		using(StreamWriter sw = new StreamWriter(logFilename, true)){
			sw.WriteLine(s);
		}
	}
	
	public static string logFilename {
		get {
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string outputDir = Path.Combine(appData, System.Windows.Forms.Application.ProductName);
			if(!Directory.Exists(outputDir)){
				Directory.CreateDirectory(outputDir);
			}
			return Path.Combine(outputDir, "log.log");
		}
	}
	
}