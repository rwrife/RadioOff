namespace RadioOff
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.MainMenu mainMenu1;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.enableMenu = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.updateCheckMenu = new System.Windows.Forms.MenuItem();
			this.OnTimePicker = new System.Windows.Forms.DateTimePicker();
			this.OffTimePicker = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.enabledCb = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.Add(this.enableMenu);
			this.mainMenu1.MenuItems.Add(this.menuItem2);
			// 
			// enableMenu
			// 
			this.enableMenu.Text = "Exit";
			this.enableMenu.Click += new System.EventHandler(this.enableMenu_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.MenuItems.Add(this.updateCheckMenu);
			this.menuItem2.MenuItems.Add(this.menuItem3);
			this.menuItem2.MenuItems.Add(this.menuItem4);
			this.menuItem2.Text = "Menu";
			// 
			// updateCheckMenu
			// 
			this.updateCheckMenu.Text = "Check For Updates";
			this.updateCheckMenu.Click += new System.EventHandler(this.updateCheckMenu_Click);
			// 
			// OnTimePicker
			// 
			this.OnTimePicker.CustomFormat = "hh:mm tt";
			this.OnTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.OnTimePicker.Location = new System.Drawing.Point(3, 44);
			this.OnTimePicker.Name = "OnTimePicker";
			this.OnTimePicker.Size = new System.Drawing.Size(152, 23);
			this.OnTimePicker.TabIndex = 0;
			this.OnTimePicker.Value = new System.DateTime(2007, 12, 27, 10, 10, 0, 0);
			// 
			// OffTimePicker
			// 
			this.OffTimePicker.CustomFormat = "hh:mm tt";
			this.OffTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.OffTimePicker.Location = new System.Drawing.Point(3, 90);
			this.OffTimePicker.Name = "OffTimePicker";
			this.OffTimePicker.Size = new System.Drawing.Size(152, 23);
			this.OffTimePicker.TabIndex = 1;
			this.OffTimePicker.Value = new System.DateTime(2007, 12, 27, 10, 10, 0, 0);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 22);
			this.label1.Text = "On";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 22);
			this.label2.Text = "Off";
			// 
			// menuItem3
			// 
			this.menuItem3.Text = "Radio On";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Text = "Radio Off";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// enabledCb
			// 
			this.enabledCb.Location = new System.Drawing.Point(4, 4);
			this.enabledCb.Name = "enabledCb";
			this.enabledCb.Size = new System.Drawing.Size(152, 22);
			this.enabledCb.TabIndex = 4;
			this.enabledCb.Text = "Enabled";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(176, 180);
			this.Controls.Add(this.enabledCb);
			this.Controls.Add(this.OffTimePicker);
			this.Controls.Add(this.OnTimePicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "RadioOff";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MenuItem enableMenu;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem updateCheckMenu;
		private System.Windows.Forms.DateTimePicker OnTimePicker;
		private System.Windows.Forms.DateTimePicker OffTimePicker;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.CheckBox enabledCb;
	}
}

