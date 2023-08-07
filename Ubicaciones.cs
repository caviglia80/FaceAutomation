
namespace FaceAutomation2
{
	public partial class Ubicaciones : Form
	{
		public Ubicaciones()
		{
			InitializeComponent();
		}
		private void btnVolver_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void Ubicaciones_Load(object sender, EventArgs e)
		{
			try
			{
				TraerLista();
				AgregarTooltips();
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
				ToolTip1.SetToolTip(this.btnImportar, "Puede seleccionar un bloc de notas con ubicaciones una debajo de otra para incluirla a la biblioteca.");

			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void TraerLista()
		{
			try
			{
				//lvLocations.Columns[0].Width = lvLocations.Width - 5;
				lvLocations.Items.Clear();
				foreach (var Loc in BDQuery.GetLocationNames())
					lvLocations.Items.Add(Loc);
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
		private void btnAgregar_Click(object sender, EventArgs e)
		{
			try
			{
				AgregarLoc();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
		private void btnBorrar_Click(object sender, EventArgs e)
		{
			try
			{
				if (lvLocations.SelectedItems.Count == 0) return;
				BDQuery.RemoveLocation(lvLocations.SelectedItems[0].Text);
				TraerLista();
				Application.DoEvents();
				if (lvLocations.Items.Count == 0) return;
				lvLocations.Items[0].Focused = true;
				lvLocations.Items[0].Selected = true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
		private void AgregarLoc()
		{
			try
			{
				if (txtLoc.Text.Length == 0) return;
				if (BDQuery.AddNewLocation(txtLoc.Text.Trim())) txtLoc.Text = "";
				TraerLista();
				txtLoc.Focus();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}
		private void txtLoc_KeyPress(object sender, KeyPressEventArgs e) { if ((int)e.KeyChar == (int)Keys.Enter) AgregarLoc(); }

		private void btnBorrarTodo_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show("Borrar todo?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
				if (lvLocations.Items.Count == 0) return;
				if(!BDQuery.RemoveAllLocations()) MessageBox.Show("Error al intentar eliminar todas las ubicaciones.");
				TraerLista();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private void btnImportar_Click(object sender, EventArgs e)
		{
			try
			{
				string files = Globals.ObtenerArchivo();
				if (files == "") return;
				foreach (var Loc in File.ReadAllLines(files))
					BDQuery.AddNewLocation(Loc, true);
				TraerLista();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}









	}
}
