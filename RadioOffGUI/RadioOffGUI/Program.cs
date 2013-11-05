using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RadioOffGUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main(string[] args)
		{
			if (args.Length > 0)
				MessageBox.Show(args[0]);
			else
				Application.Run(new Form1());
		}
	}
}