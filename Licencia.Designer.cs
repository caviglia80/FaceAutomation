namespace FaceAutomation2
{
	partial class Licencia
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Licencia));
			this.BtnRegistrar = new System.Windows.Forms.Button();
			this.TxtKey = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BtnRegistrar
			// 
			this.BtnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnRegistrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.BtnRegistrar.ForeColor = System.Drawing.Color.White;
			this.BtnRegistrar.Location = new System.Drawing.Point(6, 29);
			this.BtnRegistrar.Name = "BtnRegistrar";
			this.BtnRegistrar.Size = new System.Drawing.Size(189, 29);
			this.BtnRegistrar.TabIndex = 6;
			this.BtnRegistrar.Text = "Registrar";
			this.BtnRegistrar.UseVisualStyleBackColor = true;
			this.BtnRegistrar.Click += new System.EventHandler(this.BtnRegistrar_Click);
			// 
			// TxtKey
			// 
			this.TxtKey.Font = new System.Drawing.Font("Sitka Small", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.TxtKey.Location = new System.Drawing.Point(6, 4);
			this.TxtKey.Name = "TxtKey";
			this.TxtKey.Size = new System.Drawing.Size(189, 23);
			this.TxtKey.TabIndex = 4;
			this.TxtKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Licencia
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.ClientSize = new System.Drawing.Size(200, 62);
			this.Controls.Add(this.BtnRegistrar);
			this.Controls.Add(this.TxtKey);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Licencia";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Licencia";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.Licencia_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		internal Button BtnRegistrar;
		internal TextBox TxtKey;
	}
}