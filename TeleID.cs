using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAutomation2
{
	internal class TeleID
	{
		public string ID { get; set; }
		public string ID_CHAT { get; set; }
		public string Habilitado { get; set; }

		public TeleID() { }

		public TeleID(string id, string ID_Chat, string habilitado ="SI") { 
			this.ID = id;
			this.ID_CHAT = ID_Chat;
			this.Habilitado = habilitado;
		}
	}
}
