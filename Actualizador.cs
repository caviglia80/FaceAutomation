
namespace FaceAutomation2
{
	public class Actualizador
	{
		public static int LocalVer_FA = 0;
		public static int RemoteVer_FA = 0;

		public static int LocalVer_FU = 0;
		public static int RemoteVer_FU = 0;






		

		public static void ComprobarActualizaciones()
		{
			try
			{
				//TraerVersiones
				LocalVer_FA = GetLocalVer(Globals.ProgramFiles_FaceAutomation);
				RemoteVer_FA = GetRemoteVer("/FA/FA");
				LocalVer_FU = GetLocalVer(Globals.ProgramFiles_FAUpdate);
				RemoteVer_FU = GetRemoteVer("/FA/FU");

				//compruebo si falta actualizar algo
				if (RemoteVer_FU > LocalVer_FU)
				{
					LimpiamosFU();
					DescargamosFU();
					DescomprimimosFU();
				}

				//INICIO FAUpdate.exe
				if (File.Exists(Globals.FU_EXE))
				{
					if (RemoteVer_FA > LocalVer_FA)
					{
						System.Diagnostics.Process.Start(Globals.FU_EXE).Dispose();
						System.Environment.Exit(0);
					}
				}
				else
				{
					MessageBox.Show("Falta el archivo de actualizacion.");
					Globals.DebugSendMessaje("Falta el archivo de actualizacion.");
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static int GetRemoteVer(string Dir)
		{
			try
			{
				using FluentFTP.FtpClient client = new(Globals.FTP, new System.Net.NetworkCredential(Globals.ftp_User, Globals.ftp_Pass));
				client.Connect();
				foreach (var item in client.GetNameListing(Dir))
					if (item.Contains("VER"))
						return Globals.SoloNumeros(item);
				client.Disconnect();
				client.Dispose();
				return 0;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return 0;
			}
		}

		private static int GetLocalVer(string Dir)
		{
			try
			{
				foreach (FileInfo item in new DirectoryInfo(Dir).GetFiles())
					if (item.Name.Contains("VER"))
						return Globals.SoloNumeros(item.Name);
				return 0;
			}
			catch
			{ return 0; }
		}

		private static void LimpiamosFU()
		{
			try
			{
				foreach (System.Diagnostics.Process P in System.Diagnostics.Process.GetProcessesByName("FAUpdate"))
					P.Kill();

				if (Directory.Exists(Globals.ProgramFiles_FAUpdate))
				{
					var List1 = Directory.GetFiles(Globals.ProgramFiles_FAUpdate, "*.*");
					if (List1.Length > 0)
						foreach (var item in List1)
							File.Delete(item);

					var List2 = Directory.GetDirectories(Globals.ProgramFiles_FAUpdate);
					if (List2.Length > 0)
						foreach (var item in List2)
							Directory.Delete(item);
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static void DescargamosFU()
		{
			try
			{
				using FluentFTP.FtpClient client = new(Globals.FTP, new System.Net.NetworkCredential(Globals.ftp_User, Globals.ftp_Pass));
				client.Connect();
				string RemoteFileName = "";
				foreach (var item in client.GetNameListing("/FA/FU"))
					if (item.Contains("VER"))
						RemoteFileName = item;
				client.DownloadFile(Globals.FU_TEMP, RemoteFileName);
				client.Disconnect();
				client.Dispose();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static void DescomprimimosFU()
		{
			try
			{
				System.IO.Compression.ZipFile.ExtractToDirectory(Globals.FU_TEMP, Globals.ProgramFiles_FAUpdate, System.Text.Encoding.Default, true);
				Application.DoEvents();
				File.Delete(Globals.FU_TEMP);
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		//foreach (System.Diagnostics.Process P in System.Diagnostics.Process.GetProcessesByName("FAUpdate"))
		//	P.Kill();
		//foreach (var item in Directory.GetFiles("C:\\Program Files\\FA\\FAUpdate", "*.*"))
		//	File.Delete(item);
		//foreach (var item in Directory.GetDirectories("C:\\Program Files\\FA\\FAUpdate"))
		//	Directory.Delete(item);
	}
}
