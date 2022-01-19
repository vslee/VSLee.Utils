using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSLee.Utils
{
	public static class SystemExtensions
	{
		/// <summary>
		/// Creates a new directory in the same parent dir that the main executable is in (not the referenced library DLL)
		/// </summary>
		/// <param name="directoryName">relative path of new directory</param>
		/// <returns></returns>
		public static DirectoryInfo CreateDirectoryIfMissing(this string directoryName)
		{
			var currentDir = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
			var newDir = Path.Combine(currentDir, directoryName);
			if (!Directory.Exists(newDir))
				return Directory.CreateDirectory(newDir);
			else return new DirectoryInfo(newDir);
		}

		public static bool IsLinux
		{
			get
			{
				var p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		public static bool Is64BitProcess
		{
			get { return IntPtr.Size == 8; }
		}
	}
}
