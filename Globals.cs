using OpenQA.Selenium.Chrome;
using System.Net.Mail;
using Telegram.Bot;
using System.Diagnostics;

namespace FaceAutomation2 
{
	public class Globals
	{
		public static bool DEBUG = false;
		public static string IMFolderGeneralAutos = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "\\FA\\Vehiculos\\Autos\\");
		public static string IMFolder_Originales = string.Concat(IMFolderGeneralAutos, "xxx\\");
		public static string IMFolder_Temporales = string.Concat(Path.GetTempPath(), "FA\\xxx\\");

		public static string DRIVERS =  Application.StartupPath + "WebDrivers";   // RutaLocalFa + "\\WebDrivers"; //"C:\\WebDrivers";
		//public static string UAGENT = "--user-agent=Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
		public static string UAGENT = "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36";

		public static Random Rando = new();

		public static string ProgramFiles_FaceAutomation = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\FaceAutomation";
		public static string ProgramFiles_FAUpdate = ProgramFiles_FaceAutomation + "\\FAUpdate";
		public static string AppData_FaceAutomation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FaceAutomation";
		public static string DB_File = AppData_FaceAutomation + "\\DB.db";
		public static string Log_File = AppData_FaceAutomation + "\\log.dat";
		public static string FU_TEMP = Path.GetTempPath() + "FUx4e5dcrf6tg7yh8uj.zip";
		public static string FU_EXE = ProgramFiles_FAUpdate + "\\FAUpdate.exe";
		public static string FA_EXE = ProgramFiles_FaceAutomation + "\\FaceAutomation2.exe";

		public static string HKEY_CURRENT_USERR = "HKEY_CURRENT_USER\\SOFTWARE\\FA";
		public static string HKEY_SOFTWAREE = "SOFTWARE\\FA";
		//public static string RutaLocalFa = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "\\FA");

		public static string ftp_User = "";
		public static string ftp_Pass = "";
		public static string FTP = ""; //"ftp://xxxx.ferozo.com"
		public static string Licencias = "";

		public static TelegramBotClient BOT = new("5201967196:AAHOngL1Ter9HV2hlih8GJWt3JBO7OJhVQQ");
		public static TelegramBotClient BOT_Debug = new("5230179231:AAHtPxt_6fUCL-_5cqdpYrm0-N_cTrdjsIg");
		public static string ID_Chat_Debug = "1821497307";





























		public static string EjecutarComando(string comando)
		{
			try
			{
				// Crea un proceso de CMD
				Process proceso = new Process();
				proceso.StartInfo.FileName = "cmd.exe";
				proceso.StartInfo.UseShellExecute = false;
				proceso.StartInfo.RedirectStandardOutput = true;
				proceso.StartInfo.RedirectStandardError = true;
				proceso.StartInfo.CreateNoWindow = true;

				// Establece el comando y los argumentos
				proceso.StartInfo.Arguments = $"/c {comando}";

				// Inicia el proceso
				proceso.Start();

				// Lee la salida y el error
				string salida = proceso.StandardOutput.ReadToEnd();
				string error = proceso.StandardError.ReadToEnd();

				// Espera a que el proceso termine
				proceso.WaitForExit();

				// Cierra el proceso
				proceso.Close();
				proceso.Dispose();

				// Devuelve la salida y el error como una cadena de texto
				return salida + Environment.NewLine + error;
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "Error";
			}
		}


		public static string GetFullTextException(Exception ex)
		{
			try
			{
				return string.Concat(
					GetRegKeyString("AdminName"),
					Environment.NewLine,
					"Sistema de 64Bits: ", System.Environment.Is64BitOperatingSystem.ToString(),
					Environment.NewLine,
					"Procesador de 64Bits: ", System.Environment.Is64BitProcess.ToString(),
					Environment.NewLine,
					"Versión del SO: ", System.Environment.OSVersion,
					Environment.NewLine,
					"Nombrel del Equipo: ", System.Environment.MachineName,
					Environment.NewLine,
					"Nombre del Usuario: ", System.Security.Principal.WindowsIdentity.GetCurrent().Name,
					Environment.NewLine,
					"Procesadores activos para la app: ", System.Environment.ProcessorCount.ToString(),
					Environment.NewLine,
					"FA Version: ", Actualizador.LocalVer_FA.ToString(),
					Environment.NewLine,
					"FU Version: ", Actualizador.LocalVer_FU.ToString(),
					Environment.NewLine,
					Environment.NewLine,
					ex.Message, " ((", ex.TargetSite, "))",
					Environment.NewLine,
					Environment.NewLine,
					ex.ToString()
					);
			}
			catch (Exception ex1)
			{
				DebugSendMessaje(ex1.StackTrace);
			}
			return "???";
		}

