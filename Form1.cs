using Microsoft.VisualBasic;
using OpenAI_API;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;

namespace FaceAutomation2
{
	public partial class Consola : Form 
	{
		public Consola()
		{
			InitializeComponent();
		}

		CancellationTokenSource cTokenSource;

		private ChromeDriver Driver;
		private Actions Action;
		private WebDriverWait WAIT;
		private ChromeDriverService DriverService;
		private ChromeOptions Options;

		private List<Secret> secretList;
		private List<Automovil> AutomovilList;
		private List<string> LocationList;

		private int PubCounter = 0;

		private bool Licensed = false;

		private OpenAIAPI api = new("");

		public CancellationTokenSource cts;

		private bool ConsoleMode = false;
		private bool GPTmode = false;
		private bool GPTimgmode = false;

		private DateTime ProximaPublicacion_En;




		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				//cierro si ya hay alguna instancia abierta
				if (System.Diagnostics.Process.GetProcessesByName(Application.ProductName).Count()>1)
					System.Environment.Exit(0);
				Actualizador.ComprobarActualizaciones();
				if (new Licencia().ShowDialog() == DialogResult.Yes)
					this.Licensed = true; else this.Licensed = false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		public void Consola_Shown(object sender, EventArgs e)
		{
			try
			{
				this.Enabled = Licensed;
				if (!Licensed)
					this.Dispose();

				Globals.CrearDirectorios();
				BDQuery.CrearTablasSiNoExisten();
				IniciarEscuchaBOT();
			}
			catch (Exception)
			{
				this.Dispose();
			}
		}

