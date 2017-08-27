using System;
using System.Drawing;
using System.Windows.Forms;

class AboutBox : Form {
	private Button closeButton = new Button();
	private Label aboutText = new Label(){Text=Application.ProductName + " " + Application.ProductVersion + "\nAuthor: Megan Leet", Location=new Point(10, 10), AutoSize=true};
	public AboutBox(){
		FormBorderStyle = FormBorderStyle.FixedSingle; 
		Text = "About " + Application.ProductName;
		closeButton.Text = "Close";
		closeButton.Location = new Point((ClientRectangle.Width - closeButton.Width) - 10, (ClientRectangle.Height - closeButton.Height) - 10);
		closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		Controls.Add(closeButton);
		closeButton.Click += new System.EventHandler(delegate(object sender, EventArgs e){Close();});
		
		MaximizeBox = false;
		AcceptButton = closeButton;
		CancelButton = closeButton;
		
		Controls.Add(aboutText);
		
		Width = (int)((aboutText.Width + closeButton.Width + 10) * 1.25);
		Height = (aboutText.Height + closeButton.Height + 10) * 2;
	}
}
