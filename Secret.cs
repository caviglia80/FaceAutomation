
namespace FaceAutomation2
{
	internal class Secret
	{
		public int ID { get; set; }
		public string Usuario { get; set; }
		public string Clave { get; set; }
		public string Habilitado { get; set; }
		public string MinutosDeEspera { get; set; }
		public DateTime UltimaPublicacion { get; set; }
		public Secret() { }
		public Secret(int id, string usuario, string clave, string habilitado, string MinutosDeEspera, DateTime ultimaPublicacion)
		{
			this.ID = id;
			this.Usuario = usuario;
			this.Clave = clave;
			this.Habilitado = habilitado;
			this.MinutosDeEspera = MinutosDeEspera;
			this.UltimaPublicacion = ultimaPublicacion;
		}
	}
}
