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
	static class Program
	{
		[DllImport("coredll.dll")]
		static extern int SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder lpszPath, int nFolder, int fCreate);
		[DllImport("coredll.dll")]
		static extern int SHCreateShortcut(StringBuilder szShortcut, StringBuilder szTarget);

		const int CSIDL_PROGRAMS = 2;  // \Windows\Start Menu\Programs
		const int CSIDL_STARTUP = 7; // \Windows\Startup

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				//if (args[0].ToUpper() == "REBOOT" || args[0].ToUpper() == "APPRUNATTIME")
				{
					//MessageBox.Show("Launch from notify");
					string shortcutTarget = new StringBuilder("\"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\RadioOff.exe" + "\"").ToString();
					bool enabled = Convert.ToBoolean(Convert.ToInt16(RadioOffRegKey.GetValue("Enabled", "0")));
					if (enabled)
					{
						OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.None);

						DateTime offTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OffTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "10:00 PM")));
						DateTime onTime = Convert.ToDateTime(RadioOffRegKey.GetValue("OnTime", Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + "7:00 AM")));

						OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.RestoreEnd);

						if (onTime.TimeOfDay.CompareTo(offTime.TimeOfDay) == -1)
						{
							if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) >= 0 && DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) == -1)
							{
								RadioOn();
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + offTime.ToShortTimeString()));
							}
							else if (DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) >= 0)
							{
								RadioOff();
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + onTime.ToShortTimeString()));
							}
							else
							{
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + offTime.ToShortTimeString()));
							}

						}
						else
						{
							if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) == -1 && DateTime.Now.TimeOfDay.CompareTo(offTime.TimeOfDay) >= 0)
							{
								RadioOff();
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + onTime.ToShortTimeString()));
							}
							else if (DateTime.Now.TimeOfDay.CompareTo(onTime.TimeOfDay) >= 0)
							{
								RadioOn();
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " " + offTime.ToShortTimeString()));
							}
							else
							{
								OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + onTime.ToShortTimeString()));
							}
						}
					}
					else
					{
						OpenNETCF.Win32.Notify.Notify.RunAppAtTime(shortcutTarget, DateTime.MinValue);
						OpenNETCF.Win32.Notify.Notify.RunAppAtEvent(shortcutTarget, NotificationEvent.None);
						//DeleteShortcut();
					}
				}
				return;
			}else
				Application.Run(new Form1());
		}

		//private static void DeleteShortcut()
		//{
		//    StringBuilder programs = new StringBuilder(255);
		//    SHGetSpecialFolderPath((IntPtr)0, programs, CSIDL_STARTUP, 0);
		//    StringBuilder shortcutLocation = new StringBuilder(Path.Combine(programs.ToString(), "RadioOff.lnk"));

		//    if (System.IO.File.Exists(shortcutLocation.ToString()))
		//        System.IO.File.Delete(shortcutLocation.ToString());
		//}

		private static RegistryKey RadioOffRegKey
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

		private static void RadioOff()
		{
			OpenNETCF.Tapi.Tapi t = new OpenNETCF.Tapi.Tapi();
			t.Initialize();
			OpenNETCF.Tapi.Line l = t.CreateLine(0, OpenNETCF.Tapi.LINEMEDIAMODE.INTERACTIVEVOICE, OpenNETCF.Tapi.LINECALLPRIVILEGE.MONITOR);			
			OpenNETCF.Tapi.CellTSP.lineSetEquipmentState(l.hLine, (int)OpenNETCF.Tapi.LINEEQUIPSTATE.NOTXRX);
			l.Dispose();
			t.Shutdown();
		}

		private static void RadioOn()
		{
			OpenNETCF.Tapi.Tapi t = new OpenNETCF.Tapi.Tapi();
			t.Initialize();
			OpenNETCF.Tapi.Line l = t.CreateLine(0, OpenNETCF.Tapi.LINEMEDIAMODE.INTERACTIVEVOICE, OpenNETCF.Tapi.LINECALLPRIVILEGE.MONITOR);
			OpenNETCF.Tapi.CellTSP.lineSetEquipmentState(l.hLine, (int)OpenNETCF.Tapi.LINEEQUIPSTATE.FULL);
			//OpenNETCF.Tapi.CellTSP.lineRegister(l.hLine, (int)OpenNETCF.Tapi.LINEREGMODE.AUTOMATIC, "", 0);
			l.Dispose();
			t.Shutdown();
		}
	}
}