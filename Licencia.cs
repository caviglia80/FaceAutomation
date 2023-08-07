using FluentFTP;
using System.Text;


namespace FaceAutomation2
{
	public partial class Licencia : Form
	{
		public Licencia()
		{
			InitializeComponent();
		}

		private void Licencia_Load(object sender, EventArgs e)
		{
			//compruebo fecha en el registro
			ComprobarFecha();
		}

		private void BtnRegistrar_Click(object sender, EventArgs e)
		{
			if (TxtKey.Text.Trim().Length == 0) return;

			List<string> lista = DescargarLicencias();
			foreach (string Licencia in lista)
				if (Licencia.Equals(TxtKey.Text.Trim()))
				{
					SetNewKey(Licencia);    //seteo nueva fecha en el registro
					lista.Remove(Licencia); //la elimino
					SubirLicencias(lista);  //devuelvo lista restante al ftp
					Globals.AddShortcut();
					this.DialogResult = DialogResult.Yes;
					break;
				}
			this.BackColor = Color.Black;
			this.BtnRegistrar.ForeColor = Color.Red;
		}

		private void ComprobarFecha()
		{
			try
			{
				DateTime Vencimiento = Convert.ToDateTime(Globals.GetRegKeyDate("Fecha"));
				if (Vencimiento >= Globals.GetServerTime())
					this.DialogResult = DialogResult.Yes;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static void SetNewKey(string KEY)
		{
			try
			{
				int days = Globals.SoloNumeros(KEY);
				Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Globals.HKEY_SOFTWAREE);
				Microsoft.Win32.Registry.SetValue(Globals.HKEY_CURRENT_USERR, "Fecha", Globals.GetServerTime().AddDays((double)days).ToString());
				Microsoft.Win32.Registry.SetValue(Globals.HKEY_CURRENT_USERR, "UltimaCompraDias", days.ToString());
				Microsoft.Win32.Registry.SetValue(Globals.HKEY_CURRENT_USERR, "AdminName", KEY.Replace(days.ToString(), ""));
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void SubirLicencias(List<string> Lista)
		{
			try
			{
				string textContent = Newtonsoft.Json.JsonConvert.SerializeObject(Lista);
				using FtpClient client = new(Globals.FTP, new System.Net.NetworkCredential(Globals.ftp_User, Globals.ftp_Pass));
				client.Connect();
				byte[] fileContents = Encoding.ASCII.GetBytes(textContent); ;
				FtpStatus asd = client.UploadBytes(fileContents, Globals.Licencias, FtpRemoteExists.Overwrite);
				//return asd.ToString().Equals("Success");
				client.Disconnect();
				client.Dispose();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private List<string> DescargarLicencias()
		{
			try
			{
				// "ftp://c1491694.ferozo.com/FA/FAdocs/Licencias.txt"
				using FtpClient client = new(Globals.FTP, new System.Net.NetworkCredential(Globals.ftp_User, Globals.ftp_Pass));
				client.Connect();
				if (!client.DownloadBytes(out byte[] bytes, Globals.Licencias))
					throw new Exception("Cannot read file");
				string result = Encoding.UTF8.GetString(bytes);
				List<string> lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(result);
				client.Disconnect();
				client.Dispose();
				return lista;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}
	}
}