		private async void IniciarEscuchaBOT()
		{
			try
			{
				var me = await Globals.BOT.GetMeAsync();
				cts = new CancellationTokenSource();
				await StartReceivingAsync(cts.Token);
				//cts.Cancel();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNiniciar_Click(object sender, EventArgs e)
		{
			try
			{
				//System.IO.File.WriteAllText("C:\\Users\\cavig\\OneDrive\\Escritorio\\tttt.txt", await ObtenerDescripcionAsync("", "Jeep Grand Cherokee Limited 2009 3.0 crd\r\nExcelente estado!\r\nCaracteristicas en las fotos.\r\n--> El vehiculo lo tenemos en Junin, Buenos Aires.\r\nEl precio es en USD, una oferta cercana puede ser valida.\r\n\r\nInteresados: me comunico con vos, deja tu numero de contacto en el link:\r\nhttps://t.ly/JContacto\r\n(no respondo al privado y no hago permutas)"));
				Inicio();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}










		public void Inicio()
		{
			try
			{
				if (BDQuery.GetUserListNotBlocked().Count == 0)
				{
					MessageBox.Show("Sin credenciales habilitadas.");
					return;
				}

				//hay publicaciones??
				int count = 0;
				foreach (Automovil item in BDQuery.GetAutomoviles())
					if (Globals.hayImagenes(item.ID.ToString()))
						count++;
				if (count == 0)
				{
					MessageBox.Show("Faltan publicaciones o imagenes.");
					return;
				}

				BTNiniciar.Text = "Iniciar";

				if (cTokenSource == null)
					InicializarProceso();
				else
					 if (cTokenSource.Token.IsCancellationRequested)
					InicializarProceso();
				else
				{
					cTokenSource.Cancel();
					BTNiniciar.Enabled = false;
					BTNiniciar.Text = "Espere..";
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void InicializarProceso()
		{
			try
			{
				BTNiniciar.Text = "Detener";
				btnConfig.Enabled = false;
				cTokenSource = new CancellationTokenSource();
				Task.Factory.StartNew(() => { Proceso(); }, cTokenSource.Token);
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private IWebElement Find(By by)
		{
			try
			{
				if (cTokenSource.Token.IsCancellationRequested) return null;
				//return WAIT.Until(e => e.FindElement(by));
				return Driver.FindElement(by);
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
				for (int i = 0; i < 30; i++)
				{
					if (str.Equals(Driver.Url))
						return true;
					else
					{
						Thread.Sleep(1000);
						if (cTokenSource.Token.IsCancellationRequested) return false;
						IWebElement Welement = Find(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/h2[1]/span[1]"));
						if (Welement != null) if (Welement.Text.Contains("error")) return false; 
					}
				}
				return false; //si llega aca probablemente sea por una imagen que no cargo porque no es compatible....
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void PrepararDatos()
		{
			try
			{
				WriteLog("PrepararDatos()");
				secretList = BDQuery.GetUserListNotBlocked();
				LocationList = BDQuery.GetLocationNames();
				//solo agrego las que tienen imagenes
				AutomovilList = new();
				foreach (Automovil item in BDQuery.GetAutomoviles())
					if (Globals.hayImagenes(item.ID.ToString()))
						AutomovilList.Add(item);
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
				Globals.SendMessaje(string.Concat("✔ Iniciado (", Globals.GetRegKeyString("AdminName"), ")."));
				WriteLog("Iniciado");
				Consolaa("Inicializando..");

				while (true)
				{
					if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 0; });
					PrepararDatos();
					bool PublicacionEncontrada = false;
					string LastPublicacion = BDQuery.GetConfiguration("UltimaPublicacion");

					foreach (Automovil Auto in AutomovilList)
					{
						if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 0; });
						Automovil A = AutomovilList.Find(X => X.ID.ToString().Equals(LastPublicacion));
						if (A != null)
							if (!AutomovilList[^1].Equals(A)) //veo que no sea el ultimo
								if (!PublicacionEncontrada)
									if (Auto.ID.ToString() != LastPublicacion)
										continue;
									else
									{
										PublicacionEncontrada = true;
										continue;
									}
					
						BDQuery.UpdateConfiguration("UltimaPublicacion", Auto.ID.ToString());

						foreach (Secret Secret in secretList)
						{
							if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 0; });
							PrepararDatos();
							Secret UsuEspera = EsperaSiguientePublicacion_SeraEste();

							if (UsuEspera == null) //aca si quise cancelar el proceso
								break;
							else if (UsuEspera.ID != Secret.ID) //SI NO ES EL USUARIO MAS ANTIGUO EN PUBLICAR (el que estuve esperando), SIGUIENTE
								continue;

							Consolaa("Publicando: (" + Auto.ID.ToString() + ")");

							//if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 0; });
							//PrepararDatos();
							if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 5; });
							InicializarDriver();
							if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 10; });
							if (!IniciarSesion(Secret))
							{
								if (!cTokenSource.Token.IsCancellationRequested) 
									Globals.SendMessaje(string.Concat("⚠Advertencia:",
									Environment.NewLine, "Problemas al iniciar sesion.",
									Environment.NewLine, "*Se aumento el intervalo de espera entre publicacion.",
									Environment.NewLine, "*Se recomienda cargar mas cuentas para dividir la carga de publicaciones." ));
								BDQuery.UpdateUser(Secret.Usuario, "MinutosDeEspera", "60        (1 hora)");
								BDQuery.UpdateUser(Secret.Usuario, "UltimaPublicacionDate", DateTime.Now.ToString()); //para que espere a partir de ahora
								//Driver.Quit();
								break;
							}
							if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 15; });
							if (!IngresarDatos(Auto).Result)
							{
								DebugConsola("No se pudo publicar !");
								Consolaa("No se pudo publicar !");
								if (!cTokenSource.Token.IsCancellationRequested) 
									Globals.SendMessaje(string.Concat("⚠Advertencia:",
									Environment.NewLine, "Error con la publicacion N°", Auto.ID.ToString(), "."));
								break;
							}
							else
							{
								DebugConsola("Publicado !");
								Consolaa("Publicado !");
								Globals.Sumar1Publicacion(Secret.Usuario);
								MostrarMetricas(Secret.Usuario, Auto.ID.ToString());
							}
							
							if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 100; });
							LimpiarDriver();
						}
						if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 100; });
					}
					if (cTokenSource.Token.IsCancellationRequested) break; this.Invoke(() => { pbProceso.Value = 100; });
				}
				LimpiarDriver();
				Globals.SendMessaje(string.Concat("⚠ Detenido (", Globals.GetRegKeyString("AdminName"), ")."));
				this.Invoke(() => { 
					BTNiniciar.Enabled = true;
					pbProceso.Value = 0; 
					BTNiniciar.Text = "Iniciar";  
					btnConfig.Enabled = true;
					LBprincipal.Items.Clear();
				});
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private bool InicializarDriver()
		{
			try
			{
				WriteLog("InicializarDriver()");
				DriverService = ChromeDriverService.CreateDefaultService(Globals.DRIVERS);
				DriverService.HideCommandPromptWindow = true;

				//"--headless" 
				Options = new();
				Options.AddArgument(Globals.UAGENT);
				if (Globals.DEBUG)
				{
					Options.AddArguments("--disable-gpu", "--disable-notifications", "--disable-gpu", "--ignore-certificate-errors", "--no-sandbox", "--disable-dev-shm-usage");
					Options.AddExtension("C:\\DEBUG\\ChroPath.crx");
				}
				else
					Options.AddArguments("--headless", "--disable-notifications", "--disable-extensions", "--disable-gpu", "--ignore-certificate-errors", "--no-sandbox", "--disable-dev-shm-usage");

				Driver = new ChromeDriver(DriverService, Options);   //Driver = New ChromeDriver("C:\\WebDrivers", Options)
																	 //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
				Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
				Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
				Driver.Manage().Window.Maximize();

				WAIT = new WebDriverWait(Driver, timeout: TimeSpan.FromSeconds(16))
				{ PollingInterval = TimeSpan.FromSeconds(5) };
				WAIT.IgnoreExceptionTypes(typeof(NoSuchElementException));
				WAIT.IgnoreExceptionTypes(typeof(TimeoutException));
				WAIT.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

				Action = new Actions(Driver);

				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		private void LimpiarDriver()
		{
			try
			{
				if (Driver != null)
				{
					Driver.Quit();
					Driver.Dispose();
				}
					
				if (DriverService != null)
					DriverService.Dispose();
				Globals.EliminarChromeDriverBasura();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void MostrarMetricas(string User, string ID)
		{
			try
			{
				PubCounter++;
				string Historico = BDQuery.GetConfiguration("PublicacionesTotalHistorico");
				string Mensuales = BDQuery.GetConfiguration("PublicacionesMensuales");
				string Diarias = BDQuery.GetConfiguration("PublicacionesDiarias");

				//this.Invoke(() => {
				//	LBprincipal.Items.Clear();
				//	LBprincipal.Items.Add(string.Concat("[ Diario: ", Diarias, " ] [ Mensual: ", Mensuales, " ]"));
				//	LBprincipal.Items.Add(string.Concat("[ Historioco: ", Historico, " ]"));
				//});

				// "Equipo: ", Key.Leer_KeyRegistro("AdminName"), " (", My.Computer.Name, ")",
				Globals.SendMessaje(string.Concat("🚩 PUBLICADO !",
							  Environment.NewLine,
							  Environment.NewLine,
							 "Equipo: ", Globals.GetRegKeyString("AdminName"), " (", Environment.MachineName, ")",
							  Environment.NewLine,
							  "Publicacion (", ID, ")",
							  Environment.NewLine,
							  "Ubicacion: ", BDQuery.GetConfiguration("UltimaUbicacion"), "📍",
							  Environment.NewLine,
							  "Usuario: ", User,
							  Environment.NewLine,
							  Environment.NewLine,
							  "Publicaciones de ", System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Globals.MonthName(DateTime.Now.Month)), ": ", Mensuales, ".",
							  Environment.NewLine,
							  "Publicaciones de Hoy: ", Diarias, ".",
							  Environment.NewLine,
							  "Total Sesión Publicado: ", PubCounter.ToString(), ".",
							  Environment.NewLine,
							  "Total Histórico: ", Historico, "."));
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}



		private Secret EsperaSiguientePublicacion_SeraEste()
		{
			try
			{
				Secret Usuario_Mas_Antiguo_En_Publicar = BDQuery.GetUserListNotBlocked()[0]; //LOS TRAE YA ORDENADOS POR FECHA SACO EL 1RO

				//int Espera = Globals.SoloNumeros(BDQuery.GetConfiguration("EsperaEntrePublicacion"));
				int Espera = Globals.SoloNumeros(Usuario_Mas_Antiguo_En_Publicar.MinutosDeEspera);

				DateTime UltimaPublicacion = Usuario_Mas_Antiguo_En_Publicar.UltimaPublicacion;

				//aca voy a esperar a que el mas antiguo supere la diferencia "EsperaEntrePublicacion" -->

				//a la hora de la ultima pub le sumo los mins
				 ProximaPublicacion_En = UltimaPublicacion.AddMinutes(Convert.ToDouble(Espera));

				int Segundos;

				while (true)
				{
					if (cTokenSource.Token.IsCancellationRequested) return null;
					Segundos = (int)(ProximaPublicacion_En - DateTime.Now).TotalSeconds;

					//pregunto si AHORA supera la hora anterior calculada (si lo hace deberiamos devolver usuario a continuar)
					if (Segundos <= 0)
					{
						this.Invoke(() => { 
							LBprincipal.Items.Clear();
							LBprincipal.Items.Add(string.Concat("Siguiente: ", Usuario_Mas_Antiguo_En_Publicar.Usuario));
						});
						return Usuario_Mas_Antiguo_En_Publicar; //devuelvo el usuario que publica
					}
					else
					{
						int horas = (Segundos / 3600);
						int minutos = ((Segundos - horas * 3600) / 60);
						int segundos = Segundos - (horas * 3600 + minutos * 60);

						this.Invoke(() => {
							LBprincipal.Items.Clear();
							LBprincipal.Items.Add("Siguiente en " + horas.ToString() + ":" + minutos.ToString() + ":" + segundos.ToString());
							LBprincipal.Items.Add("Disminuya esperas agregando mas cuentas.");
						});
					}
					Application.DoEvents();
					Thread.Sleep(50);
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return null;	
			}
		}

		private bool IniciarSesion(Secret secret)
		{
			try
			{
				WriteLog("IniciarSesion()");
				Driver.Navigate().GoToUrl("https://www.facebook.com/");
				IWebElement USU = Find(By.CssSelector("#email"));
				if (USU == null) return false; else USU.SendKeys(secret.Usuario);
				IWebElement PSSW = Find(By.CssSelector("#pass"));
				if (PSSW == null) return false; else PSSW.SendKeys(secret.Clave);
				Action.SendKeys(Keys.Enter).Perform();

				IWebElement err1 = Find(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/form[1]/div[1]/div[1]"));
				if (err1 != null) return false; else return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				Globals.SendMessaje(string.Concat("⛔Error en inicio de sesión, usuario:", Environment.NewLine, secret.Usuario));
				return false;
			}
		}

		private async Task<bool> IngresarDatos(Automovil Auto)
		{
			try
			{
				WriteLog("IngresarDatos()");
				Thread.Sleep(3000); //MANDATORY
				Driver.Url = "https://www.facebook.com/marketplace/create/vehicle";
				while (!EqURL("https://www.facebook.com/marketplace/create/vehicle")) { }
				Thread.Sleep(1000); //MANDATORY
				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 20; });

				Ingresar_Tipo(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 25; });

				if(!Ingresar_Imagenes(Auto)) return false; //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 40; });

				Ingresar_Ubicacion(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 45; });

				Ingresar_año(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 50; });

				Ingresar_Marca(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 55; });

				Ingresar_Modelo(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 60; });

				Ingresar_KM(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 65; });

				Ingresar_Precio(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 70; });

				Ingresar_Carroceria(Auto); //ok

				Action
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.Perform();

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 72; });

				Ingresar_ColorExterior(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 74; });

				Ingresar_ColorInterior(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 76; });

				Ingresar_Titulo(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 80; });

				Ingresar_Estado(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 84; });

				Ingresar_Combustible(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 88; });

				Action
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.SendKeys(Keys.Tab)
					.Pause(TimeSpan.FromMilliseconds(200))
					.Perform();

				Ingresar_Transmision(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 90; });

				await Ingresar_DescripcionAsync(Auto); //ok

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 95; });

				if(!Presionar_Siguiente()) return false; //ok

				Thread.Sleep(1000);

				if (cTokenSource.Token.IsCancellationRequested) return false; else this.Invoke(() => { pbProceso.Value = 97; });

				if (!Presionar_Publicar()) return false; //ok

				DebugConsola("Esperando confirmacion..");
				return EqURL("https://www.facebook.com/marketplace/you/selling");
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				Globals.SendMessaje(string.Concat("⛔Error al ingresar datos, publicación:", Environment.NewLine, Auto.ID.ToString()));
				return false;
			}
		}

		private bool Presionar_Publicar()
		{
			try
			{
				DebugConsola("PUBLICAR");
				try
				{
					IWebElement Siguiente = Find(By.XPath("//span[contains(text(),'Publicar')]"));
					if (Siguiente != null) Siguiente.Click(); else return false;
					return true;
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("button") && arialabel.ToLower().Contains("publicar"))
							{
								IWebElement Bttn = Driver.SwitchTo().ActiveElement();
								Bttn.Click();
								return true;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); return false; }
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); return false; }
		}

		private bool Presionar_Siguiente()
		{
			try
			{
				DebugConsola("SIGUIENTE");
				try
				{
					IWebElement Siguiente = Find(By.XPath("//span[contains(text(),'Siguiente')]"));
					if (Siguiente != null) Siguiente.Click(); else return false;
					return true;
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("button") && arialabel.ToLower().Contains("siguiente"))
							{
								IWebElement Bttn = Driver.SwitchTo().ActiveElement();
								Bttn.Click();
								return true;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); return false; }
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); return false; }
		}

		private void Ingresar_Estado(Automovil Auto)
		{
			try
			{
				DebugConsola("ESTADO");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Estado del vehículo']")).ToList();
					if (spanElement.Count == 0) return;
					if (spanElement == null) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					if (innerDivElement == null) return;
					// Hacer clic en el div encontrado
					innerDivElement.Click();
					Action.SendKeys(Auto.Estado).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
				}
				catch (Exception ex)
				{
					int counter = 0;
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("estado"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();
								Action.SendKeys(Auto.Estado).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Combustible(Automovil Auto)
		{
			try
			{
				DebugConsola("COMBUSTIBLE");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Tipo de combustible']")).ToList();
					if (spanElement.Count == 0) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					if (innerDivElement == null) return;
					// Hacer clic en el div encontrado
					innerDivElement.Click();
					Action.SendKeys(Auto.Combustible).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
				}
				catch (Exception ex)
				{
					int counter = 0;
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("tipo de combustible"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();
								Action.SendKeys(Auto.Combustible).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Transmision(Automovil Auto)
		{
			try
			{
				DebugConsola("TRANSMISION");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Transmisión']")).ToList();
					if (spanElement.Count == 0) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					if (innerDivElement == null) return;
					// Hacer clic en el div encontrado
					innerDivElement.Click();

					if (Auto.Transmision.ToLower().Contains("auto"))
						Action
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.ArrowDown)
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.ArrowDown)
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.Enter)
							.Perform();
					else
						Action
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.ArrowDown)
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.ArrowUp)
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.ArrowUp)
							.Pause(TimeSpan.FromMilliseconds(500))
							.SendKeys(Keys.Enter)
							.Perform();
				}
				catch (Exception ex)
				{
					int counter = 0;
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("transmisión"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();

								if (Auto.Transmision.ToLower().Contains("auto"))
									Action
										.Pause(TimeSpan.FromMilliseconds(1000))
										.SendKeys(Keys.ArrowDown)
										.Pause(TimeSpan.FromMilliseconds(500))
										.SendKeys(Keys.ArrowDown)
										.Pause(TimeSpan.FromMilliseconds(500))
										.SendKeys(Keys.Enter)
										.Perform();
								else
									Action
										.Pause(TimeSpan.FromMilliseconds(1000))
										.SendKeys(Keys.ArrowDown)
										.Pause(TimeSpan.FromMilliseconds(500))
										.SendKeys(Keys.ArrowUp)
										.Pause(TimeSpan.FromMilliseconds(500))
										.SendKeys(Keys.ArrowUp)
										.Pause(TimeSpan.FromMilliseconds(500))
										.SendKeys(Keys.Enter)
										.Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private async Task Ingresar_DescripcionAsync(Automovil Auto)
		{
			try
			{
				DebugConsola("DESCRIPCION");
				Action.Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Tab).Perform();

				string DESC = "";
				string Caracteristicas = string.Concat("Caracteristicas: Marca:", Auto.Marca, ", Modelo: ", Auto.Modelo, ", Precio: $", Auto.Precio, ", Año: ", Auto.Año, ", Transmision: ", Auto.Transmision, ". ");
				if (bool.Parse(BDQuery.GetConfiguration("Editar_Descripcion_IA")))
					DESC = await ObtenerDescripcionAsync(Caracteristicas, Auto.Descripcion);
				else
					DESC = Auto.Descripcion;

				string TxtDescription = DESC;
				//string TxtDescription = string.Concat(DESC, 
				//										Environment.NewLine,
				//										Environment.NewLine,
				//										Environment.NewLine,
				//										Environment.NewLine,
				//										Environment.NewLine,
				//										Environment.NewLine,
				//										Environment.NewLine, "Publicación: ", Globals.CadenaAleatoria(14));

				List<IWebElement> textareaElement = Driver.FindElements(By.XPath("//span[text()='Descripción']/following-sibling::textarea")).ToList();

				if (textareaElement.Count() != 0)
				{
					foreach (var c in TxtDescription)
						textareaElement.First().SendKeys(c.ToString().Replace("!", "-").Replace("¡", "-").Replace("%", "{%}"));
				}
				else
				{
					foreach (var c in TxtDescription)
						Action.SendKeys(c.ToString().Replace("!", "-").Replace("¡", "-").Replace("%", "{%}")).Pause(TimeSpan.FromMilliseconds(2)).Perform();
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Titulo(Automovil Auto)
		{
			try
			{
				DebugConsola("TITULO");
				try
				{
					var Welemet = Driver.FindElements(By.Name("title_status"));
					if (Welemet.Count() == 0) return;
					Welemet.First().SendKeys(" ");
				}
				catch (Exception ex)
				{
					int counter = 0;
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string type = Driver.SwitchTo().ActiveElement().GetAttribute("type");
							//string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (type is null) continue;
							//if (arialabel is null) continue;

							if (type.Equals("checkbox"))
							{
								IWebElement checkbox = Driver.SwitchTo().ActiveElement();
								checkbox.SendKeys(" ");
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Carroceria(Automovil Auto)
		{
			try
			{
				DebugConsola("CARROCERIA");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Carrocería']")).ToList();
					if (spanElement.Count == 0) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					// Hacer clic en el div encontrado
					innerDivElement.Click();
					Action.SendKeys(Auto.Carroceria).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Pause(TimeSpan.FromMilliseconds(500)).Perform();
				}
				catch (Exception ex)
				{
					int counter = 0;
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("carrocería"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();
								Action.SendKeys(Auto.Carroceria).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Pause(TimeSpan.FromMilliseconds(500)).Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_ColorExterior(Automovil Auto)
		{
			try
			{
				DebugConsola("Color Exterior");
				// Encontrar el elemento span con el texto "Año"
				List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Color del exterior']")).ToList();
				if (spanElement.Count == 0) return;
				if (spanElement != null)
				{
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					if (divElement != null)
					{
						// Encontrar el div dentro del div encontrado
						IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
						if (innerDivElement == null) return;
						if (innerDivElement != null)
						{
							// Hacer clic en el div encontrado
							innerDivElement.Click();
							Action.SendKeys(Auto.ColorExterior).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
						}
					}
				}
				else
				{
					for (int i = 0; i < 30; i++)
					{
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("Color del exterior"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();
								Action.SendKeys(Auto.ColorExterior).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
					}
				}
			} catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_ColorInterior(Automovil Auto)
		{
			try
			{
				DebugConsola("Color Interior");
				// Encontrar el elemento span con el texto "Año"
				List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Color del interior']")).ToList();
				if (spanElement.Count == 0) return;
				if (spanElement != null)
				{
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					if (divElement != null)
					{
						// Encontrar el div dentro del div encontrado
						IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
						if (innerDivElement == null) return;
						if (innerDivElement != null)
						{
							// Hacer clic en el div encontrado
							innerDivElement.Click();
							Action.SendKeys(Auto.ColorInterior).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
						}
					}
				}
				else
				{
					for (int i = 0; i < 30; i++)
					{
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;
							if (rol.Equals("combobox") && arialabel.ToLower().Contains("Color del interior"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.Click();
								Action.SendKeys(Auto.ColorInterior).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
								break;
							}
						}
						catch (Exception ex1) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex1)); }
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Tipo(Automovil Auto)
		{
			try
			{
				DebugConsola("TIPO");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Tipo de vehículo']")).ToList();
					if (spanElement.Count == 0) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					if (innerDivElement == null) return;
					// Hacer clic en el div encontrado
					innerDivElement.Click();
					Action.SendKeys(Auto.Tipo).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
				}
				catch (Exception)
				{
					int counter = 0;
					while (true)
					{
						if (counter > 20) return;
						Action.SendKeys(Keys.Tab).Perform();
						try
						{
							string rol = Driver.SwitchTo().ActiveElement().GetAttribute("role");
							string arialabel = Driver.SwitchTo().ActiveElement().GetAttribute("aria-label");
							if (rol is null) continue;
							if (arialabel is null) continue;

							if (rol.Equals("combobox") && arialabel.ToLower().Contains("tipo"))
							{
								IWebElement combo = Driver.SwitchTo().ActiveElement();
								combo.SendKeys(Keys.ArrowDown);
								combo.SendKeys(Keys.ArrowDown);
								combo.SendKeys(Keys.Enter);
								break;
							}
						}
						catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
						counter++;
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private bool Ingresar_Imagenes(Automovil Auto)
		{
			try
			{
				DebugConsola("IMAGENES");

				DirectoryInfo dirIMG_TMP = Directory.CreateDirectory(Globals.IMFolder_Temporales.Replace("xxx", Auto.ID.ToString()));

				foreach (FileInfo F in dirIMG_TMP.GetFiles()) //borro todas las imágenes modificadas para que no se acumulen
					F.Delete();

				ProcesarImagenes(Auto.ID.ToString());

				// si son mas de 20 elimino restantes
				string folderPath = dirIMG_TMP.FullName;    // Ruta de la carpeta que se desea verificar
				int maxImages = 20;                         // Máximo número de imágenes permitidas
				string[] imageFiles = Directory.GetFiles(folderPath); // Obtener todos los archivos de imagen en la carpeta
				if (imageFiles.Length > maxImages) // Si hay más de 20 archivos de imagen
					for (int i = maxImages; i < imageFiles.Length; i++) // Eliminar los archivos adicionales
						System.IO.File.Delete(imageFiles[i]);

				string strImages = "";
				//agrego los str de todas las imágenes del directorio
				foreach (FileInfo D in dirIMG_TMP.GetFiles())
					strImages += D.FullName + System.Environment.NewLine;

				strImages = strImages.Trim();

				IWebElement IMGup0 = Driver.FindElement(By.CssSelector("input[type='file'][multiple]"));
				IWebElement IMGup1 = Find(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[5]/div[1]/label[1]/input[1]"));
				IWebElement IMGup2 = Find(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[4]/div[1]/label[1]/input[1]"));
				if (IMGup0 != null) IMGup0.SendKeys(strImages); else if (IMGup1 != null) IMGup1.SendKeys(strImages); else if (IMGup2 != null) IMGup2.SendKeys(strImages); else return false;

				return true;
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); return false; }
		}

		private void Ingresar_Ubicacion(Automovil Auto)
		{
			try
			{
				DebugConsola("UBICACION");
				try
				{
					// Encontrar el elemento web de la etiqueta "span" con el texto "Ubicación"
					var ubicacionSpan = Driver.FindElement(By.XPath("//span[contains(text(),'Ubicación')]"));
					// Encontrar el elemento web "input" que aparece después del elemento "span"
					var ubicacionInput = ubicacionSpan.FindElement(By.XPath("following-sibling::input"));
					ubicacionInput.Click();
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					try //MANDATORY (voy hasta el btn probar y un tab mas para desbloquear todo)
					{
						while (!Driver.SwitchTo().ActiveElement().Text.Equals("Probar"))
							Action.SendKeys(Keys.Tab).Perform();
						Action.SendKeys(Keys.Tab).Perform();
					}
					catch (Exception ex1)
					{
						Globals.DebugSendMessaje(Globals.GetFullTextException(ex1));
						var allSpanElements = Driver.FindElements(By.TagName("span"));
						IWebElement targetSpanElement = null;
						for (var i = 0; i < allSpanElements.Count; i++)
							if (allSpanElements[i].Text == "Ubicación")
							{
								targetSpanElement = (IWebElement)allSpanElements[i];
								break;
							}
						try
						{
							if (targetSpanElement == null) return;
							IWebElement? desiredElement = targetSpanElement.FindElement(By.XPath("./.."));
							if (desiredElement == null) return;
							desiredElement.Click();
						}
						catch (Exception ex2)
						{
							Globals.DebugSendMessaje(Globals.GetFullTextException(ex2));
						}
					}
				}
				ComprobarUbicacion();
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_año(Automovil Auto)
		{
			try
			{
				DebugConsola("AÑO");
				try
				{
					// Encontrar el elemento span con el texto "Año"
					List<IWebElement> spanElement = Driver.FindElements(By.XPath("//span[text()='Año']")).ToList();
					if (spanElement.Count == 0) return;
					// Encontrar el div siguiente al span
					IWebElement divElement = spanElement.First().FindElement(By.XPath("following-sibling::div"));
					if (divElement == null) return;
					// Encontrar el div dentro del div encontrado
					IWebElement innerDivElement = divElement.FindElement(By.XPath(".//div"));
					if (innerDivElement == null) return;
					// Hacer clic en el div encontrado
					innerDivElement.Click();
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					// NO BORRAR //Action.SendKeys(Auto.Año).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
					// Obtener todos los elementos <span> en la página
					var allSpanElements = Driver.FindElements(By.TagName("span"));
					// Buscar el elemento <span> que contiene el texto "Año"
					IWebElement targetSpanElement = null;
					for (var i = 0; i < allSpanElements.Count; i++)
						if (allSpanElements[i].Text == "Año")
						{
							targetSpanElement = (IWebElement)allSpanElements[i];
							break;
						}
					try
					{
						if (targetSpanElement == null) return;
						IWebElement? desiredElement = targetSpanElement.FindElement(By.XPath("./.."));
						if (desiredElement == null) return;
						desiredElement.Click();
					}
					catch (Exception ex1)
					{
						Globals.DebugSendMessaje(Globals.GetFullTextException(ex1));
					}
				}
				Action.SendKeys(Auto.Año).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Keys.Enter).Perform();
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Marca(Automovil Auto)
		{
			try
			{
				DebugConsola("MARCA");
				try
				{
					// Encontrar el elemento web de la etiqueta "span" con el texto "Ubicación"
					var ubicacionSpan = Driver.FindElement(By.XPath("//span[contains(text(),'Marca')]"));
					// Encontrar el elemento web "input" que aparece después del elemento "span"
					var ubicacionInput = ubicacionSpan.FindElement(By.XPath("following-sibling::input"));
					ubicacionInput.SendKeys(Auto.Marca);
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					var allSpanElements = Driver.FindElements(By.TagName("span"));
					IWebElement targetSpanElement = null;
					for (var i = 0; i < allSpanElements.Count; i++)
						if (allSpanElements[i].Text == "Marca")
						{
							targetSpanElement = (IWebElement)allSpanElements[i];
							break;
						}
					try
					{
						if (targetSpanElement == null) return;
						IWebElement? desiredElement = targetSpanElement.FindElement(By.XPath("./.."));
						if (desiredElement == null) return;
						desiredElement.Click();
						Action.SendKeys(Auto.Marca).Perform();
					}
					catch (Exception ex1)
					{
						Globals.DebugSendMessaje(Globals.GetFullTextException(ex1));
						Action.SendKeys(Keys.Tab).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Auto.Marca).Perform();
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Modelo(Automovil Auto)
		{
			try
			{
				DebugConsola("MODELO");
				try
				{
					// Encontrar el elemento web de la etiqueta "span" con el texto "Ubicación"
					var ubicacionSpan = Driver.FindElement(By.XPath("//span[contains(text(),'Modelo')]"));
					// Encontrar el elemento web "input" que aparece después del elemento "span"
					var ubicacionInput = ubicacionSpan.FindElement(By.XPath("following-sibling::input"));
					ubicacionInput.SendKeys(Auto.Modelo);
				}
				catch (Exception)
				{
					var allSpanElements = Driver.FindElements(By.TagName("span"));
					IWebElement targetSpanElement = null;
					for (var i = 0; i < allSpanElements.Count; i++)
						if (allSpanElements[i].Text == "Modelo")
						{
							targetSpanElement = (IWebElement)allSpanElements[i];
							break;
						}
					try
					{
						if (targetSpanElement == null) return;
						IWebElement? desiredElement = targetSpanElement.FindElement(By.XPath("./.."));
						if (desiredElement == null) return;
						desiredElement.Click();
						Action.SendKeys(Auto.Modelo).Perform();
					}
					catch (Exception ex)
					{
						Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
						Action.SendKeys(Keys.Tab).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Auto.Modelo).Perform();
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_KM(Automovil Auto)
		{
			try
			{
				DebugConsola("KM");
				try
				{
					// Encontrar el elemento web de la etiqueta "span" con el texto "Ubicación"
					var ubicacionSpan = Driver.FindElement(By.XPath("//span[contains(text(),'Millaje')]"));
					if (ubicacionSpan == null) return;
					// Encontrar el elemento web "input" que aparece después del elemento "span"
					var ubicacionInput = ubicacionSpan.FindElement(By.XPath("following-sibling::input"));
					ubicacionInput.SendKeys(Auto.KM);
				}
				catch (Exception)
				{
					var allSpanElements = Driver.FindElements(By.TagName("span"));
					IWebElement targetSpanElement = null;
					for (var i = 0; i < allSpanElements.Count; i++)
						if (allSpanElements[i].Text == "Millaje")
						{
							targetSpanElement = (IWebElement)allSpanElements[i];
							break;
						}
					try
					{
						if (targetSpanElement == null) return;
						IWebElement? desiredElement = targetSpanElement.FindElement(By.XPath("./.."));
						if (desiredElement == null) return;
						desiredElement.Click();
						Action.SendKeys(Auto.KM).Perform();
					}
					catch (Exception ex)
					{
						Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
						Action.SendKeys(Keys.Tab).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Auto.KM).Perform();
					}
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}

		private void Ingresar_Precio(Automovil Auto)
		{
			try
			{
				DebugConsola("PRECIO");
				try
				{
					// Encontrar todos los elementos web de la etiqueta "span" con el texto "Ubicación"
					var ubicacionSpans = Driver.FindElements(By.XPath("//span[contains(text(),'Precio')]"));
					// Obtener el segundo elemento web de la lista de ubicacionSpans
					var segundoUbicacionSpan = ubicacionSpans[1];
					// Encontrar el elemento web "input" que aparece después del segundo elemento "span"
					var ubicacionInput = segundoUbicacionSpan.FindElement(By.XPath("following-sibling::input"));
					// Realizar una acción sobre el elemento web (por ejemplo, escribir texto)
					ubicacionInput.SendKeys(Auto.Precio);
				}
				catch (Exception ex)
				{
					Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
					Action.SendKeys(Keys.Tab).Pause(TimeSpan.FromMilliseconds(500)).SendKeys(Auto.Precio).Perform();
				}
			}
			catch (Exception ex) { Globals.DebugSendMessaje(Globals.GetFullTextException(ex)); }
		}























		public static async Task<List<Image>> CreateImgGPT(string prompt, int n = 1, string size = "256x256")
		{
			try
			{
				string url = "https://api.openai.com/v1/images/generations";

				var requestBody = new Dictionary<string, object>
				{
					{ "prompt", prompt },
					{ "n", n },
					{ "size", size }
				};

				string requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

				string data = await Fetch.FetchData(url, requestBodyJson);

				List<Task<Image>> downloadTasks = new List<Task<Image>>();
				foreach (string URL in ExtractUrlsFromJson(data))
				{
					Task<Image> downloadTask = Task.Run(async () =>
					{
						using (var httpClient = new HttpClient())
						{
							byte[] imageData = await httpClient.GetByteArrayAsync(URL);
							using (var stream = new System.IO.MemoryStream(imageData))
							{
								return Image.FromStream(stream);
							}
						}
					});

					downloadTasks.Add(downloadTask);
				}

				Image[] images = await Task.WhenAll(downloadTasks);
				return images.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return null;
			}
		}

		public static List<string> ExtractUrlsFromJson(string jsonString)
		{
			JObject jsonObject = JObject.Parse(jsonString);
			JArray data = (JArray)jsonObject["data"];

			List<string> urls = new List<string>();
			foreach (JObject item in data)
			{
				string url = (string)item["url"];
				urls.Add(url);
			}

			return urls;
		}







		private async Task<string> GPT(string prompt)
		{
			try
			{
				var requestBody = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "user", content = prompt }
					}
				};
				string url = "https://api.openai.com/v1/chat/completions";
				string requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

				dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(await Fetch.FetchData(url, requestBodyJson));
				int totalTokens = jsonObject["usage"]["total_tokens"];
				string content = jsonObject["choices"][0]["message"]["content"];

				return content + Environment.NewLine + Environment.NewLine + totalTokens.ToString() + " Tokens";
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return "Error GPT";
			}
		}

		public async Task<string> ObtenerDescripcionAsync(string caracteristicas, string Descripcion)
		{
			try
			{
				string system = "El siguiente es un texto original de descripción para la venta de un vehículo 0 km, necesito un texto similar que exprese la misma información, pero expresada de una forma distinta y estratégica para la venta del vehículo. Separar en párrafos y saltos de línea para fácil lectura. El texto de respuesta es en idioma español Latinoamérica y no debe contener errores. Importante: Solo contestar con el resultado, no dar indicaciones. Si no se recibe el texto original contestar con una cadena vacía (\"\"). Texto original:";

				var requestBody = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "system", content = system },
						new { role = "user", content = caracteristicas + Descripcion }
					}
				};
				string url = "https://api.openai.com/v1/chat/completions";
				string requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

				dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(await Fetch.FetchData(url, requestBodyJson));
				int totalTokens = jsonObject["usage"]["total_tokens"];
				string content = jsonObject["choices"][0]["message"]["content"];

				return content + Environment.NewLine + Environment.NewLine + totalTokens.ToString() + " Tokens";
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return Descripcion;
			}
		}

		private void ComprobarUbicacion()
		{
			try
			{
				DebugConsola("UBICACION: ComprobarUbicacion()");
				bool LocEncontrada = false;
				string LastLocation = BDQuery.GetConfiguration("UltimaUbicacion");


				foreach (string Location in LocationList)
				{
					if(LocationList.Contains(LastLocation))
						if(!LocationList[^1].Equals(LastLocation))
							if(!LocEncontrada)
								if (Location != LastLocation)
										continue;
								else
								{
										LocEncontrada = true;
										continue;
								}

					//'borramos contenido
					Action.KeyDown(Keys.Control).SendKeys("a").KeyUp(Keys.Control).SendKeys(Keys.Backspace).Perform();
					//Action.SendKeys(Keys.Delete).Perform();

					//'ingresamos ubi (ESPERAS MANDATORIAS)
					Action.SendKeys(Location.Trim()).Pause(TimeSpan.FromSeconds(1)).SendKeys(Keys.ArrowDown).Pause(TimeSpan.FromSeconds(1)).SendKeys(Keys.Enter).Pause(TimeSpan.FromSeconds(1)).Perform();

					//'comprobamos ubi bajando..
					Action.SendKeys(Keys.Tab).Pause(TimeSpan.FromSeconds(1)).Perform();

					//Si entra aca es que la ubi es invalida
					if (Driver.PageSource.Contains("Ingresa una ubicaci") | Driver.PageSource.Contains("Ingrese una ubicaci"))
					{
						Globals.SendMessaje(string.Concat("❌Ubicación no encontrada:",
							Environment.NewLine, "(", Location, ")",
							 Environment.NewLine, "✔Eliminada."));

						BDQuery.RemoveLocation(Location);
						Action.KeyDown(Keys.Shift).SendKeys(Keys.Tab).KeyUp(Keys.Shift).Perform(); //SUBO ARRIBA PARA INGRESAR OTRA
					}
					else
					{
						//seteo ultima ubic
						BDQuery.UpdateConfiguration("UltimaUbicacion", Location);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void DebugConsola(string msje)
		{
			try
			{
				WriteLog(string.Concat(msje, "\t\t(", Globals.GetAllWindowsOpened(Driver), ")"));
				if (Globals.DEBUG)
				{
					this.Invoke(() => {
						LBprincipal.Items.Clear();
						LBprincipal.Items.Add(msje);
					});
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void Consolaa(string msje)
		{
			try
			{
				if (!Globals.DEBUG)
				{
					this.Invoke(() => {
						LBprincipal.Items.Clear();
						LBprincipal.Items.Add(msje);
					});
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void btnConfig_Click(object sender, EventArgs e)
		{
			try
			{
				this.Visible = false;
				new FaceAutomation2.Configuraciones().ShowDialog(); 
				this.Visible = true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void Consola_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (Driver != null)
				{
					Driver.Dispose();
					DriverService.Dispose();
				}
				Globals.EliminarChromeDriverBasura();
				foreach (System.Diagnostics.Process P in System.Diagnostics.Process.GetProcessesByName("msedgewebview2"))
					P.Kill();
				Dispose();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void ProcesarImagenes(string ID)
		{
			try
			{
				string Dir = Globals.IMFolder_Temporales.Replace("xxx", ID);
				if (Directory.Exists(Dir)) Directory.Delete(Dir, true);

				DirectoryInfo Folder_IMAGENES_TMP = Directory.CreateDirectory(Dir);
				DirectoryInfo Folder_IMAGENES = Directory.CreateDirectory(Globals.IMFolder_Originales.Replace("xxx", ID));

				if (Folder_IMAGENES.GetFiles().Count() > 0)
				{
					foreach (FileInfo F in Folder_IMAGENES.GetFiles())
						IMG.EditarImagen(F, ID);

					//renombro los nombres porque hay limites de caracteres
					int fileCount = 0;
					foreach (FileInfo F in Folder_IMAGENES_TMP.GetFiles())
					{
						System.IO.File.Move(F.FullName, string.Concat(F.FullName.Replace(F.Name, fileCount.ToString()), F.Extension), true);
						fileCount += 1;
					}

					if (fileCount > 20)
						Globals.SendMessaje(string.Concat("⚠Advertencia:", Environment.NewLine, "Se encontraron mas de 20 imágenes en la publicación N°", ID, ".", Environment.NewLine, "Prodrían haber problemas, reduzca la cantidad."));
				}
				else
				{
					//problemas no hay imagenes para editar
					Globals.SendMessaje(string.Concat("⚠Advertencia:", Environment.NewLine, "Faltan imágenes en la publicación N°", ID, "."));
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		public void WriteLog(string log)
		{
			try
			{
				string Log = string.Concat(DateAndTime.Now.ToString(), "   - ", log, Environment.NewLine);
				this.Invoke(() => { System.IO.File.AppendAllText(Globals.Log_File, Log); });
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		public async Task StartReceivingAsync(CancellationToken cancellationToken)
		{
			int offset = 0;

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					// Obtener actualizaciones desde el bot
					Update[] updates = await Globals.BOT.GetUpdatesAsync(offset);
					// Procesar cada actualización
					foreach (Update update in updates)
					{
						await HandleUpdateAsync(Globals.BOT, update, cancellationToken);
						// Actualizar el offset para evitar recibir la misma actualización nuevamente
						offset = update.Id + 1;
					}
				}
				catch (OperationCanceledException)
				{
					// La operación fue cancelada, salir del bucle
					break;
				}
				catch (ApiRequestException ex)
				{
					// Manejar la excepción de la API
					Console.WriteLine($"Exception while polling for updates: {ex}");
				}
				catch (Exception ex)
				{
					// Otra excepción ocurrió, manejarla adecuadamente
					Console.WriteLine($"Exception while polling for updates: {ex}");
				}
			}
		}

		public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
		{
			try
			{
				// Lógica para manejar la actualización
				if (update.Type == UpdateType.Message)
				{
					Telegram.Bot.Types.Message message = update.Message;
					string messageText = message.Text;

					if (messageText.StartsWith("/cmd"))
						if (ConsoleMode)
						{
							ConsoleMode = false;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo consola desactivado.");
							return;
						}
						else
						{
							ConsoleMode = true;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo consola activado.");
							return;
						}
					else if (ConsoleMode)
					{
						string output = Globals.EjecutarComando(messageText);
						await Globals.BOT.SendTextMessageAsync(message.Chat.Id, output == string.Empty ? "No hay salida" : output);
					}
					
					if (messageText.StartsWith("/gpt"))
						if (GPTmode)
						{
							GPTmode = false;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo GPT desactivado.");
							return;
						}
						else
						{
							GPTmode = true;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo GPT activado.");
							return;
						}
					else if (GPTmode)
					{
						string output = await GPT(messageText);
						await Globals.BOT.SendTextMessageAsync(message.Chat.Id, output == string.Empty ? "No hay salida" : output);
					}

					if (messageText.StartsWith("/imggpt"))
						if (GPTimgmode)
						{
							GPTimgmode = false;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo GPT IMG desactivado.");
							return;
						}
						else
						{
							GPTimgmode = true;
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Modo GPT IMG activado.");
							return;
						}
					//int.Parse(messageText.Split(";")[1])
					if (GPTimgmode)
					{
						foreach (Image im in await CreateImgGPT(messageText.Split(";")[0], messageText.Contains(";")?int.Parse(messageText.Split(";")[1]):1))
						{
							using (MemoryStream stream = new MemoryStream())
							{
								im.Save(stream, ImageFormat.Png); // Cambiar si el formato de la imagen es diferente
								stream.Position = 0; // Resetear la posición del stream antes de utilizarlo
								Telegram.Bot.Types.Message message1 = await Globals.BOT.SendPhotoAsync(message.Chat.Id, new InputFileStream(stream));
							}
						}
						return;
					}

					string comandos = string.Concat(
								"/start --> Publicar", Environment.NewLine,
								"/stop  --> Dejar de Publicar", Environment.NewLine,
								"/cmd   --> Iniciar modo consola de windows", Environment.NewLine,
								"/gpt	--> Iniciar Chat", Environment.NewLine,
								"/imggpt  --> Generar Imagen", Environment.NewLine,
								"/shutdown  --> Apagar Dispositivo", Environment.NewLine,
								"/comandos  --> Mostar Menu", Environment.NewLine,
								"/exit  --> Salir de la aplicación", Environment.NewLine
								);

					switch (messageText)
					{
						case string s when s.StartsWith("/start"):
							if (BTNiniciar.Text == "Iniciar")
								Inicio();
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Se comenzó a publicar.");
							break;
						case string s when s.StartsWith("/stop"):
							if (BTNiniciar.Text == "Detener")
								Inicio();
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Se detuvo la publicación.");
							break;
						case "/exit":
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Cerrando app..");
							this.Dispose();
							break;
						case "/pforce":
							ProximaPublicacion_En = DateTime.Now;
							break;
						case "/comandos":
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id, comandos);
							break;
						case "/shutdown":
							await Globals.BOT.SendTextMessageAsync(message.Chat.Id,  "Apagando dispositivo..");
							Globals.EjecutarComando("shutdown -s -f -t 3");
							break;
						default:
							if (!ConsoleMode && !GPTmode && !GPTimgmode)
								await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Comando desconocido." + Environment.NewLine + Environment.NewLine + comandos);
							break;
					}
					//await bot.SendTextMessageAsync(message.Chat.Id, "Mensaje recibido: " + message.Text, cancellationToken: cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}



		





















	}
}






//// Obtener el elemento que tiene el foco
//var activeElement = Driver.SwitchTo().ActiveElement();

//// Obtener todos los textos dentro del elemento con foco
//var allText = activeElement.Text;



//this.Invoke(() => {
//	Thread.Sleep(2000);
//	Thread.Sleep(500);
//});



//this.Visible = false;
//new FaceAutomation2.Configuraciones().Show();
//Configuraciones F = new Configuraciones();
//F.ShowDialog();


//if (messageText.StartsWith("/start"))
//{
//	if (BTNiniciar.Text == "Iniciar")
//		Inicio();
//	await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Se comenzó a publicar.");
//}
//else if (messageText.StartsWith("/stop"))
//{
//	if (BTNiniciar.Text == "Detener")
//		Inicio();
//	await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Se detuvo la publicación.");
//}
//else if (messageText.StartsWith("/exit"))
//{
//	await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Cerrando app..");
//	this.Dispose();
//	Environment.Exit(0);
//}
//else
//{
//	await Globals.BOT.SendTextMessageAsync(message.Chat.Id, "Comando desconocido.");
//}
