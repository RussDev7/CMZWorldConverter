using System;
using System.Collections.Generic;
using System.IO;
using DNA.IO.Storage;

namespace ConsoleApp1
{
	public class WorldInfo
	{
		public int Version
		{
			get
			{
				return 5;
			}
		}

		public string SavePath
		{
			get
			{
				return this._savePath;
			}
			set
			{
				this._savePath = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string OwnerGamerTag
		{
			get
			{
				return this._ownerGamerTag;
			}
		}

		public string CreatorGamerTag
		{
			get
			{
				return this._creatorGamerTag;
			}
		}

		public DateTime CreatedDate
		{
			get
			{
				return this._createdDate;
			}
		}

		public DateTime LastPlayedDate
		{
			get
			{
				return this._lastPlayedDate;
			}
			set
			{
				this._lastPlayedDate = value;
			}
		}

		public int Seed
		{
			get
			{
				return this._seed;
			}
		}

		public Guid WorldID
		{
			get
			{
				return this._worldID;
			}
		}

		private WorldInfo()
		{
			this.ServerMessage = "D.RUSS#2430";
		}

		public static WorldInfo[] LoadWorldInfo(SaveDevice device)
		{
			WorldInfo[] result;
			try
			{
				WorldInfo.CorruptWorlds.Clear();
				if (!device.DirectoryExists(WorldInfo.BasePath))
				{
					result = new WorldInfo[0];
				}
				else
				{
					List<WorldInfo> list = new List<WorldInfo>();
					foreach (string text in device.GetDirectories(WorldInfo.BasePath))
					{
						WorldInfo worldInfo = null;
						try
						{
							worldInfo = WorldInfo.LoadFromStroage(text, device);
						}
						catch
						{
							worldInfo = null;
							WorldInfo.CorruptWorlds.Add(text);
						}
						if (worldInfo != null)
						{
							list.Add(worldInfo);
						}
					}
					result = list.ToArray();
				}
			}
			catch
			{
				result = new WorldInfo[0];
			}
			return result;
		}

		public void SaveToStorage(SaveDevice saveDevice)
		{
			try
			{
				if (!saveDevice.DirectoryExists(this.SavePath))
				{
					saveDevice.CreateDirectory(this.SavePath);
				}
				string fileName = Path.Combine(this.SavePath, WorldInfo.FileName);
				saveDevice.Save(fileName, false, false, delegate(Stream stream)
				{
					Console.WriteLine("SaveDevice");
					BinaryWriter binaryWriter = new BinaryWriter(stream);
					this.Save(binaryWriter);
					binaryWriter.Flush();
				});
			}
			catch
			{
			}
		}

		private static WorldInfo LoadFromStroage(string folder, SaveDevice saveDevice)
		{
			WorldInfo info = new WorldInfo();
			saveDevice.Load(Path.Combine(folder, WorldInfo.FileName), delegate(Stream stream)
			{
				info.Load(stream);
				info._savePath = folder;
			});
			return info;
		}

		public void Save(BinaryWriter writer)
		{
			byte[] array = new byte[4];
			array[0] = 2;
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Console.WriteLine("Saving new worldinfo " + i);
				writer.Write(array2[i]);
			}
			for (int j = 4; j < this.Bytes.Length; j++)
			{
				Console.WriteLine("Saving new worldinfo " + j);
				writer.Write(this.Bytes[j]);
			}
			writer.Write(this.ServerMessage);
			writer.Write(this.ServerPassword);
		}

		public byte[] ReadAllBytes(Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		private void Load(Stream stream)
		{
			this.Bytes = this.ReadAllBytes(stream);
		}

		public byte[] Bytes;

		public static List<string> CorruptWorlds = new List<string>();

		private static readonly string BasePath = "OutputWorlds";

		private static readonly string FileName = "world.info";

		private string _savePath;

		public WorldInfo.WorldTypeIDs _terrainVersion = WorldInfo.WorldTypeIDs.CastleMinerZ;

		private string _name = "World";

		private string _ownerGamerTag;

		private string _creatorGamerTag;

		private DateTime _createdDate;

		private DateTime _lastPlayedDate;

		private int _seed;

		private Guid _worldID;

		public bool InfiniteResourceMode;

		public int HellBossesSpawned;

		public int MaxHellBossSpawns;

		public string ServerMessage = "D.RUSS#2430";

		public string ServerPassword = "";

		private enum WorldInfoVersion
		{
			Initial = 1,
			Doors,
			Spawners,
			HellBosses,
			CurrentVersion
		}

		public enum WorldTypeIDs
		{
			CastleMinerZ = 1
		}
	}
}
