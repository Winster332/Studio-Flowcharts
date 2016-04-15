using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WPF_Test.FileManager
{
	public static class ManagerProjects
	{
		#region variables
		public const String XSF = ".xsf";
		public const String PATH = @"Projects\";
		public const String PATH_LIST_FILE = @"Data\Settings\Projects\ProjectsInform.xml";
		public const String NAME_CONFIG = "Config.xml";
		public const String NAME_BLOCK = "Blocks.xml";
		public const String NAME_TXT_CODE = "file_code.txt";
		private static Random rand = new Random();
		private static String PrevError;
		private static INFO_ALL_PROJECTS present_project;
		private static INFO_PROJECT present_project_info;
		#endregion
		#region CreateProject
		/// <summary>
		/// Создает директорию и файлы проекта
		/// </summary>
		/// <param name="info">Информация о проекте</param>
		public static Boolean CreateProject(INFO_PROJECT info)
		{
			Boolean result = CheckedName(info.name);
			
			if (result)
			{
				CreateBaseDirectory(info.name);
				String local_path = PATH + info.name + "\\";
				XmlSerializer ser = new XmlSerializer(typeof(INFO_PROJECT));

				using (Stream stream = new FileStream(local_path + @"\Config.xml", FileMode.Create))
				{
					ser.Serialize(stream, info);
				}

				INFO_CONFIG ic = new INFO_CONFIG();
				ic.file_block = local_path + @"Blocks.xml";
				ic.file_setting = local_path + @"Config.xml";
				CreateFileConfigProject(info, ic);

				present_project_info = info;
			}

			return result;
		}
		#endregion
		#region CorrectName
		/// <summary>
		/// Корректирует имя проекта подменяя недопустимые знаки, на знак в параметре SubSim
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="SubSim">Корректирующий символ</param>
		/// <returns></returns>
		public static String CorrectName(String name, String SubSim)
		{
			String[] sm = new String[] { ".", ":", "|", @"\", @"/", "?", ",", "!", "@", "#", "$", "%", "&", "*", "'", "\"" };
			String result = "";
			Boolean con = false;
			for (int i = 0; i < name.Length; i++)
			{
				for (int j = 0; j < sm.Length; j++)
				{
					if (name[i].ToString() == sm[j])
					{
						result += SubSim;
						con = true;
					}
				}
				if (con)
				{
					con = false;
					continue;
				}
				result += name[i];
			}

			return result;
		}
		#endregion
		#region CreateBaseDirectory
		/// <summary>
		/// Создает подрепозитории главного репозитория
		/// </summary>
		/// <param name="name">Имя</param>
		public static void CreateBaseDirectory(String name)
		{
			Directory.CreateDirectory(PATH + name);
		}
		#endregion
		#region Exist
		/// <summary>
		/// Если объект существует, возвращает true
		/// </summary>
		/// <param name="name">Имя проекта</param>
		/// <returns></returns>
		public static Boolean Exist(String name)
		{
			String[] dirs = Directory.GetDirectories(PATH);

			for (int i = 0; i < dirs.Length; i++)
				if (PATH + name == dirs[i])
					return false;

			return true;
		}
		#endregion
		#region GenericNameProject
		/// <summary>
		/// Генерация имени
		/// </summary>
		public static String GenericNameProject()
		{
			String name = "App";
			int index = 0;
			Boolean live_while = true;

			do
			{
				name = "App" + index;
				index++;
				live_while = Exist(name);

			} while (!live_while);

			return name;
		}
		#endregion
		#region RemoveProject
		public static void RemoveProject(String name)
		{

		}
		#endregion
		#region SaveProject
		public static void SaveProject(LMD_Compiling.CompilingManager cm, List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks, LMD_GUI.LMD_BottomPanel info,
			LMD_GUI.LMD_FastInformationPanel fast_info)
		{
			INFO_ALL_PROJECTS info_project = GetPresentProject();

			if (info_project != null)
			{
				List<INFO_BLOCK> info_list = cm.GetInfoBlocks(blocks, info, fast_info);

				if (info_list.Count > 0)
				{
					String path = present_project.Path + present_project.Name + @"\Blocks.xml";

					XmlSerializer ser = new XmlSerializer(typeof(List<INFO_BLOCK>));

					using (Stream stream = new FileStream(path, FileMode.Create))
					{
						ser.Serialize(stream, info_list);
					}
				}
			}
		}
		#endregion
		#region LoadProject
		public static Boolean LoadProject(INFO_ALL_PROJECTS info)
		{
			Boolean result = true;
			String local_path = info.Path + info.Name;

			if (Directory.Exists(local_path))
			{
				result = true;
				present_project = info;

				INFO_CONFIG conf = LoadXSF(local_path + "\\" + info.Name + XSF);

			//	LoadBlocks(local_path);
			}
			else
			{
				PrevError = "Не удалось найти файл. Скорее всего он был перемещен или удален.";
				result = false;
			}

			return result;
		}
		#endregion
		#region LoadXSF
		public static INFO_CONFIG LoadXSF(String path)
		{
			INFO_CONFIG config = null;
			XmlSerializer ser= new XmlSerializer(typeof(INFO_CONFIG));

			using (Stream stream = new FileStream(path, FileMode.Open))
			{
				config = (INFO_CONFIG)ser.Deserialize(stream);
			}

			return config;
		}
		#endregion
		#region GetError
		/// <summary>
		/// Возвращает информацию о последней ошибке, если она была
		/// </summary>
		/// <returns></returns>
		public static String GetError()
		{
			String er = PrevError;
			PrevError = "";

			return er;
		}
		#endregion
		#region CheckedName
		public static Boolean CheckedName(String name)
		{
			Boolean result = Exist(name);

			if (!result)
			{
				PrevError = "Проект с таким именем [" + name + "] уже существует. Переименуйте проект";
				return false;
			}
			else if (result)
			{
				String[] sm = new String[] { ".", ":", "|", @"\", @"/", "?", ",", "!", "@", "#", "$", "%", "&", "*", "'", "\"" };

				for (int i = 0; i < name.Length; i++)
				{
					for (int j = 0; j < sm.Length; j++)
						if (name[i].ToString() == sm[j])
						{
							PrevError = "В названии проекта содержится недопустимый символ [" + sm[j] + "], замените его на другой";
							return false;
						}
				}
			}

			return true;
		}
		#endregion
		#region GetPresentProject
		public static INFO_ALL_PROJECTS GetPresentProject()
		{
			return present_project;
		}
		#endregion
		#region GetPresentProjectInfo
		public static INFO_PROJECT GetPresentProjectInfo()
		{
			return present_project_info;
		}
		#endregion
		#region CreateFileConfigProject
		public static void CreateFileConfigProject(INFO_PROJECT info, INFO_CONFIG config)
		{
			String local_path = PATH + info.name + "\\";
			XmlSerializer ser = new XmlSerializer(typeof(INFO_CONFIG));

			using (Stream stream = new FileStream(local_path + info.name + ".xsf", FileMode.Create))
			{
				ser.Serialize(stream, config);
			}
		}
		#endregion
		#region LoadBlocks
		public static void LoadBlocks(String path)
		{
			String path_block = path;
		}
		#endregion
		#region CreateTXTCode
		public static void CreateTXTCode(String code)
		{
			String path_txt = present_project_info.path + present_project_info.name + "\\" + NAME_TXT_CODE;
			using (Stream stream = new FileStream(path_txt, FileMode.Create))
			{
				StreamWriter writer = new StreamWriter(stream);
				writer.Write(code);
				writer.Close();
			}
		}
		#endregion
		#region ApplySettings
		public static void ApplySettings(INFO_PROJECT info)
		{
			String local_path = PATH + info.name + "\\";
			XmlSerializer ser = new XmlSerializer(typeof(INFO_PROJECT));

			using (Stream stream = new FileStream(local_path + @"\Config.xml", FileMode.Create))
			{
				ser.Serialize(stream, info);
			}

			present_project_info = info;
		}
		#endregion
		#region Clear
		public static void Clear()
		{
			Directory.Delete(PATH, true);
			Directory.CreateDirectory("Projects");
			File.Delete(PATH_LIST_FILE);
		}
		#endregion

		#region Serializable class
		#region INFO_ALL_PROJECTS
		[Serializable]
		public class INFO_ALL_PROJECTS
		{
			public String Name;
			public String Path;
		}
		#endregion
		#region INFO_BLOCK
		[Serializable]
		public class INFO_BLOCK
		{
			public int ID;
			public int NextID;
			public LMD_GUI.TYPE_BLOCK Type;
			public float x;
			public float y;
			public String data_write;
			public String data_read;
		}
		#endregion
		#region INFO_CONFIG
		[Serializable]
		public class INFO_CONFIG
		{
			public String file_block;
			public String file_setting;
		}
		#endregion
		#endregion
	}
}
