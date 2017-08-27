using System;
using System.Drawing;
using System.Windows.Forms;

class SettingsDialog : Form {

	private Label sourceDirLabel = new Label(){Text="Source directory:"};
	private TextBox sourceDirBox = new TextBox();
	private Button sourceDirButton = new Button(){Text="Browse..."};
	private CheckBox logCheck = new CheckBox();
	private Label intervalLabel = new Label(){Text="Interval:"};
	private NumericUpDown intervalSpin = new NumericUpDown();
	
	private Button saveButton = new Button(){Text="Save"};
	private Button applyButton = new Button(){Text="Apply"};
	private Button cancelButton = new Button(){Text="Cancel"};
	
	private TableLayoutPanel sourceDir = new TableLayoutPanel(){ColumnCount=3, RowCount=1};
	
	public SettingsDialog(){
		MaximizeBox = false;
		Text = "Settings";
		
		Controls.Add(sourceDir);
		sourceDir.AutoSize = true;
		sourceDir.Dock = DockStyle.Top;
		sourceDir.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
		sourceDir.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
		sourceDir.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

		sourceDirLabel.Top = 10;
		sourceDirLabel.Left = 10;
		sourceDirLabel.Dock = DockStyle.Fill;
		sourceDir.Controls.Add(sourceDirLabel, 0, 0);
		
		sourceDirBox.Top = sourceDirLabel.Top;
		sourceDirBox.Left = sourceDirLabel.Right + 10;
		sourceDirBox.Dock = DockStyle.Fill;
		sourceDirBox.Text = FileChooserStuff.sourceDir.FullName;
		sourceDir.Controls.Add(sourceDirBox, 1, 0);
		
		sourceDirButton.Top = sourceDirLabel.Top;
		sourceDirButton.Left = sourceDirBox.Right + 10;
		sourceDirButton.Dock = DockStyle.Top;
		sourceDirButton.Click += new System.EventHandler(sourceDirButtonClick);
		sourceDir.Controls.Add(sourceDirButton, 2, 0);
		Width *= 2;
		MinimumSize = new Size(Width, Height);
		
		Controls.Add(logCheck);
		logCheck.Text = "Log activity to " + System.IO.Path.GetFileName(Logging.logFilename);
		logCheck.Top = sourceDir.Bottom + 10;
		logCheck.Left = sourceDir.Left;
		logCheck.AutoSize = true;
		logCheck.Checked = Logging.loggingEnabled;

		Controls.Add(intervalLabel);
		intervalLabel.Left = logCheck.Left;
		intervalLabel.Top = logCheck.Bottom + 10;
		intervalLabel.AutoSize = true;
		
		Controls.Add(intervalSpin);
		intervalSpin.Minimum = 1;
		intervalSpin.Left = intervalLabel.Right + 10;
		intervalSpin.Top = intervalLabel.Top;
		intervalSpin.Value = MainClass.interval;
		
		Controls.Add(saveButton);
		saveButton.Top = (ClientRectangle.Height - saveButton.Height) - 10;
		saveButton.Left = intervalLabel.Left;
		saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		saveButton.Click += new System.EventHandler(delegate(object sender, EventArgs e){if(applySettings()){Close();}});
		
		Controls.Add(applyButton);
		applyButton.Top = saveButton.Top;
		applyButton.Left = saveButton.Right + 10;
		applyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		applyButton.Click += new System.EventHandler(delegate(object sender, EventArgs e){applySettings();});
		
		Controls.Add(cancelButton);
		cancelButton.Top = applyButton.Top;
		cancelButton.Left = applyButton.Right + 10;
		cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		cancelButton.Click += new System.EventHandler(delegate(object sender, EventArgs e){Close();});
	}
	
	private bool applySettings(){
		if(!System.IO.Directory.Exists(sourceDirBox.Text)){
			MessageBox.Show("That folder does not exist.");
			sourceDirBox.Text = FileChooserStuff.sourceDir.FullName;
			return false;
		}
		FileChooserStuff.sourceDir = new System.IO.DirectoryInfo(sourceDirBox.Text);
		
		Logging.loggingEnabled = logCheck.Checked;
		
		if(!intervalSpin.Validate()){
			//Should not matter because it auto-validates when exiting the control anyway, but just in case
			MessageBox.Show("Please only enter numbers into the interval box.");
			intervalSpin.Value = MainClass.interval;
			return false;
		}
		MainClass.interval = (uint)intervalSpin.Value;
		
		return true;
	}
	
	private void sourceDirButtonClick(object sender, EventArgs e){
		FolderBrowserDialog fbd = new FolderBrowserDialog();
		if(fbd.ShowDialog() == DialogResult.OK){
			sourceDirBox.Text = fbd.SelectedPath;
		}
	}
}