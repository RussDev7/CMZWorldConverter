using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DNA.IO.Storage;
using DNA.Security.Cryptography;

namespace ConsoleApp1
{
	internal class Program
	{
        public static string HelloThere = "Hello there fellow Decompiler, This Program Was Made By D.RUSS#2430 (xXCrypticNightXx).";
        private static void Main(string[] args)
		{
            bool flag = false;
			string currentDirectory = Directory.GetCurrentDirectory();
			SaveDevice device = new FileSystemSaveDevice(currentDirectory, null);
			Console.WriteLine("HELLO PLS ENTER GAMERTAG OR STEAMID64 THAT THE MAP(S) CAME FROM");
			string text = Console.ReadLine();
			if (text.Length < 17)
			{
				flag = true;
			}
			Program.DecryptSaves(text, currentDirectory);
			if (flag)
			{
				Program.UpdateSaves(device);
			}
			Console.WriteLine("Done. Press any key to close");
			Console.ReadKey();
		}

		private static void UpdateSaves(SaveDevice device)
		{
			foreach (WorldInfo worldInfo in WorldInfo.LoadWorldInfo(device))
			{
				Console.WriteLine("Saving new worldinfo");
				worldInfo.SaveToStorage(device);
			}
		}

		private static void DecryptSaves(string gamertag, string directory)
		{
			string text = Path.Combine(directory, "Worlds");
			string text2 = Path.Combine(directory, "OutputWorlds");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			byte[] data = new MD5HashProvider().Compute(Encoding.UTF8.GetBytes(gamertag + "CMZ778")).Data;
			SaveDevice saveDevice = new FileSystemSaveDevice(text, data);
			SaveDevice saveDevice2 = new FileSystemSaveDevice(text2, null);
			List<string> list = new List<string>();
			Program.GetFiles(text, list);
			foreach (string text3 in list)
			{
				byte[] dataToSave;
				try
				{
					Console.WriteLine("Loading " + text3.Replace(directory, ""));
					dataToSave = saveDevice.LoadData(text3);
				}
				catch
				{
					Console.WriteLine("Failed to load " + text3.Replace(directory, ""));
					continue;
				}
				string text4 = text3.Replace(text, text2);
				if (!Directory.Exists(Path.GetDirectoryName(text4)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(text4));
				}
				Console.WriteLine("saving " + text4.Replace(directory, ""));
				saveDevice2.Save(text4, dataToSave, false, false);
			}
		}

		private static void GetFiles(string path, List<string> returnedFiles)
		{
			string[] array = Directory.GetDirectories(path);
			for (int i = 0; i < array.Length; i++)
			{
				Program.GetFiles(array[i], returnedFiles);
			}
			foreach (string item in Directory.GetFiles(path))
			{
				returnedFiles.Add(item);
			}
		}
	}
}
