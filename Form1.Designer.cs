namespace FaceAutomation2
{
	partial class Consola
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Consola));
			this.btnConfig = new System.Windows.Forms.Button();
			this.BTNiniciar = new System.Windows.Forms.Button();
			this.pbProceso = new System.Windows.Forms.ProgressBar();
			this.LBprincipal = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnConfig
			// 
			this.btnConfig.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnConfig.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.btnConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnConfig.Image")));
			this.btnConfig.Location = new System.Drawing.Point(392, 11);
			this.btnConfig.Name = "btnConfig";
			this.btnConfig.Size = new System.Drawing.Size(31, 26);
			this.btnConfig.TabIndex = 1006;
			this.btnConfig.UseVisualStyleBackColor = false;
			this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
			// 
			// BTNiniciar
			// 
			this.BTNiniciar.BackColor = System.Drawing.Color.DodgerBlue;
			this.BTNiniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BTNiniciar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.BTNiniciar.ForeColor = System.Drawing.Color.White;
			this.BTNiniciar.Location = new System.Drawing.Point(3, 8);
			this.BTNiniciar.Name = "BTNiniciar";
			this.BTNiniciar.Size = new System.Drawing.Size(77, 34);
			this.BTNiniciar.TabIndex = 1004;
			this.BTNiniciar.Text = "Iniciar";
			this.BTNiniciar.UseVisualStyleBackColor = false;
			this.BTNiniciar.Click += new System.EventHandler(this.BTNiniciar_Click);
			// 
			// pbProceso
			// 
			this.pbProceso.Cursor = System.Windows.Forms.Cursors.AppStarting;
			this.pbProceso.ForeColor = System.Drawing.Color.Black;
			this.pbProceso.Location = new System.Drawing.Point(-1, -4);
			this.pbProceso.Name = "pbProceso";
			this.pbProceso.Size = new System.Drawing.Size(426, 10);
			this.pbProceso.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbProceso.TabIndex = 1011;
			// 
			// LBprincipal
			// 
			this.LBprincipal.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.LBprincipal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.LBprincipal.FormattingEnabled = true;
			this.LBprincipal.ItemHeight = 15;
			this.LBprincipal.Location = new System.Drawing.Point(86, 8);
			this.LBprincipal.Name = "LBprincipal";
			this.LBprincipal.Size = new System.Drawing.Size(305, 34);
			this.LBprincipal.TabIndex = 1012;
			// 
			// Consola
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.ClientSize = new System.Drawing.Size(424, 44);
			this.Controls.Add(this.LBprincipal);
			this.Controls.Add(this.pbProceso);
			this.Controls.Add(this.btnConfig);
			this.Controls.Add(this.BTNiniciar);
			this.Enabled = false;
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Consola";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FaceAutomation";
			this.TopMost = true;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Consola_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Shown += new System.EventHandler(this.Consola_Shown);
			this.ResumeLayout(false);

		}

		#endregion
		internal Button btnConfig;
		internal Button BTNiniciar;
		private ProgressBar pbProceso;
		private ListBox LBprincipal;
	}
}