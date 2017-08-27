using System;
using System.Windows.Forms;

class TrayIcon {
	private NotifyIcon ni = new NotifyIcon();
#if NON_NATIVE_CONTEXT_MENU
	private ContextMenuStrip cms = new ContextMenuStrip();
#else
	private ContextMenu cm = new ContextMenu();
#endif

	public TrayIcon(){
	#if NON_NATIVE_CONTEXT_MENU
		ni.ContextMenuStrip = cms;
		cms.Items.Add(new ToolStripMenuItem("Settings", null, settings));
		cms.Items.Add(new ToolStripMenuItem("Change Wallpaper Now", null, changeWallpaperNow));
		cms.Items.Add(new ToolStripSeparator());
		cms.Items.Add(new ToolStripMenuItem("About", null, about));
		cms.Items.Add(new ToolStripMenuItem("Exit", null, exit));
	#else
		ni.ContextMenu = cm;
		cm.MenuItems.Add("Settings", settings);
		cm.MenuItems.Add("Change Wallpaper Now", changeWallpaperNow);
		cm.MenuItems.Add("-");
		cm.MenuItems.Add("About", about);
		cm.MenuItems.Add("Exit", exit);
	#endif
		ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
	}

	public void setupIcon(){
		ni.Visible = true;
	}
	
	public void removeIcon(){
		ni.Visible = false;
	}
	
	void exit(object sender, EventArgs e){
		removeIcon();
		Application.Exit();
	}
	
	static void about(object sender, EventArgs e){
		new AboutBox().ShowDialog();
	}
	
	static void changeWallpaperNow(object sender, EventArgs e){
		MainClass.scheduledWallChange(null);
	}
	
	static void settings(object sender, EventArgs e){
		new SettingsDialog().ShowDialog();
	}
}