		public static void CrearDirectorios()
		{
			try
			{
				Directory.CreateDirectory(ProgramFiles_FaceAutomation);
				Directory.CreateDirectory(AppData_FaceAutomation);
				Directory.CreateDirectory(IMFolderGeneralAutos);
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
			}
		}

		public static void AddShortcut()
		{
			try
			{
				string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				using StreamWriter writer = new(deskDir + "\\FaceAutomation.url");
				string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
				writer.WriteLine("[InternetShortcut]");
				writer.WriteLine("URL=file:///" + FA_EXE);
				writer.WriteLine("IconIndex=0");
				string icon = app.Replace('\\', '/');
				writer.WriteLine("IconFile=" + icon);
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
			}
		}

		public static int SoloNumeros(string strCadena)
		{
			try
			{
				return Convert.ToInt32(System.Text.RegularExpressions.Regex.Match(strCadena, "(\\d+)").ToString());
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return 0;
			}
		}

		public static DateTime GetServerTime()
		{
			try
			{
				System.Net.Sockets.TcpClient client;
				DateTime localDateTime = new();

				try
				{
					client = new System.Net.Sockets.TcpClient("time.nist.gov", 13);
					using StreamReader streamReader = new(client.GetStream());
					var utcDateTimeString = streamReader.ReadToEnd().Substring(7, 17);
					localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
				}
				catch
				{
					try
					{
						Application.DoEvents();
						client = new System.Net.Sockets.TcpClient("wwv.nist.gov", 13);
						using StreamReader streamReader = new(client.GetStream());
						var utcDateTimeString = streamReader.ReadToEnd().Substring(7, 17);
						localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
					}
					catch (Exception ex)
					{
						DebugSendMessaje(GetFullTextException(ex));
						MessageBox.Show("Error de conexion, reintente en unos segundos o compruebe su conexion a internet.");
						System.Environment.Exit(0);
					}
				}
				return localDateTime;
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return new();
			}
		}

