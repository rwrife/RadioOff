using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenNETCF.Win32.Notify;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;

namespace RadioOff
{
	public partial class Form1 : Form
	{
		const string appname = "radiooff";
		const int appmajor = 2;
		const int appminor = 0;
		const int appbuild = 1;

		[DllImport("coredll.dll")]
		static extern int SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder lpszPath, int nFolder, int fCreate);
		[DllImport("coredll.dll")]
		static extern int SHCreateShortcut(StringBuilder szShortcut, StringBuilder szTarget);

		const int CSIDL_PROGRAMS = 2;  // \Windows\Start Menu\Programs
		const int CSIDL_STARTUP = 7; // \Windows\Startup

		DateTime offTime = DateTime.MinValue;
		DateTime onTime = DateTime.MinValue;

		bool enabled = false;

		public Form1()
		{
			InitializeComponent();

			//StringBuilder programs = new StringBuilder(255);
			//SHGetSpecialFolderPath((IntPtr)0, programs, CSIDL_STARTUP, 0);
			//StringBuilder shortcutLocation = new StringBuilder(Path.Combine(programs.ToString(), "RadioOff.lnk"));

			//if (!System.IO.File.Exists(shortcutLocation.ToString()))
			//    enabled = false;
			//else
			//    enabled = true;

			//if (enabled)
			//    enableMenu.Text = "Disable";
			//else
			//    enableMenu.Text = "Enable";

			if (!enabled)
				enabledCb.Checked = enabled = Convert.ToBoolean(Convert.ToInt16(RadioOffRegKey.GetValue("Enabled", "0")));

			OffTimePicker.Value = offTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OffTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "10:00 PM")));
			OnTimePicker.Value = onTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OnTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "7:00 AM")));
		}

		private void enableMenu_Click(object sender, EventArgs e)
		{
			string shortcutTarget = new StringBuilder("\"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\RadioOff.exe" + "\"").ToString();

			//DateTime.Now.TimeOfDay.CompareTo(OffTimePicker.Value.TimeOfDay) = -1

			if (!enabledCb.Checked)
			{
				//DeleteShortcut();
				RadioOffRegKey.SetValue("Enabled", "0");
								
				OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, DateTime.MinValue);
				OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.None);
				//enableMenu.Text = "Enable";
				enabled = false;
			}
			else
			{
				//CreateShortcut();
				OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.None);
				OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.RestoreEnd);

				if (OnTimePicker.Value.TimeOfDay.CompareTo(OffTimePicker.Value.TimeOfDay) == -1)
				{
					if (DateTime.Now.TimeOfDay.CompareTo(OnTimePicker.Value.TimeOfDay) >= 0 && DateTime.Now.TimeOfDay.CompareTo(OffTimePicker.Value.TimeOfDay) == -1)
					{
						RadioOn();
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + OffTimePicker.Value.ToShortTimeString()));
					}
					else if (DateTime.Now.TimeOfDay.CompareTo(OffTimePicker.Value.TimeOfDay) >= 0)
					{
						RadioOff();
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + OnTimePicker.Value.ToShortTimeString()));
					}else
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + OffTimePicker.Value.ToShortTimeString()));
				}
				else
				{
					if (DateTime.Now.TimeOfDay.CompareTo(OnTimePicker.Value.TimeOfDay) == -1 && DateTime.Now.TimeOfDay.CompareTo(OffTimePicker.Value.TimeOfDay) >= 0)
					{
						RadioOff();						
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + OnTimePicker.Value.ToShortTimeString()));
					}
					else if (DateTime.Now.TimeOfDay.CompareTo(OnTimePicker.Value.TimeOfDay) >= 0)
					{
						RadioOn();
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + OffTimePicker.Value.ToShortTimeString()));
					}else
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + OnTimePicker.Value.ToShortTimeString()));
				}

				RadioOffRegKey.SetValue("OnTime", OnTimePicker.Value.ToString());
				RadioOffRegKey.SetValue("OffTime", OffTimePicker.Value.ToString());

				RadioOffRegKey.SetValue("Enabled", "1");				
				//enableMenu.Text = "Disable";
				enabled = true;
			}

			this.Close();			
		}

		private void CreateShortcut()
		{
			StringBuilder programs = new StringBuilder(255);
			SHGetSpecialFolderPath((IntPtr)0, programs, CSIDL_STARTUP, 0);
			StringBuilder shortcutLocation = new StringBuilder(Path.Combine(programs.ToString(), "RadioOff.lnk"));

			if (!System.IO.File.Exists(shortcutLocation.ToString()))
			{
				StringBuilder shortcutTarget = new StringBuilder("\"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\RadioOff.exe" + "\" REBOOT");
				SHCreateShortcut(shortcutLocation, shortcutTarget);
			}
		}

		private void DeleteShortcut()
		{
			StringBuilder programs = new StringBuilder(255);
			SHGetSpecialFolderPath((IntPtr)0, programs, CSIDL_STARTUP, 0);
			StringBuilder shortcutLocation = new StringBuilder(Path.Combine(programs.ToString(), "RadioOff.lnk"));

			if (System.IO.File.Exists(shortcutLocation.ToString()))
				System.IO.File.Delete(shortcutLocation.ToString());
		}

		private void RadioOff()
		{
			OpenNETCF.Tapi.Tapi t = new OpenNETCF.Tapi.Tapi();
			t.Initialize();
			OpenNETCF.Tapi.Line l = t.CreateLine(0, OpenNETCF.Tapi.LINEMEDIAMODE.INTERACTIVEVOICE, OpenNETCF.Tapi.LINECALLPRIVILEGE.MONITOR);
			OpenNETCF.Tapi.CellTSP.lineSetEquipmentState(l.hLine, (int) OpenNETCF.Tapi.LINEEQUIPSTATE.NOTXRX);
			l.Dispose();
			t.Shutdown();
		}

		private void RadioOn()
		{
			OpenNETCF.Tapi.Tapi t = new OpenNETCF.Tapi.Tapi();
			t.Initialize();
			OpenNETCF.Tapi.Line l = t.CreateLine(0, OpenNETCF.Tapi.LINEMEDIAMODE.INTERACTIVEVOICE, OpenNETCF.Tapi.LINECALLPRIVILEGE.MONITOR);
			OpenNETCF.Tapi.CellTSP.lineSetEquipmentState(l.hLine, (int) OpenNETCF.Tapi.LINEEQUIPSTATE.FULL);
			//OpenNETCF.Tapi.CellTSP.lineRegister(l.hLine, (int) OpenNETCF.Tapi.LINEREGMODE.AUTOMATIC, "", 0);
			l.Dispose();
			t.Shutdown();
		}

		private RegistryKey RadioOffRegKey
		{
			get
			{
				RegistryKey lm = Registry.CurrentUser;
				RegistryKey softRk = lm.OpenSubKey("SOFTWARE\\Infinityball\\RadioOff", true);
				if (softRk == null)
					softRk = lm.CreateSubKey("SOFTWARE\\Infinityball\\RadioOff");
				return softRk;
			}
		}

		private void updateCheckMenu_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			Cursor.Show();
			try
			{
				string downloadURL = "http://www.infinityball.com/support/update.xml";
				downloadURL = RadioOffRegKey.GetValue("UpdateURL", downloadURL).ToString();
				if (downloadURL != null && downloadURL.Length > 0)
				{
					string content = "";
					System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(downloadURL);
					System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
					System.IO.StreamReader rdr = new System.IO.StreamReader(res.GetResponseStream());
					content = rdr.ReadToEnd();
					//					content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><updateinfo><download name=\"PhoneLog\" action=\"cab\"><version maj=\"1\" min=\"1\" bld=\"0\"/>http://www.infinityball.com/software/phonelog.cab</download></updateinfo>";
					rdr.Close();
					res.Close();

					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(content);

					XmlElement ui = xmlDoc["updateinfo"];
					if (ui != null)
					{
						foreach (XmlNode appui in ui.ChildNodes)
						{
							if (appui.Attributes["name"] != null)
							{
								if (appui.Attributes["name"].Value.ToLower() == appname.ToLower())
								{
									int maj = 0;
									int min = 0;
									int bld = 0;

									bool update = false;

									XmlNode verui = appui.FirstChild;
									if (verui != null)
									{
										if (verui.Attributes["maj"] != null) maj = Convert.ToInt16(verui.Attributes["maj"].Value);
										if (verui.Attributes["min"] != null) min = Convert.ToInt16(verui.Attributes["min"].Value);
										if (verui.Attributes["bld"] != null) bld = Convert.ToInt16(verui.Attributes["bld"].Value);
									}

									if (maj > appmajor)
										update = true;
									else
										if (min > appminor)
											update = true;
										else
											if (bld > appbuild)
												update = true;

									if (update && appui.InnerText.Length > 0)
									{
										if (MessageBox.Show("An update is available, would you like to download it?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
										{
											System.Diagnostics.Process.Start(appui.InnerText, "");
										}
									}
									else
										MessageBox.Show("No updates are available at this time.");
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Unable to check for updates.");
			}
			Cursor.Current = Cursors.Default;
			Cursor.Hide(); 
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void menuItem4_Click(object sender, EventArgs e)
		{
			RadioOff();
		}

		private void menuItem3_Click(object sender, EventArgs e)
		{
			RadioOn();
		}

		//private void Form1_Activated(object sender, EventArgs e)
		//{
		//    bool enabled = Convert.ToBoolean(RadioOffRegKey.GetValue("Enabled", "0"));
		//    if (enabled)
		//    {
		//        string shortcutTarget = new StringBuilder("\"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\RadioOff.exe" + "\"").ToString();

		//        DateTime offTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OffTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "10:00 PM")));
		//        DateTime onTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OnTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "7:00 AM")));

		//        if (onTime.TimeOfDay.CompareTo(offTime.TimeOfDay) == -1)
		//        {
		//            if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) == 1 && DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) == -1)
		//            {
		//                RadioOn();
		//                OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + offTime.ToShortTimeString()));
		//            }
		//            else if (DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) == 1)
		//            {
		//                RadioOff();
		//                OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + onTime.ToShortTimeString()));
		//            }
		//        }
		//        else
		//        {
		//            if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) == -1 && DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) == 1)
		//            {
		//                RadioOff();
		//                OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + onTime.ToShortTimeString()));
		//            }
		//            else if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) == 1)
		//            {
		//                RadioOn();
		//                OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + offTime.ToShortTimeString()));
		//            }
		//        }
		//    }
		//    else
		//    {
		//        DeleteShortcut();
		//    }
		//}		
	}
}