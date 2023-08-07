
namespace FaceAutomation2
{
	public partial class Publicaciones : Form
	{
		public Publicaciones()
		{
			InitializeComponent();
		}

		private List<Automovil> vList = new();

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Publicaciones_Load(object sender, EventArgs e)
		{
			try
			{
				AgregarTooltips();
				CargarComboID();
				CBid.SelectedIndex = 0; CBid.Select();
				CBtipo.SelectedIndex = 0;
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
				ToolTip1.SetToolTip(this.LId, "");
				ToolTip1.SetToolTip(this.LTitulo, "*Opcional, se puede agregar un titulo corto a la publicacion.");
				ToolTip1.SetToolTip(this.LTipo, "");
				ToolTip1.SetToolTip(this.LAño, "");
				ToolTip1.SetToolTip(this.LMarca, "");
				ToolTip1.SetToolTip(this.LModelo, "");
				ToolTip1.SetToolTip(this.LKm, "Kilometros que tiene el vehiculo, no puede ser un valor menor a 300.");
				ToolTip1.SetToolTip(this.LPrecio, "");
				ToolTip1.SetToolTip(this.LCarroceria, "");
				ToolTip1.SetToolTip(this.LColorExterior, "");
				ToolTip1.SetToolTip(this.LColorInterior, "");
				ToolTip1.SetToolTip(this.LEstado, "");
				ToolTip1.SetToolTip(this.LCombustible, "");
				ToolTip1.SetToolTip(this.LTransmision, "");
				ToolTip1.SetToolTip(this.LDescripcion, "*Opcional, se puede ingresar una descripcion al final de la publicacion.");

				ToolTip1.SetToolTip(this.BTNDuplicar, "Posibilidad de duplicar una poublicacion guardada, se copian los datos e imagenes.");
				ToolTip1.SetToolTip(this.BTNBorrar, "Al borrar la publicacion se borraran las imagenes tambien.");
				ToolTip1.SetToolTip(this.BTNverimágenes, "Abrir la carpeta contenedora de las imagenes correspondientes a la publicacion que se esta visualizando." +
														  "*Opcionalmente puede agregar la palabra clave: 'ED1' en el nombre de alguna imagen para una edicion leve," +
														  " debe usarse para las imagenes con texto.");
				ToolTip1.SetToolTip(this.BTNimagenes, "Seleccionar las imagenes a agregar a la publicacion que se esta visualizando.");
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void CargarComboID()
		{
			try
			{
				CBid.Items.Clear();
				CBid.Items.Add("NUEVO");
				vList = BDQuery.GetAutomoviles();
				if (vList.Count == 0) { return; }
				foreach (Automovil V in vList) CBid.Items.Add(V.ID.ToString());
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNguardar_Click(object sender, EventArgs e)
		{
			try
			{
				if (!CamposCompletos()) return;
				if (CBid.SelectedItem.ToString().Equals("NUEVO")) //si es nuevo, lo genero
				{
					if (!BDQuery.AddNewAutomovil(
												TXTtitulo.Text,
												CBtipo.SelectedItem.ToString().Trim(),
												CBaño.SelectedItem.ToString().Trim(),
												TXTmarca.Text,
												TXTmodelo.Text,
												TXTkm.Text.Trim(),
												TXTprecio.Text.Trim(),
												CBcarroceria.SelectedItem.ToString(),
												CBcolorExterior.SelectedItem.ToString(),
												CBcolorInterior.SelectedItem.ToString(),
												CBestado.SelectedItem.ToString(),
												CBcombustible.SelectedItem.ToString(),
												CBtransmision.SelectedItem.ToString(),
												TXTdescription.Text)) MessageBox.Show("Error al intentar cargar el vehiculo.");
					CargarComboID();
					CBid.SelectedIndex = CBid.Items.Count - 1;
					BTNguardar.ForeColor = Color.White;
				}
				else //si no es nuevo, actualizo
				{
					if (!BDQuery.UpdateAutomovil(
												CBid.SelectedItem.ToString(),
												TXTtitulo.Text,
												CBtipo.SelectedItem.ToString().Trim(),
												CBaño.SelectedItem.ToString().Trim(),
												TXTmarca.Text,
												TXTmodelo.Text,
												TXTkm.Text.Trim(),
												TXTprecio.Text.Trim(),
												CBcarroceria.SelectedItem.ToString(),
												CBcolorExterior.SelectedItem.ToString(),
												CBcolorInterior.SelectedItem.ToString(),
												CBestado.SelectedItem.ToString(),
												CBcombustible.SelectedItem.ToString(),
												CBtransmision.SelectedItem.ToString(),
												TXTdescription.Text)) MessageBox.Show("Error al intentar actualizar el vehiculo.");
					BTNguardar.ForeColor = Color.White;
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void CBid_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (CBid.SelectedItem == null) return;
				if (CBid.SelectedItem.ToString().Equals("NUEVO"))
				{
					TXTtitulo.Text = "";
					TXTmarca.Text = "";
					TXTmodelo.Text = "";
					TXTkm.Text = "300";
					TXTprecio.Text = "";
					TXTdescription.Text = "";
					CBtipo.SelectedItem = CBtipo.Items.IndexOf("Auto");
					CBaño.SelectedItem = null;
					CBcarroceria.SelectedItem = null;
					CBcolorExterior.SelectedItem = null;
					CBcolorInterior.SelectedItem = null;
					CBestado.SelectedItem = null;
					CBcombustible.SelectedItem = null;
					CBtransmision.SelectedItem = null;
					BTNguardar.ForeColor = Color.Yellow;
					BTNBorrar.Visible = false;
				}
				else
				{
					Automovil Auto = BDQuery.GetAutomoviles().Find(x => x.ID.ToString() == CBid.SelectedItem.ToString());
					TXTtitulo.Text = Auto.Titulo;
					TXTmarca.Text = Auto.Marca;
					TXTmodelo.Text = Auto.Modelo;
					TXTkm.Text = Auto.KM;
					TXTprecio.Text = Auto.Precio;
					TXTdescription.Text = Auto.Descripcion;
					CBtipo.SelectedIndex = CBtipo.Items.IndexOf(Auto.Tipo);
					CBaño.SelectedIndex = CBaño.Items.IndexOf(Auto.Año);
					CBcarroceria.SelectedIndex = CBcarroceria.Items.IndexOf(Auto.Carroceria);
					CBcolorExterior.SelectedIndex = CBcolorExterior.Items.IndexOf(Auto.ColorExterior);
					CBcolorInterior.SelectedIndex = CBcolorInterior.Items.IndexOf(Auto.ColorInterior);
					CBestado.SelectedIndex = CBestado.Items.IndexOf(Auto.Estado);
					CBcombustible.SelectedIndex = CBcombustible.Items.IndexOf(Auto.Combustible);
					CBtransmision.SelectedIndex = CBtransmision.Items.IndexOf(Auto.Transmision);
					BTNguardar.ForeColor = Color.White;
					BTNBorrar.Visible = true;
				}

				ComprobarBTNImagenes();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void ComprobarBTNImagenes()
		{
			try
			{
				if (!Globals.hayImagenes(CBid.SelectedItem.ToString()))
				{
					BTNimagenes.ForeColor = Color.Yellow;
					BTNverimágenes.Visible = false;
				}
				else
				{
					BTNimagenes.ForeColor = Color.White;
					BTNverimágenes.Visible = true;
				}

				if (CBid.SelectedItem.ToString().Equals("NUEVO"))
				{
					BTNimagenes.Visible = false;
					BTNBorrar.Visible = false;
				}
				else
				{
					BTNimagenes.Visible = true;
					BTNBorrar.Visible = true;
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private bool CamposCompletos()
		{
			try
			{
				if (Convert.ToInt32(TXTkm.Text) < 300) TXTkm.Text = "300";
				if (TXTprecio.Text.Length == 0) TXTprecio.Text = "0";

				if (TXTmarca.Text.Length == 0 |
				   TXTmodelo.Text.Length == 0 |
				   TXTkm.Text.Length == 0 |
				   Convert.ToInt32(TXTkm.Text.Trim()) < 300 |
				   TXTprecio.Text.Length == 0 |
				   CBtipo.SelectedItem == null |
				   CBaño.SelectedItem == null |
				   CBcarroceria.SelectedItem == null |
				   CBcolorExterior.SelectedItem == null |
				   CBcolorInterior.SelectedItem == null |
				   CBestado.SelectedItem == null |
				   CBcombustible.SelectedItem == null |
				   CBtransmision.SelectedItem == null)
				{
					MessageBox.Show("Revise los campos.", "Campos invalidos/incompletos");
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		private void TXTtitulo_TextChanged(object sender, EventArgs e) { BTNguardar.ForeColor = Color.Yellow; }

		private void CBtipo_SelectedIndexChanged(object sender, EventArgs e) { BTNguardar.ForeColor = Color.Yellow; }

		private void BTNDuplicar_Click(object sender, EventArgs e)
		{
			try
			{
				if (!CamposCompletos()) return;
				if (!BDQuery.AddNewAutomovil(
												TXTtitulo.Text,
												CBtipo.SelectedItem.ToString().Trim(),
												CBaño.SelectedItem.ToString().Trim(),
												TXTmarca.Text,
												TXTmodelo.Text,
												TXTkm.Text.Trim(),
												TXTprecio.Text.Trim(),
												CBcarroceria.SelectedItem.ToString(),
												CBcolorExterior.SelectedItem.ToString(),
												CBcolorInterior.SelectedItem.ToString(),
												CBestado.SelectedItem.ToString(),
												CBcombustible.SelectedItem.ToString(),
												CBtransmision.SelectedItem.ToString(),
												TXTdescription.Text)) MessageBox.Show("Error al intentar duplicar el vehiculo.");

				string dirAcopiar = string.Concat(Globals.IMFolderGeneralAutos, CBid.SelectedItem.ToString(), "\\");
				CargarComboID();
				CBid.SelectedIndex = CBid.Items.Count - 1;
				BTNguardar.ForeColor = Color.White;
				string dirNuevo = string.Concat(Globals.IMFolderGeneralAutos, CBid.SelectedItem.ToString(), "\\");

				if (Directory.Exists(dirAcopiar))
					Globals.CopyFilesRecursively(dirAcopiar, dirNuevo);
				ComprobarBTNImagenes();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNguardar_ForeColorChanged(object sender, EventArgs e)
		{
			if (BTNguardar.ForeColor == Color.White) BTNDuplicar.Visible = true; else BTNDuplicar.Visible = false;
		}

		private void BTNBorrar_Click(object sender, EventArgs e)
		{
			try
			{
				if (BDQuery.RemoveLOG("Automovil", CBid.SelectedItem.ToString()))
				{
					if (Directory.Exists(string.Concat(Globals.IMFolderGeneralAutos, CBid.SelectedItem.ToString(), "\\")))
						Directory.Delete(String.Concat(Globals.IMFolderGeneralAutos, CBid.SelectedItem.ToString(), "\\"), true);
					CargarComboID();
					if (CBid.Items.Count > 1) CBid.SelectedIndex = 1; else CBid.SelectedIndex = 0;
				}
				else
				{
					MessageBox.Show("Error al intentar eliminar el vehiculo.");
					return;
				}
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void VerImagenes(string ID)
		{
			try
			{
				string CarpetaImagenPub = Globals.IMFolder_Originales.Replace("xxx", ID);
				if (!Directory.Exists(CarpetaImagenPub)) return;
				System.Diagnostics.Process.Start("explorer.exe", CarpetaImagenPub);
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNverimágenes_Click(object sender, EventArgs e)
		{
			try
			{
				VerImagenes(CBid.SelectedItem.ToString());
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void BTNimagenes_Click(object sender, EventArgs e)
		{
			try
			{
				string CarpetaImagenPub = Globals.IMFolder_Originales.Replace("xxx", CBid.SelectedItem.ToString());
				DirectoryInfo Folder_IMAGENES = Directory.CreateDirectory(CarpetaImagenPub);
				OpenFileDialog openFileDialog1 = new()
				{
					InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
					Filter = "Imagenes |*.bmp;*.jpg;*.jpeg;*.png",
					Multiselect = true
				};

				if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					foreach (var FN in openFileDialog1.FileNames)
					{
						File.Copy(FN, Path.Combine(CarpetaImagenPub, string.Concat(Globals.Rando.Next(1234).ToString(), Path.GetExtension(FN))));
					}
					BTNimagenes.ForeColor = Color.White;
				};

				if (Folder_IMAGENES.GetFiles().Count() >= 20)
				{
					MessageBox.Show(string.Concat("Hay actualmente ", Folder_IMAGENES.GetFiles().Count().ToString(),
						" imágenes en la publicación, debe borrar algunas.",
						Environment.NewLine,
						Environment.NewLine,
						"Máximo: 20 imágenes.",
						Environment.NewLine,
						"Recomendado: 10 imágenes."),
						"Límite máximo de imágenes alcanzado.");
				}

				ComprobarBTNImagenes();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
	}
}











