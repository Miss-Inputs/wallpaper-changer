using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

static class FileChooserStuff {
	
	public static DirectoryInfo sourceDir {
		get {
			RegistryKey rk = Registry.CurrentUser.OpenSubKey(MainClass.SUBKEY);
			if(rk == null){
				return null;
			}
			try {
				return new DirectoryInfo((string)rk.GetValue("sourceDir"));
			} catch (InvalidCastException) {
				return null;
			} catch (ArgumentNullException) { //Thrown by DirectoryInfo constructor
				return null;
			} catch (ArgumentException) {
				return null;
			}
		}
		set {
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(MainClass.SUBKEY);
			rk.SetValue("sourceDir", value.FullName);
			rk.Close();
		}
	}
	
	private static Random randall = new Random();
	public static HashSet<FileInfo> getRandomPictureFiles(DirectoryInfo folder, int count){
		List<FileInfo> files = getPictureFiles(folder);
	
		if (files.Count < count){
			throw new IOException("There are not enough files in this folder: You wanted " + count + ", there are only " + files.Count);
		}
	
		HashSet<FileInfo> hs = new HashSet<FileInfo>(new FileInfoComparer());
		
		while(hs.Count < count){
			FileInfo f;
			do {
				f = files[randall.Next(files.Count)];
			} while (hs.Contains(f));
			hs.Add(f);
		}
		
		return hs;
	}
	
	private class FileInfoComparer: EqualityComparer<FileInfo> {
		public override bool Equals(FileInfo x, FileInfo y){
			if (x == null){
				return y == null;
			}
			return x.FullName == y.FullName;
		}
		public override int GetHashCode(FileInfo fi){
			return fi.FullName.GetHashCode();
		}
	}
	
	public static List<FileInfo> getPictureFiles(DirectoryInfo folder) {
		List<FileInfo> a = new List<FileInfo>();
		Dictionary<string, string> imageFiletypes = GraphicsStuff.supportedFileTypes;
		foreach(FileInfo f in folder.GetFiles()){
			if(imageFiletypes.ContainsKey(f.Extension)){
				a.Add(f);
			}
		}
		return a;
	}	
}