		public static string GetRegKeyDate(string NAME)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(HKEY_CURRENT_USERR, NAME, "");
				if (value!=null)
					return value.ToString();
				else
					return DateTime.MinValue.ToString();
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "?";
			}
		}

		public static string GetRegKeyString(string NAME)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(HKEY_CURRENT_USERR, NAME, "");
				if (value != null)
					return value.ToString();
				else
					return "???";
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "???";
			}
		}

		public static async void DebugSendMessaje(string msj)
		{
			try
			{
				await BOT_Debug.SendTextMessageAsync(ID_Chat_Debug, msj);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public static async void SendMessaje(string msj)
		{
			try
			{
				foreach (TeleID I in BDQuery.GetTelegramListNotBlocked())
					await BOT.SendTextMessageAsync(I.ID_CHAT, msj);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public static async void SendMessaje(string ID, string msj)
		{
			try
			{
				await BOT.SendTextMessageAsync(ID, msj);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public static async Task<bool> New_SendMessaje(string NewID, string msj)
		{
			try
			{
				await BOT.SendTextMessageAsync(NewID, msj);
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show("Telegram: ID de Chat Invalido.");
				return false;
			}
		}

		public static bool EmailValido(string strEmail)
		{
			try
			{
				MailAddress m = new(strEmail);
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show("Email invalido.");
				return false;
				throw;
			}
		}

		public static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			try
			{
				Directory.CreateDirectory(targetPath);
				//Copy all the files & Replaces any files with the same name
				foreach (string F in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
					System.IO.File.Copy(F, F.Replace(sourcePath, targetPath), true);
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
			}
		}

		public static string ObtenerArchivo()
		{
			try
			{
				using (OpenFileDialog openFileDialog = new())
				{
					openFileDialog.InitialDirectory = "c:\\";
					openFileDialog.Filter = "TXT Files (*.txt)|*.txt";
					openFileDialog.FilterIndex = 2;
					openFileDialog.RestoreDirectory = true;

					if (openFileDialog.ShowDialog() == DialogResult.OK)
						return openFileDialog.FileName;
					else
						return "";
				}

			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return null;
			}
		}

		public static void EliminarChromeDriverBasura()
		{
			try
			{
				foreach (System.Diagnostics.Process P in System.Diagnostics.Process.GetProcessesByName("chromedriver"))
					P.Kill();
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
			}
		}

		public static float NextFloat(float min, float max)
		{
			try
			{
				double val = (Rando.NextDouble() * (max - min) + min);
				return (float)val;
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return (float)0;
			}
		}

		public static bool hayImagenes(string ID)
		{
			try
			{
				string CarpetaImagenPub = IMFolder_Originales.Replace("xxx", ID);
				if (!Directory.Exists(CarpetaImagenPub)) return false;
				if (Directory.GetFiles(CarpetaImagenPub).Count() > 0) return true; else return false;
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return false;
			}
		}

		public static void Sumar1Publicacion(string User)
		{
			try
			{
				DateTime UltimaPublicacion = DateTime.Parse(BDQuery.GetConfiguration("DateUltimaPublicacion"));
				DateTime Ahora = DateTime.Now;

				//TOTAL
				int PublicacionesTotalHistorico = Convert.ToInt32(BDQuery.GetConfiguration("PublicacionesTotalHistorico")) + 1;
				BDQuery.UpdateConfiguration("PublicacionesTotalHistorico", PublicacionesTotalHistorico.ToString());

				//SOLO MENSUAL
				int PublicacionesMensuales = Convert.ToInt32(BDQuery.GetConfiguration("PublicacionesMensuales")) + 1;
				if (Ahora.Month != UltimaPublicacion.Month)
					BDQuery.UpdateConfiguration("PublicacionesMensuales", "1");
				else
					BDQuery.UpdateConfiguration("PublicacionesMensuales", PublicacionesMensuales.ToString());

				//SOLO DIARIO
				int PublicacionesDiarias = Convert.ToInt32(BDQuery.GetConfiguration("PublicacionesDiarias")) + 1;
				if (Ahora.Day != UltimaPublicacion.Day)
					BDQuery.UpdateConfiguration("PublicacionesDiarias", "1");
				else
					BDQuery.UpdateConfiguration("PublicacionesDiarias", PublicacionesDiarias.ToString());

				BDQuery.UpdateConfiguration("DateUltimaPublicacion", Ahora.ToString());

				//ACTUALIZO LA FECHA/HORA DE LA ULTIMA PUBLICACION DEL USUARIO
				BDQuery.UpdateUser(User, "UltimaPublicacionDate", Ahora.ToString());
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
			}
		}

		public static string CadenaAleatoria(int Longitud = 7)
		{
			try
			{
				Guid miGuid = Guid.NewGuid();
				string token = Convert.ToBase64String(miGuid.ToByteArray());
				token = token.Replace("=", "").Replace("+", "");
				return token.Substring(0, Longitud);
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "";
			}
		}

		public static string MonthName(int month)
		{
			try
			{
				System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.CultureInfo("es-ES", false).DateTimeFormat;
				return dtinfo.GetMonthName(month);
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "";
			}
		}

		public static string GetAllWindowsOpened(ChromeDriver driver)
		{
			try
			{
				string ventanas = "";

				// Obtener los identificadores de las ventanas abiertas
				var handles = driver.WindowHandles;

				// Iterar a través de los identificadores y cambiar a cada ventana
				foreach (var handle in handles)
				{
					ventanas += driver.Url + "  |  ";
					driver.SwitchTo().Window(handle);
				}
				return ventanas;
			}
			catch (Exception ex)
			{
				DebugSendMessaje(GetFullTextException(ex));
				return "";
			}
		}




	}
}





//	Public Function EmailValido(strEmail As String) As Boolean
//	' Retorna verdadero si strEmail es un formato de E-mail valido.
//	Return System.Text.RegularExpressions.Regex.IsMatch(strEmail, "^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" & "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
//End Function


//public static DateTime Now()
//{
//	try
//	{
//		return DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", CultureInfo.CurrentCulture);
//	}
//	catch (Exception ex)
//	{
//		MessageBox.Show(ex.StackTrace);
//		throw;
//	}
//}

//public static void clickOn(OpenQA.Selenium.WebDriver driver, OpenQA.Selenium.WebElement locator, int timeout)
//{
//	new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
//		.IgnoreExceptionTypes(typeof(OpenQA.Selenium.NoSuchElementException));

//}