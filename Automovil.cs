
namespace FaceAutomation2
{
	internal class Automovil
	{
		public int ID { get; set; }
		public string Titulo { get; set; }
		public string Tipo { get; set; }
		public string Año { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public string KM { get; set; }
		public string Precio { get; set; }
		public string Carroceria { get; set; }
		public string ColorExterior { get; set; }
		public string ColorInterior { get; set; }
		public string Estado { get; set; }
		public string Combustible { get; set; }
		public string Transmision { get; set; }
		public string Descripcion { get; set; }

		public Automovil() { }

		public Automovil(
						int	ID,
						string Titulo,
						string Tipo,
						string Año,
						string Marca,
						string Modelo,
						string KM,
						string Precio,
						string Carroceria,
						string ColorExterior,
						string ColorInterior,
						string Estado,
						string Combustible,
						string Transmision,
						string Descripcion
			)
		{
			this.ID = ID;
			this.Titulo = Titulo;
			this.Tipo = Tipo;
			this.Año = Año;
			this.Marca = Marca;
			this.Modelo = Modelo;
			this.KM = KM;
			this.Precio = Precio;
			this.Carroceria = Carroceria;
			this.ColorExterior = ColorExterior;
			this.ColorInterior = ColorInterior;
			this.Estado = Estado;
			this.Combustible = Combustible;
			this.Transmision = Transmision;
			this.Descripcion = Descripcion;
		}
	}
}
