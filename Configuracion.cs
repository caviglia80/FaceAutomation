
namespace FaceAutomation2
{
	internal class Configuracion
	{
		public string UltimaUbicacion { get; set; }
		public string UltimaPublicacion { get; set; }
		public int PublicacionesTotalHistorico { get; set; }
		public int PublicacionesMensuales { get; set; }
		public int PublicacionesDiarias { get; set; }
		public DateTime DateUltimaPublicacion { get; set; }
		public bool IM_Voltear { get; set; }
		public bool IM_Inclinar { get; set; }
		public bool IM_Mover { get; set; }
		public bool IM_Recortar { get; set; }
		public bool IM_AgregarMarcos { get; set; }
		public bool IM_EditarMetadatos { get; set; }
		public bool IM_Redimensionar { get; set; }
		public bool IM_AgregarIconos { get; set; }
		public bool IM_AgregarMarcasDeAgua { get; set; }
		public bool IM_AgregarFiltros { get; set; }

		public Configuracion() { }

		public Configuracion(
			string UltimaUbicacion,
			string UltimaPublicacion,
			int PublicacionesTotalHistorico,
			int PublicacionesMensuales,
			int PublicacionesDiarias,
			DateTime DateUltimaPublicacion,
			bool IM_Voltear,
			bool IM_Inclinar,
			bool IM_Mover,
			bool IM_Recortar,
			bool IM_AgregarMarcos,
			bool IM_EditarMetadatos,
			bool IM_Redimensionar,
			bool IM_AgregarIconos,
			bool IM_AgregarMarcasDeAgua,
			bool IM_AgregarFiltros)
		{
			this.UltimaUbicacion = UltimaUbicacion;
			this.UltimaPublicacion = UltimaPublicacion;
			this.PublicacionesTotalHistorico = PublicacionesTotalHistorico;
			this.PublicacionesMensuales = PublicacionesMensuales;
			this.PublicacionesDiarias = PublicacionesDiarias;
			this.DateUltimaPublicacion = DateUltimaPublicacion;
			this.IM_Voltear = IM_Voltear;
			this.IM_Inclinar = IM_Inclinar;
			this.IM_Mover = IM_Mover;
			this.IM_Recortar = IM_Recortar;
			this.IM_AgregarMarcos = IM_AgregarMarcos;
			this.IM_EditarMetadatos = IM_EditarMetadatos;
			this.IM_Redimensionar = IM_Redimensionar;
			this.IM_AgregarIconos = IM_AgregarIconos;
			this.IM_AgregarMarcasDeAgua = IM_AgregarMarcasDeAgua;
			this.IM_AgregarFiltros = IM_AgregarFiltros;
		}

	}
}
