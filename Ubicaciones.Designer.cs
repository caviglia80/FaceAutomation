namespace FaceAutomation2
{
	partial class Ubicaciones
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ubicaciones));
			this.lvLocations = new System.Windows.Forms.ListView();
			this.txtLoc = new System.Windows.Forms.TextBox();
			this.btnAgregar = new System.Windows.Forms.Button();
			this.btnBorrar = new System.Windows.Forms.Button();
			this.btnVolver = new System.Windows.Forms.Button();
			this.btnBorrarTodo = new System.Windows.Forms.Button();
			this.btnImportar = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lvLocations
			// 
			this.lvLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvLocations.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvLocations.FullRowSelect = true;
			this.lvLocations.Location = new System.Drawing.Point(12, 40);
			this.lvLocations.MultiSelect = false;
			this.lvLocations.Name = "lvLocations";
			this.lvLocations.Size = new System.Drawing.Size(291, 295);
			this.lvLocations.TabIndex = 0;
			this.lvLocations.UseCompatibleStateImageBehavior = false;
			this.lvLocations.View = System.Windows.Forms.View.List;
			// 
			// txtLoc
			// 
			this.txtLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLoc.BackColor = System.Drawing.Color.Gray;
			this.txtLoc.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.txtLoc.Location = new System.Drawing.Point(12, 12);
			this.txtLoc.Name = "txtLoc";
			this.txtLoc.Size = new System.Drawing.Size(221, 22);
			this.txtLoc.TabIndex = 1;
			this.txtLoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtLoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLoc_KeyPress);
			// 
			// btnAgregar
			// 
			this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAgregar.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnAgregar.ForeColor = System.Drawing.Color.White;
			this.btnAgregar.Location = new System.Drawing.Point(238, 10);
			this.btnAgregar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnAgregar.Name = "btnAgregar";
			this.btnAgregar.Size = new System.Drawing.Size(65, 26);
			this.btnAgregar.TabIndex = 17;
			this.btnAgregar.Text = "Agregar";
			this.btnAgregar.UseVisualStyleBackColor = false;
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			// 
			// btnBorrar
			// 
			this.btnBorrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnBorrar.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBorrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnBorrar.ForeColor = System.Drawing.Color.White;
			this.btnBorrar.Location = new System.Drawing.Point(12, 341);
			this.btnBorrar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnBorrar.Name = "btnBorrar";
			this.btnBorrar.Size = new System.Drawing.Size(92, 26);
			this.btnBorrar.TabIndex = 18;
			this.btnBorrar.Text = "Borrar";
			this.btnBorrar.UseVisualStyleBackColor = false;
			this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
			// 
			// btnVolver
			// 
			this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnVolver.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnVolver.ForeColor = System.Drawing.Color.White;
			this.btnVolver.Location = new System.Drawing.Point(12, 373);
			this.btnVolver.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnVolver.Name = "btnVolver";
			this.btnVolver.Size = new System.Drawing.Size(290, 26);
			this.btnVolver.TabIndex = 19;
			this.btnVolver.Text = "Volver";
			this.btnVolver.UseVisualStyleBackColor = false;
			this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
			// 
			// btnBorrarTodo
			// 
			this.btnBorrarTodo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnBorrarTodo.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnBorrarTodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBorrarTodo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnBorrarTodo.ForeColor = System.Drawing.Color.White;
			this.btnBorrarTodo.Location = new System.Drawing.Point(108, 341);
			this.btnBorrarTodo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnBorrarTodo.Name = "btnBorrarTodo";
			this.btnBorrarTodo.Size = new System.Drawing.Size(98, 26);
			this.btnBorrarTodo.TabIndex = 20;
			this.btnBorrarTodo.Text = "Borrar Todo";
			this.btnBorrarTodo.UseVisualStyleBackColor = false;
			this.btnBorrarTodo.Click += new System.EventHandler(this.btnBorrarTodo_Click);
			// 
			// btnImportar
			// 
			this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImportar.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnImportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnImportar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.btnImportar.ForeColor = System.Drawing.Color.White;
			this.btnImportar.Location = new System.Drawing.Point(210, 341);
			this.btnImportar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnImportar.Name = "btnImportar";
			this.btnImportar.Size = new System.Drawing.Size(92, 26);
			this.btnImportar.TabIndex = 21;
			this.btnImportar.Text = "Importar";
			this.btnImportar.UseVisualStyleBackColor = false;
			this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
			// 
			// Ubicaciones
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.ClientSize = new System.Drawing.Size(313, 403);
			this.Controls.Add(this.btnImportar);
			this.Controls.Add(this.btnBorrarTodo);
			this.Controls.Add(this.btnVolver);
			this.Controls.Add(this.btnBorrar);
			this.Controls.Add(this.btnAgregar);
			this.Controls.Add(this.txtLoc);
			this.Controls.Add(this.lvLocations);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Ubicaciones";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Administrar Ubicaciones";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.Ubicaciones_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ListView lvLocations;
		internal TextBox txtLoc;
		internal Button btnAgregar;
		internal Button btnBorrar;
		internal Button btnVolver;
		internal Button btnBorrarTodo;
		internal Button btnImportar;
	}
}