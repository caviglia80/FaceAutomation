using OpenQA.Selenium.Chrome;
using Telegram.Bot;




namespace FaceAutomation2
{
	public partial class Configuraciones : Form
	{
		private ChromeDriver Driver;
		private OpenQA.Selenium.Interactions.Actions Action;
		private OpenQA.Selenium.Support.UI.WebDriverWait WAIT;
		private ChromeDriverService DriverService;

		public Configuraciones()
		{
			InitializeComponent();
		}

		private void BTNvolver_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Configuraciones_Load(object sender, EventArgs e)
		{
			try
			{
				LoadConf();
				AgregarTooltips();
				LVersion.Text = "Version: " + Actualizador.LocalVer_FA.ToString();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void AgregarTooltips()
		{
			try
			{
				ToolTip ToolTip1 = new()
				{
					AutoPopDelay = 5000,
					InitialDelay = 0,
					ReshowDelay = 0,
					ShowAlways = true
				};
				ToolTip1.SetToolTip(this.CbEsperaUsuario, "El usuario que publico debera esperar a que transcurran estos minutos antes de volver a publicar.");
				ToolTip1.SetToolTip(this.LVtelegram, "Opcionalmente, puede optar por estar informado de metricas, avances y problemas mediante la aplicacion de telegram.");
				ToolTip1.SetToolTip(this.LVfb, "Es necesario agregar al menos un usuario para poder usar la aplicacion, es recomendable tener multiples usuarios.");
				ToolTip1.SetToolTip(this.BTNquitarTL, "Se recomienda no quitar la cuenta de telegram ya que haciendo doble clic sobre la misma puede deshabilitar las notificaciones sin tener que borrar.");
				ToolTip1.SetToolTip(this.BTNquitarFB, "Se recomienda no quitar la cuenta ya que haciendo doble clic sobre la misma puede deshabilitar la misma, esto hara que la aplicacion no use la cuenta para publicar.");
				ToolTip1.SetToolTip(this.CLBImagenes, "Edicion de imagenes aleatorias, trata de evitar que las mismas se reconozcan como duplicadas al subierse y no se llegue a publicar, mas opciones seleccionadas puede beneficiar mas el proceso pero podria llevar a perdidas de calidad de imagen.");
				ToolTip1.SetToolTip(this.BTNUbicaciones, "Se deben seleccionar las ubicaciones que se tomaran en cuenta a la hora de publicar. Por defecto viene algunas ubicaciones de prueba que pueden ser reemplazadas.");
				ToolTip1.SetToolTip(this.BTNgestionPublicaciones, "Se deben configurar las publicaciones que se consumiran en el proceso, es mandatorio agregar imagenes.");
				ToolTip1.SetToolTip(this.BTNrestaurar, "Restaurar configuracion de fabrica, esto no borrara las publicaciones ni las ubicaciones configuradas.");
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNgestionPublicaciones_Click(object sender, EventArgs e)
		{
			try
			{
				this.Visible = false;
				new Publicaciones().ShowDialog();
				this.Visible = true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void TraerUsuariosFacebook()
		{
			try
			{
				LVfb.Clear();
				LVfb.Columns.Add("Usuario", (LVfb.Width / 2) - 2);
				LVfb.Columns.Add("Habilitado", (LVfb.Width / 2) - 2, HorizontalAlignment.Center);
				foreach (Secret S in BDQuery.GetUserList())
					LVfb.Items.Add(new ListViewItem(new string[] { S.Usuario, S.Habilitado }));
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNagregarFB_Click(object sender, EventArgs e)
		{
			try
			{
				if (TXTuser.Text.Length == 0 | TXTpssw.Text.Length < 6) { return; }
				if (!Globals.EmailValido(TXTuser.Text)) { return; }
				TLoader.Enabled = true;

				Task.Factory.StartNew(() => { Proceso(); });
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void Proceso()
		{
			try
			{
				if (!ComprobarUsuario(TXTuser.Text, TXTpssw.Text))
				{
					MessageBox.Show("No se logro iniciar sesion con el usuario ingresado, reviselo y/o verifique que el inicio de sesion de su cuenta no cuente con autenticacion de doble factor (2F).");
				}
				else
				{
					if (BDQuery.AddNewUser(TXTuser.Text, TXTpssw.Text)) { this.Invoke(() => { TraerUsuariosFacebook(); }); }
				}

				this.Invoke(() =>
				{
					Driver.Quit();
					TXTuser.Text = "";
					TXTpssw.Text = "";
					TLoader.Enabled = false;
					LFestado.Text = "";
					LFestado.ForeColor = Color.Black;
				});
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private bool ComprobarUsuario(string User, string Pssw)
		{
			try
			{
				InicializarDriver();
				Driver.Navigate().GoToUrl("https://www.facebook.com/");
				OpenQA.Selenium.IWebElement UserEle = Find(OpenQA.Selenium.By.CssSelector("#email"));
				OpenQA.Selenium.IWebElement PsswEle = Find(OpenQA.Selenium.By.CssSelector("#pass"));
				if (UserEle != null) UserEle.SendKeys(User); else return false;
				if (PsswEle != null) PsswEle.SendKeys(Pssw); else return false;
				Action.SendKeys(OpenQA.Selenium.Keys.Enter).Pause(TimeSpan.FromMilliseconds(500)).Perform();

				Driver.Navigate().GoToUrl("https://www.facebook.com/marketplace/create/vehicle");

				if (EqURL("https://www.facebook.com/marketplace/create/vehicle"))
					return true;
				else return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		private bool InicializarDriver()
		{
			try
			{
				if (Driver != null)
				{
					Driver.Dispose();
					DriverService.Dispose();

				}
				Globals.EliminarChromeDriverBasura();

				DriverService = ChromeDriverService.CreateDefaultService(Globals.DRIVERS);
				DriverService.HideCommandPromptWindow = true;

				//'"--headless" 
				ChromeOptions Options = new();
				Options.AddArgument(Globals.UAGENT);
				Options.AddArguments("--headless", "--disable-notifications", "--disable-extensions", "--disable-gpu", "--ignore-certificate-errors", "--no-sandbox", "--disable-dev-shm-usage");

				Driver = new ChromeDriver(DriverService, Options);   //Driver = New ChromeDriver("C:\\WebDrivers", Options)
																	 //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
				Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
				Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(25);
				Driver.Manage().Window.Maximize();

				WAIT = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, timeout: TimeSpan.FromSeconds(5))
				{ PollingInterval = TimeSpan.FromSeconds(5) };

				Action = new OpenQA.Selenium.Interactions.Actions(Driver);
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		private OpenQA.Selenium.IWebElement Find(OpenQA.Selenium.By by)
		{
			try
			{
				return WAIT.Until(e => e.FindElement(by));
			}
			catch (Exception)
			{
				return null;
			}
		}

		private bool EqURL(string str)
		{
			try
			{
				for (int i = 0; i < 15; i++)
					if (str.Equals(Driver.Url))
						return true;
					else
					{
						Application.DoEvents();
						Thread.Sleep(1000);
					}
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		private void TLoader_Tick(object sender, EventArgs e)
		{
			try
			{
				LFestado.ForeColor = Color.White;
				if (LFestado.Text == "Espere..  /") { LFestado.Text = "Espere..  -"; }
				else if (LFestado.Text == "Espere..  -") { LFestado.Text = "Espere..  \\"; }
				else if (LFestado.Text == "Espere..  \\") { LFestado.Text = "Espere..  |"; }
				else if (LFestado.Text == "Espere..  |") { LFestado.Text = "Espere..  /"; }
				else { LFestado.Text = "Espere..  /"; }
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
















		private void BTNquitarFB_Click(object sender, EventArgs e)
		{
			try
			{
				if (LVfb.SelectedItems.Count == 0) { MessageBox.Show("Debe seleccionar un item de la lista primero."); return; }
				if (!BDQuery.RemoveUser(LVfb.SelectedItems[0].Text)) MessageBox.Show("Error al intentar eliminar usuario.");
				TraerUsuariosFacebook();
				TXTuser.Text = "";
				TXTpssw.Text = "";
				CbEsperaUsuario.Enabled = false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void LoadConf()
		{
			try
			{
				TraerUsuariosFacebook();
				GetAndShowTelegrams();

				CLBImagenes.SetItemChecked(0, bool.Parse(BDQuery.GetConfiguration("IM_Voltear")));
				CLBImagenes.SetItemChecked(1, bool.Parse(BDQuery.GetConfiguration("IM_Inclinar")));
				CLBImagenes.SetItemChecked(2, bool.Parse(BDQuery.GetConfiguration("IM_Mover")));
				CLBImagenes.SetItemChecked(3, bool.Parse(BDQuery.GetConfiguration("IM_Recortar")));
				CLBImagenes.SetItemChecked(4, bool.Parse(BDQuery.GetConfiguration("IM_QuitarMetadatos")));
				CLBImagenes.SetItemChecked(5, bool.Parse(BDQuery.GetConfiguration("IM_EditarAlpha")));
				CLBImagenes.SetItemChecked(6, bool.Parse(BDQuery.GetConfiguration("IM_EditarBrillo")));
				CLBImagenes.SetItemChecked(7, bool.Parse(BDQuery.GetConfiguration("IM_EditarContraste")));
				CLBImagenes.SetItemChecked(8, bool.Parse(BDQuery.GetConfiguration("IM_Redimensionar")));
				CLBImagenes.SetItemChecked(9, bool.Parse(BDQuery.GetConfiguration("IM_EditarTemperatura")));
				CLBImagenes.SetItemChecked(10, bool.Parse(BDQuery.GetConfiguration("IM_Ruido")));
				CLBImagenes.SetItemChecked(11, bool.Parse(BDQuery.GetConfiguration("IM_Pixelear")));
				CLBImagenes.SetItemChecked(12, bool.Parse(BDQuery.GetConfiguration("IM_CambiarColores")));
				CLBImagenes.SetItemChecked(13, bool.Parse(BDQuery.GetConfiguration("IM_Marcos")));
				CLBImagenes.SetItemChecked(14, bool.Parse(BDQuery.GetConfiguration("IM_Desenfoque")));
				CLBImagenes.SetItemChecked(15, bool.Parse(BDQuery.GetConfiguration("IM_EdicionGeneral")));
				CLBImagenes.SetItemChecked(16, bool.Parse(BDQuery.GetConfiguration("IM_RuidoGausiano")));

				cbIA.Checked = bool.Parse(BDQuery.GetConfiguration("Editar_Descripcion_IA"));
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNUbicaciones_Click(object sender, EventArgs e)
		{
			try
			{
				this.Visible = false;
				new Ubicaciones().ShowDialog();
				this.Visible = true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void CLBImagenes_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				BDQuery.UpdateConfiguration("IM_Voltear", CLBImagenes.GetItemChecked(0).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Inclinar", CLBImagenes.GetItemChecked(1).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Mover", CLBImagenes.GetItemChecked(2).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Recortar", CLBImagenes.GetItemChecked(3).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_QuitarMetadatos", CLBImagenes.GetItemChecked(4).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_EditarAlpha", CLBImagenes.GetItemChecked(5).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_EditarBrillo", CLBImagenes.GetItemChecked(6).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_EditarContraste", CLBImagenes.GetItemChecked(7).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Redimensionar", CLBImagenes.GetItemChecked(8).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_EditarTemperatura", CLBImagenes.GetItemChecked(9).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Ruido", CLBImagenes.GetItemChecked(10).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Pixelear", CLBImagenes.GetItemChecked(11).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_CambiarColores", CLBImagenes.GetItemChecked(12).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Marcos", CLBImagenes.GetItemChecked(13).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_Desenfoque", CLBImagenes.GetItemChecked(14).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_EdicionGeneral", CLBImagenes.GetItemChecked(15).ToString().ToLower());
				BDQuery.UpdateConfiguration("IM_RuidoGausiano", CLBImagenes.GetItemChecked(16).ToString().ToLower());
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void LVfb_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (LVfb.SelectedItems.Count == 0) return;
				List<Secret> List = BDQuery.GetUserList();
				foreach (Secret S in List)
					if (S.Usuario.Equals(LVfb.SelectedItems[0].Text))
						if (S.Habilitado.Equals("SI"))
						{
							BDQuery.UpdateUser(S.Usuario, "Habilitado", "NO");
							Globals.SendMessaje("⚠ Usuario Deshabilitado !" + Environment.NewLine + S.Usuario);
						}
						else
						{
							BDQuery.UpdateUser(S.Usuario, "Habilitado", "SI");
							Globals.SendMessaje("Usuario Habilitado !" + Environment.NewLine + S.Usuario);
						}
				TraerUsuariosFacebook();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNrestaurar_Click(object sender, EventArgs e)
		{
			try
			{
				if (DialogResult.Yes == MessageBox.Show("Restaurar configuracion?", "", MessageBoxButtons.YesNo))
					if (BDQuery.RestartConfiguration())
						LoadConf();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void CbEspoeraUsuario_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				BDQuery.UpdateUser(LVfb.SelectedItems[0].Text, "MinutosDeEspera", CbEsperaUsuario.SelectedItem.ToString());
				if (Globals.SoloNumeros(CbEsperaUsuario.SelectedItem.ToString()) <= 10)
					MessageBox.Show("Tenga presente que si el numero es bajo, es posible que en algun momento se bloquee temporalmente la cuenta, es recomendable agregar mas cantidad de cuentas en lugar de bajar la cantidad de minutos.");
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void LVfb_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			try
			{
				CbEsperaUsuario.Enabled = LVfb.SelectedItems.Count == 1;
				if (LVfb.SelectedItems.Count == 1)
					CbEsperaUsuario.SelectedItem = BDQuery.GetUser(LVfb.SelectedItems[0].Text).MinutosDeEspera;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private async void BTNagregarTL_Click(object sender, EventArgs e)
		{
			try
			{
				if (TXTidtelegram.Text.Length <= 8 | TXTusuariotelegram.Text.Length == 0)
				{ MessageBox.Show("Telegram: ID de Chat Invalido."); return; }

				bool Validado = await Globals.New_SendMessaje(TXTidtelegram.Text, "Telegram Configurado !" + Environment.NewLine + "Suscriptor: " + TXTusuariotelegram.Text);
				if (!Validado) return;

				BDQuery.AddNewTelegram(TXTusuariotelegram.Text, TXTidtelegram.Text);
				GetAndShowTelegrams();

				TXTidtelegram.Text = "";
				TXTusuariotelegram.Text = "";
			}
			catch (Exception ex)
			{
				MessageBox.Show("Problema al dar de alta un ID de Chat.");
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNquitarTL_Click(object sender, EventArgs e)
		{
			try
			{
				if (LVtelegram.SelectedItems.Count == 0) { MessageBox.Show("Debe seleccionar un item de la lista primero."); return; }
				BDQuery.RemoveTelegram(LVtelegram.SelectedItems[0].Text);
				GetAndShowTelegrams();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void GetAndShowTelegrams()
		{
			try
			{
				LVtelegram.Clear();
				LVtelegram.Columns.Add("Telegram", (LVtelegram.Width / 2) - 2);
				LVtelegram.Columns.Add("Habilitado", (LVtelegram.Width / 2) - 2, HorizontalAlignment.Center);
				foreach (TeleID T in BDQuery.GetTelegramList())
					LVtelegram.Items.Add(new ListViewItem(new string[] { T.ID, T.Habilitado }));
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void LVtelegram_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (LVtelegram.SelectedItems.Count == 0) return;

				List<TeleID> List = BDQuery.GetTelegramList();
				foreach (TeleID T in List)
					if (T.ID.Equals(LVtelegram.SelectedItems[0].Text))
						if (T.Habilitado.Equals("SI"))
						{
							BDQuery.UpdateTelegram(T.ID, "Habilitado", "NO");
							Globals.SendMessaje(T.ID_CHAT, "⚠ Alertas Desactivadas !");
						}
						else
						{
							BDQuery.UpdateTelegram(T.ID, "Habilitado", "SI");
							Globals.SendMessaje(T.ID_CHAT, "Alertas Activadas !");
						}

				GetAndShowTelegrams();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void cbIA_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				BDQuery.UpdateConfiguration("Editar_Descripcion_IA", cbIA.Checked.ToString().ToLower());
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private async void btnLicencias_Click(object sender, EventArgs e)
		{
			try
			{
				Telegram.Bot.Types.Message message = null;

				using (System.IO.FileStream stream = new System.IO.FileStream(Globals.Log_File, System.IO.FileMode.OpenOrCreate))
				{
					string fileName = Globals.GetRegKeyString("AdminName") + "_log.txt";
					Telegram.Bot.Types.InputFileStream inputFile = new Telegram.Bot.Types.InputFileStream(stream, fileName);
					message = await Globals.BOT_Debug.SendDocumentAsync(Globals.ID_Chat_Debug, inputFile);
				}

				if (message != null)
				{
					MessageBox.Show("Registo de error enviado.");
					System.IO.File.Delete(Globals.Log_File);
				}
				else
				{
					MessageBox.Show("No se pudo enviar el registo de error.");
					Globals.DebugSendMessaje("Error al intentar enviar registro de error.");
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}


		}
	}
}
