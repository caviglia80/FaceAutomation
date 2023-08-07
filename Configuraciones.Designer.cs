namespace FaceAutomation2
{
	partial class Configuraciones
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuraciones));
			LVersion = new Label();
			GBops = new GroupBox();
			btnError = new Button();
			BTNrestaurar = new Button();
			BTNgestionPublicaciones = new Button();
			BTNUbicaciones = new Button();
			GBfb = new GroupBox();
			LFestado = new Label();
			CbEsperaUsuario = new ComboBox();
			LVfb = new ListView();
			ColumnHeader2 = new ColumnHeader();
			Label5 = new Label();
			Label7 = new Label();
			TXTpssw = new TextBox();
			TXTuser = new TextBox();
			BTNquitarFB = new Button();
			BTNagregarFB = new Button();
			GBimg = new GroupBox();
			cbIA = new CheckBox();
			Limagentitle = new Label();
			CLBImagenes = new CheckedListBox();
			GBtelegram = new GroupBox();
			LVtelegram = new ListView();
			ColumnHeader1 = new ColumnHeader();
			Label3 = new Label();
			Label2 = new Label();
			TXTidtelegram = new TextBox();
			TXTusuariotelegram = new TextBox();
			BTNquitarTL = new Button();
			BTNagregarTL = new Button();
			BTNvolver = new Button();
			TLoader = new System.Windows.Forms.Timer(components);
			GBops.SuspendLayout();
			GBfb.SuspendLayout();
			GBimg.SuspendLayout();
			GBtelegram.SuspendLayout();
			SuspendLayout();
			// 
			// LVersion
			// 
			LVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			LVersion.AutoSize = true;
			LVersion.Location = new Point(625, 365);
			LVersion.Name = "LVersion";
			LVersion.Size = new Size(0, 15);
			LVersion.TabIndex = 1009;
			// 
			// GBops
			// 
			GBops.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			GBops.BackColor = Color.Transparent;
			GBops.Controls.Add(btnError);
			GBops.Controls.Add(BTNrestaurar);
			GBops.Controls.Add(BTNgestionPublicaciones);
			GBops.Controls.Add(BTNUbicaciones);
			GBops.ForeColor = Color.Black;
			GBops.Location = new Point(368, 183);
			GBops.Name = "GBops";
			GBops.Size = new Size(350, 165);
			GBops.TabIndex = 1008;
			GBops.TabStop = false;
			// 
			// btnError
			// 
			btnError.BackColor = Color.DodgerBlue;
			btnError.FlatStyle = FlatStyle.Flat;
			btnError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			btnError.ForeColor = Color.White;
			btnError.Location = new Point(6, 124);
			btnError.Name = "btnError";
			btnError.Size = new Size(336, 28);
			btnError.TabIndex = 1007;
			btnError.Text = "Enviar Informe de Errores";
			btnError.UseVisualStyleBackColor = false;
			btnError.Click += btnLicencias_Click;
			// 
			// BTNrestaurar
			// 
			BTNrestaurar.BackColor = Color.DodgerBlue;
			BTNrestaurar.FlatStyle = FlatStyle.Flat;
			BTNrestaurar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNrestaurar.ForeColor = Color.White;
			BTNrestaurar.Location = new Point(6, 90);
			BTNrestaurar.Name = "BTNrestaurar";
			BTNrestaurar.Size = new Size(336, 28);
			BTNrestaurar.TabIndex = 1005;
			BTNrestaurar.Text = "Restaurar Configuraciones ";
			BTNrestaurar.UseVisualStyleBackColor = false;
			BTNrestaurar.Click += BTNrestaurar_Click;
			// 
			// BTNgestionPublicaciones
			// 
			BTNgestionPublicaciones.BackColor = Color.DodgerBlue;
			BTNgestionPublicaciones.FlatStyle = FlatStyle.Flat;
			BTNgestionPublicaciones.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNgestionPublicaciones.ForeColor = Color.White;
			BTNgestionPublicaciones.Location = new Point(6, 56);
			BTNgestionPublicaciones.Name = "BTNgestionPublicaciones";
			BTNgestionPublicaciones.Size = new Size(336, 28);
			BTNgestionPublicaciones.TabIndex = 1;
			BTNgestionPublicaciones.Text = "Gestión de Publicaciones";
			BTNgestionPublicaciones.UseVisualStyleBackColor = false;
			BTNgestionPublicaciones.Click += BTNgestionPublicaciones_Click;
			// 
			// BTNUbicaciones
			// 
			BTNUbicaciones.BackColor = Color.DodgerBlue;
			BTNUbicaciones.FlatStyle = FlatStyle.Flat;
			BTNUbicaciones.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNUbicaciones.ForeColor = Color.White;
			BTNUbicaciones.Location = new Point(6, 22);
			BTNUbicaciones.Name = "BTNUbicaciones";
			BTNUbicaciones.Size = new Size(336, 28);
			BTNUbicaciones.TabIndex = 0;
			BTNUbicaciones.Text = "Gestionar Ubicaciones";
			BTNUbicaciones.UseVisualStyleBackColor = false;
			BTNUbicaciones.Click += BTNUbicaciones_Click;
			// 
			// GBfb
			// 
			GBfb.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			GBfb.BackColor = Color.Transparent;
			GBfb.Controls.Add(LFestado);
			GBfb.Controls.Add(CbEsperaUsuario);
			GBfb.Controls.Add(LVfb);
			GBfb.Controls.Add(Label5);
			GBfb.Controls.Add(Label7);
			GBfb.Controls.Add(TXTpssw);
			GBfb.Controls.Add(TXTuser);
			GBfb.Controls.Add(BTNquitarFB);
			GBfb.Controls.Add(BTNagregarFB);
			GBfb.Location = new Point(12, 183);
			GBfb.Name = "GBfb";
			GBfb.Size = new Size(350, 165);
			GBfb.TabIndex = 1007;
			GBfb.TabStop = false;
			// 
			// LFestado
			// 
			LFestado.AutoSize = true;
			LFestado.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point);
			LFestado.Location = new Point(279, 45);
			LFestado.Name = "LFestado";
			LFestado.Size = new Size(0, 15);
			LFestado.TabIndex = 1006;
			// 
			// CbEsperaUsuario
			// 
			CbEsperaUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
			CbEsperaUsuario.Enabled = false;
			CbEsperaUsuario.FlatStyle = FlatStyle.Flat;
			CbEsperaUsuario.FormattingEnabled = true;
			CbEsperaUsuario.Items.AddRange(new object[] { "10        (minutos)", "15        (minutos)", "30        (minutos)", "60        (1 hora)", "120      (2 horas)", "180      (3 horas)", "240      (4 horas)", "300      (5 horas)" });
			CbEsperaUsuario.Location = new Point(196, 14);
			CbEsperaUsuario.Name = "CbEsperaUsuario";
			CbEsperaUsuario.Size = new Size(148, 23);
			CbEsperaUsuario.TabIndex = 1006;
			CbEsperaUsuario.SelectedIndexChanged += CbEspoeraUsuario_SelectedIndexChanged;
			// 
			// LVfb
			// 
			LVfb.BackColor = Color.Gray;
			LVfb.Columns.AddRange(new ColumnHeader[] { ColumnHeader2 });
			LVfb.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			LVfb.ForeColor = Color.Black;
			LVfb.FullRowSelect = true;
			LVfb.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			LVfb.ImeMode = ImeMode.NoControl;
			LVfb.Location = new Point(6, 14);
			LVfb.MultiSelect = false;
			LVfb.Name = "LVfb";
			LVfb.Size = new Size(184, 144);
			LVfb.TabIndex = 1005;
			LVfb.UseCompatibleStateImageBehavior = false;
			LVfb.View = View.Details;
			LVfb.ItemSelectionChanged += LVfb_ItemSelectionChanged;
			LVfb.DoubleClick += LVfb_DoubleClick;
			// 
			// ColumnHeader2
			// 
			ColumnHeader2.Text = "Facebook";
			ColumnHeader2.Width = 130;
			// 
			// Label5
			// 
			Label5.AutoSize = true;
			Label5.BackColor = Color.Transparent;
			Label5.ForeColor = Color.Black;
			Label5.Location = new Point(196, 85);
			Label5.Name = "Label5";
			Label5.Size = new Size(36, 15);
			Label5.TabIndex = 19;
			Label5.Text = "Clave";
			// 
			// Label7
			// 
			Label7.AutoSize = true;
			Label7.BackColor = Color.Transparent;
			Label7.ForeColor = Color.Black;
			Label7.Location = new Point(196, 45);
			Label7.Name = "Label7";
			Label7.Size = new Size(43, 15);
			Label7.TabIndex = 18;
			Label7.Text = "Correo";
			// 
			// TXTpssw
			// 
			TXTpssw.BackColor = Color.Gray;
			TXTpssw.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
			TXTpssw.Location = new Point(196, 101);
			TXTpssw.Name = "TXTpssw";
			TXTpssw.PasswordChar = '*';
			TXTpssw.Size = new Size(148, 22);
			TXTpssw.TabIndex = 1;
			TXTpssw.TextAlign = HorizontalAlignment.Center;
			// 
			// TXTuser
			// 
			TXTuser.BackColor = Color.Gray;
			TXTuser.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
			TXTuser.Location = new Point(196, 62);
			TXTuser.Name = "TXTuser";
			TXTuser.Size = new Size(148, 22);
			TXTuser.TabIndex = 0;
			TXTuser.TextAlign = HorizontalAlignment.Center;
			// 
			// BTNquitarFB
			// 
			BTNquitarFB.BackColor = Color.DodgerBlue;
			BTNquitarFB.FlatStyle = FlatStyle.Flat;
			BTNquitarFB.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNquitarFB.ForeColor = Color.White;
			BTNquitarFB.Location = new Point(279, 130);
			BTNquitarFB.Name = "BTNquitarFB";
			BTNquitarFB.Size = new Size(65, 28);
			BTNquitarFB.TabIndex = 3;
			BTNquitarFB.TabStop = false;
			BTNquitarFB.Text = "Quitar";
			BTNquitarFB.UseVisualStyleBackColor = false;
			BTNquitarFB.Click += BTNquitarFB_Click;
			// 
			// BTNagregarFB
			// 
			BTNagregarFB.BackColor = Color.DodgerBlue;
			BTNagregarFB.FlatStyle = FlatStyle.Flat;
			BTNagregarFB.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNagregarFB.ForeColor = Color.White;
			BTNagregarFB.Location = new Point(196, 130);
			BTNagregarFB.Name = "BTNagregarFB";
			BTNagregarFB.Size = new Size(77, 28);
			BTNagregarFB.TabIndex = 2;
			BTNagregarFB.Text = "Agregar";
			BTNagregarFB.UseVisualStyleBackColor = false;
			BTNagregarFB.Click += BTNagregarFB_Click;
			// 
			// GBimg
			// 
			GBimg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			GBimg.BackColor = Color.Transparent;
			GBimg.Controls.Add(cbIA);
			GBimg.Controls.Add(Limagentitle);
			GBimg.Controls.Add(CLBImagenes);
			GBimg.ForeColor = Color.Black;
			GBimg.Location = new Point(368, 12);
			GBimg.Name = "GBimg";
			GBimg.Size = new Size(350, 165);
			GBimg.TabIndex = 1006;
			GBimg.TabStop = false;
			// 
			// cbIA
			// 
			cbIA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			cbIA.AutoSize = true;
			cbIA.Location = new Point(139, 145);
			cbIA.Name = "cbIA";
			cbIA.Size = new Size(209, 19);
			cbIA.TabIndex = 1005;
			cbIA.Text = "Editar la descripcion con IA (BETA).";
			cbIA.UseVisualStyleBackColor = true;
			cbIA.CheckedChanged += cbIA_CheckedChanged;
			// 
			// Limagentitle
			// 
			Limagentitle.AutoSize = true;
			Limagentitle.BackColor = Color.Transparent;
			Limagentitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			Limagentitle.ForeColor = Color.Black;
			Limagentitle.Location = new Point(6, 14);
			Limagentitle.Name = "Limagentitle";
			Limagentitle.Size = new Size(108, 15);
			Limagentitle.TabIndex = 1004;
			Limagentitle.Text = "Edición de Imagen";
			// 
			// CLBImagenes
			// 
			CLBImagenes.BackColor = Color.Gray;
			CLBImagenes.BorderStyle = BorderStyle.FixedSingle;
			CLBImagenes.CheckOnClick = true;
			CLBImagenes.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			CLBImagenes.ForeColor = Color.Black;
			CLBImagenes.FormattingEnabled = true;
			CLBImagenes.Items.AddRange(new object[] { "Voltear Imagen", "Inclinar Imagen (recomendado)", "Mover Imagen (recomendado)", "Recortar Imagen (recomendado)", "Quitar Metadatos (recomendado)", "Editar Canal Alpha (recomendado)", "Editar Brillo (recomendado)", "Editar Contraste (recomendado)", "Redimensionar", "Editar Temperatura", "Agregar Ruido", "Pixelear", "Cambiar Colores", "Agregar Marcos", "Desenfoque (recomendado)", "Edicion General (recomendado)", "Ruido Gausiano (recomendado)" });
			CLBImagenes.Location = new Point(6, 31);
			CLBImagenes.Name = "CLBImagenes";
			CLBImagenes.Size = new Size(338, 110);
			CLBImagenes.TabIndex = 1003;
			CLBImagenes.ThreeDCheckBoxes = true;
			CLBImagenes.UseTabStops = false;
			CLBImagenes.SelectedIndexChanged += CLBImagenes_SelectedIndexChanged;
			// 
			// GBtelegram
			// 
			GBtelegram.BackColor = Color.Transparent;
			GBtelegram.Controls.Add(LVtelegram);
			GBtelegram.Controls.Add(Label3);
			GBtelegram.Controls.Add(Label2);
			GBtelegram.Controls.Add(TXTidtelegram);
			GBtelegram.Controls.Add(TXTusuariotelegram);
			GBtelegram.Controls.Add(BTNquitarTL);
			GBtelegram.Controls.Add(BTNagregarTL);
			GBtelegram.Location = new Point(12, 12);
			GBtelegram.Name = "GBtelegram";
			GBtelegram.Size = new Size(350, 165);
			GBtelegram.TabIndex = 1005;
			GBtelegram.TabStop = false;
			// 
			// LVtelegram
			// 
			LVtelegram.BackColor = Color.Gray;
			LVtelegram.Columns.AddRange(new ColumnHeader[] { ColumnHeader1 });
			LVtelegram.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			LVtelegram.ForeColor = Color.Black;
			LVtelegram.FullRowSelect = true;
			LVtelegram.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			LVtelegram.Location = new Point(6, 14);
			LVtelegram.MultiSelect = false;
			LVtelegram.Name = "LVtelegram";
			LVtelegram.Size = new Size(184, 144);
			LVtelegram.TabIndex = 1007;
			LVtelegram.UseCompatibleStateImageBehavior = false;
			LVtelegram.View = View.Details;
			LVtelegram.DoubleClick += LVtelegram_DoubleClick;
			// 
			// ColumnHeader1
			// 
			ColumnHeader1.Text = "Telegram";
			ColumnHeader1.Width = 130;
			// 
			// Label3
			// 
			Label3.AutoSize = true;
			Label3.BackColor = Color.Transparent;
			Label3.ForeColor = Color.Black;
			Label3.Location = new Point(196, 85);
			Label3.Name = "Label3";
			Label3.Size = new Size(62, 15);
			Label3.TabIndex = 19;
			Label3.Text = "ID de Chat";
			// 
			// Label2
			// 
			Label2.AutoSize = true;
			Label2.BackColor = Color.Transparent;
			Label2.ForeColor = Color.Black;
			Label2.Location = new Point(196, 45);
			Label2.Name = "Label2";
			Label2.Size = new Size(51, 15);
			Label2.TabIndex = 18;
			Label2.Text = "Nombre";
			// 
			// TXTidtelegram
			// 
			TXTidtelegram.BackColor = Color.Gray;
			TXTidtelegram.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
			TXTidtelegram.Location = new Point(196, 101);
			TXTidtelegram.Name = "TXTidtelegram";
			TXTidtelegram.Size = new Size(148, 22);
			TXTidtelegram.TabIndex = 1;
			TXTidtelegram.TextAlign = HorizontalAlignment.Center;
			// 
			// TXTusuariotelegram
			// 
			TXTusuariotelegram.BackColor = Color.Gray;
			TXTusuariotelegram.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
			TXTusuariotelegram.Location = new Point(196, 62);
			TXTusuariotelegram.Name = "TXTusuariotelegram";
			TXTusuariotelegram.Size = new Size(148, 22);
			TXTusuariotelegram.TabIndex = 0;
			TXTusuariotelegram.TextAlign = HorizontalAlignment.Center;
			// 
			// BTNquitarTL
			// 
			BTNquitarTL.BackColor = Color.DodgerBlue;
			BTNquitarTL.FlatStyle = FlatStyle.Flat;
			BTNquitarTL.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNquitarTL.ForeColor = Color.White;
			BTNquitarTL.Location = new Point(279, 130);
			BTNquitarTL.Name = "BTNquitarTL";
			BTNquitarTL.Size = new Size(65, 28);
			BTNquitarTL.TabIndex = 3;
			BTNquitarTL.TabStop = false;
			BTNquitarTL.Text = "Quitar";
			BTNquitarTL.UseVisualStyleBackColor = false;
			BTNquitarTL.Click += BTNquitarTL_Click;
			// 
			// BTNagregarTL
			// 
			BTNagregarTL.BackColor = Color.DodgerBlue;
			BTNagregarTL.FlatStyle = FlatStyle.Flat;
			BTNagregarTL.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNagregarTL.ForeColor = Color.White;
			BTNagregarTL.Location = new Point(196, 130);
			BTNagregarTL.Name = "BTNagregarTL";
			BTNagregarTL.Size = new Size(77, 28);
			BTNagregarTL.TabIndex = 2;
			BTNagregarTL.Text = "Agregar";
			BTNagregarTL.UseVisualStyleBackColor = false;
			BTNagregarTL.Click += BTNagregarTL_Click;
			// 
			// BTNvolver
			// 
			BTNvolver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			BTNvolver.BackColor = Color.DodgerBlue;
			BTNvolver.FlatStyle = FlatStyle.Flat;
			BTNvolver.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			BTNvolver.ForeColor = Color.White;
			BTNvolver.Location = new Point(12, 352);
			BTNvolver.Name = "BTNvolver";
			BTNvolver.Size = new Size(69, 28);
			BTNvolver.TabIndex = 1004;
			BTNvolver.Text = "Volver";
			BTNvolver.UseVisualStyleBackColor = false;
			BTNvolver.Click += BTNvolver_Click;
			// 
			// TLoader
			// 
			TLoader.Interval = 900;
			TLoader.Tick += TLoader_Tick;
			// 
			// Configuraciones
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.DeepSkyBlue;
			ClientSize = new Size(722, 383);
			Controls.Add(LVersion);
			Controls.Add(GBops);
			Controls.Add(GBfb);
			Controls.Add(GBimg);
			Controls.Add(GBtelegram);
			Controls.Add(BTNvolver);
			ForeColor = Color.Black;
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Configuraciones";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Configuraciones";
			Load += Configuraciones_Load;
			GBops.ResumeLayout(false);
			GBfb.ResumeLayout(false);
			GBfb.PerformLayout();
			GBimg.ResumeLayout(false);
			GBimg.PerformLayout();
			GBtelegram.ResumeLayout(false);
			GBtelegram.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		internal Label LVersion;
		internal GroupBox GBops;
		internal Button btnError;
		internal Button BTNrestaurar;
		internal Button BTNgestionPublicaciones;
		internal Button BTNUbicaciones;
		internal GroupBox GBfb;
		internal Label LFestado;
		internal ListView LVfb;
		internal ColumnHeader ColumnHeader2;
		internal Label Label5;
		internal Label Label7;
		internal TextBox TXTpssw;
		internal TextBox TXTuser;
		internal Button BTNquitarFB;
		internal Button BTNagregarFB;
		internal GroupBox GBimg;
		internal Label Limagentitle;
		public CheckedListBox CLBImagenes;
		internal GroupBox GBtelegram;
		internal ListView LVtelegram;
		internal ColumnHeader ColumnHeader1;
		internal Label Label3;
		internal Label Label2;
		internal TextBox TXTidtelegram;
		internal TextBox TXTusuariotelegram;
		internal Button BTNquitarTL;
		internal Button BTNagregarTL;
		internal Button BTNvolver;
		private System.Windows.Forms.Timer TLoader;
		private ComboBox CbEsperaUsuario;
		private CheckBox cbIA;
	}
}