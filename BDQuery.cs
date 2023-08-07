using Microsoft.Data.Sqlite;

namespace FaceAutomation2
{
	internal class BDQuery
	{
		private static string strConnection = string.Concat("Data Source=",  Globals.DB_File,  ";");

		public static bool RemoveUser(string User)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "DELETE FROM Secrets WHERE Usuario=@User;";
						cmd.Parameters.AddWithValue("@User",  User);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool AddNewUser(string User,  string Pssw)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "INSERT INTO Secrets (Usuario,  Clave,  UltimaPublicacionDate) VALUES (@User,  @Pssw,  @TIME);";
						cmd.Parameters.AddWithValue("@User",  User);
						cmd.Parameters.AddWithValue("@Pssw",  Pssw);
						cmd.Parameters.AddWithValue("@TIME", DateTime.MinValue.ToString());
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				if (e1.Message.Contains("UNIQUE")) { MessageBox.Show("El Usuario ya existe."); }
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool UpdateUser(string User, string Column, string Value)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("UPDATE Secrets SET '", Column, "'='", Value, "' WHERE Usuario=@Usuario;");
						cmd.Parameters.AddWithValue("@Usuario", User);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static Secret GetUser(string User)
		{
			try
			{
				Secret secret;
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "SELECT * FROM Secrets WHERE Usuario=@Usuario;";
						cmd.Parameters.AddWithValue("@Usuario", User);
						SqliteDataReader SQLiteReader = cmd.ExecuteReader();
						SQLiteReader.Read();

						secret = new Secret(
											Convert.ToInt32(SQLiteReader["ID"].ToString()),
											SQLiteReader["Usuario"].ToString(),
											SQLiteReader["Clave"].ToString(),
											SQLiteReader["Habilitado"].ToString(),
											SQLiteReader["MinutosDeEspera"].ToString(),
											Convert.ToDateTime(SQLiteReader["UltimaPublicacionDate"].ToString()
											));
					}
					conn.Close();
				}
				return secret;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return null;
			}
		}

		public static List<Secret> GetUserList()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<Secret> secretList = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Secrets;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
					{
						secretList.Add(new Secret(
													Convert.ToInt32(SQLiteReader["ID"].ToString()),
													SQLiteReader["Usuario"].ToString(),
													SQLiteReader["Clave"].ToString(),
													SQLiteReader["Habilitado"].ToString(),
													SQLiteReader["MinutosDeEspera"].ToString(),
													Convert.ToDateTime(SQLiteReader["UltimaPublicacionDate"].ToString()
													)));
					}
					SQLiteReader.Close();
				}
				conn.Close();
				return secretList;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static List<Secret> GetUserListNotBlocked()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<Secret> secretList = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Secrets WHERE Habilitado='SI' ORDER BY UltimaPublicacionDate ASC;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
					{
						secretList.Add(new Secret(
													Convert.ToInt32(SQLiteReader["ID"].ToString()),
													SQLiteReader["Usuario"].ToString(),
													SQLiteReader["Clave"].ToString(),
													SQLiteReader["Habilitado"].ToString(),
													SQLiteReader["MinutosDeEspera"].ToString(),
													Convert.ToDateTime(SQLiteReader["UltimaPublicacionDate"].ToString()
													)));
					}
					SQLiteReader.Close();
				}
				conn.Close();
				return secretList;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static Configuracion GetConfiguration()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				Configuracion Conf = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Config;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					SQLiteReader.Read();

					Conf = new Configuracion(
												SQLiteReader["UltimaUbicacion"].ToString(),
												Convert.ToString(SQLiteReader["UltimaPublicacion"]), 
												Convert.ToInt32(SQLiteReader["PublicacionesTotalHistorico"]), 
												Convert.ToInt32(SQLiteReader["PublicacionesMensuales"]), 
												Convert.ToInt32(SQLiteReader["PublicacionesDiarias"]), 
												Convert.ToDateTime(SQLiteReader["DateUltimaPublicacion"]), 
												bool.Parse(SQLiteReader["IM_Voltear"].ToString()), 
												bool.Parse(SQLiteReader["IM_Inclinar"].ToString()), 
												bool.Parse(SQLiteReader["IM_Mover"].ToString()), 
												bool.Parse(SQLiteReader["IM_Recortar"].ToString()), 
												bool.Parse(SQLiteReader["IM_AgregarMarcos"].ToString()), 
												bool.Parse(SQLiteReader["IM_EditarMetadatos"].ToString()), 
												bool.Parse(SQLiteReader["IM_Redimensionar"].ToString()), 
												bool.Parse(SQLiteReader["IM_AgregarIconos"].ToString()), 
												bool.Parse(SQLiteReader["IM_AgregarMarcasDeAgua"].ToString()), 
												bool.Parse(SQLiteReader["IM_AgregarFiltros"].ToString()));
					SQLiteReader.Close();
				}
				conn.Close();
				return Conf;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static bool UpdateConfiguration(string KEY, string VALUE)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("UPDATE Config SET ", KEY, "=@value;");
						cmd.Parameters.AddWithValue("@value", VALUE);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool RestartConfiguration()
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();

					//LIMPIO
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("DELETE FROM Config;");
						cmd.ExecuteNonQuery();
					}

					//CONFIG DE 0
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "INSERT INTO Config ('UltimaUbicacion') VALUES ('0');";
						cmd.ExecuteNonQuery();
					}

					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				MessageBox.Show(e1.Message);
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static string GetConfiguration(string KEY)
		{
			try
			{
				string configuration = "";
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("SELECT ", KEY, " FROM Config;");
						configuration = cmd.ExecuteScalar().ToString();
					}
					conn.Close();
				}
				return configuration;
			}
			catch (SqliteException e1)
			{
				MessageBox.Show(e1.Message);
				return "";
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return "";
			}
		}

		public static List<Automovil> GetAutomoviles()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<Automovil> vList = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Automovil;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
					{
						vList.Add(new Automovil(
								Convert.ToInt32(SQLiteReader["ID"]),
												SQLiteReader["Titulo"].ToString(),
												SQLiteReader["Tipo"].ToString(),
												SQLiteReader["Año"].ToString(),
												SQLiteReader["Marca"].ToString(),
												SQLiteReader["Modelo"].ToString(),
												SQLiteReader["KM"].ToString(),
												SQLiteReader["Precio"].ToString(),
												SQLiteReader["Carroceria"].ToString(),
												SQLiteReader["ColorExterior"].ToString(),
												SQLiteReader["ColorInterior"].ToString(),
												SQLiteReader["Estado"].ToString(),
												SQLiteReader["Combustible"].ToString(),
												SQLiteReader["Transmision"].ToString(),
												SQLiteReader["Descripcion"].ToString()
						));
					}
					SQLiteReader.Close();
				}
				conn.Close();
				return vList;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static bool AddNewAutomovil(
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
						string Descripcion) {
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("INSERT INTO Automovil (Titulo, Tipo, Año, Marca, Modelo, KM, Precio, Carroceria, ColorExterior, ColorInterior, Estado, Combustible, Transmision, Descripcion) VALUES (@Titulo, @Tipo, @Año, @Marca, @Modelo, @KM, @Precio, @Carroceria, @ColorExterior, @ColorInterior, @Estado, @Combustible, @Transmision, @Descripcion);");
						cmd.Parameters.AddWithValue("@Titulo",  Titulo);
						cmd.Parameters.AddWithValue("@Tipo",  Tipo);
						cmd.Parameters.AddWithValue("@Año",  Año);
						cmd.Parameters.AddWithValue("@Marca",  Marca);
						cmd.Parameters.AddWithValue("@Modelo",  Modelo);
						cmd.Parameters.AddWithValue("@KM",  KM);
						cmd.Parameters.AddWithValue("@Precio",  Precio);
						cmd.Parameters.AddWithValue("@Carroceria",  Carroceria);
						cmd.Parameters.AddWithValue("@ColorExterior",  ColorExterior);
						cmd.Parameters.AddWithValue("@ColorInterior",  ColorInterior);
						cmd.Parameters.AddWithValue("@Estado",  Estado);
						cmd.Parameters.AddWithValue("@Combustible",  Combustible);
						cmd.Parameters.AddWithValue("@Transmision",  Transmision);
						cmd.Parameters.AddWithValue("@Descripcion",  Descripcion);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool UpdateAutomovil(
						string ID,
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
						string Descripcion) {
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("UPDATE Automovil " +
														" SET Titulo=@Titulo, " +
														" Tipo=@Tipo, " +
														" Año=@Año, " +
														" Marca=@Marca, " +
														" Modelo=@Modelo, " +
														" KM=@KM, " +
														" Precio=@Precio, " +
														" Carroceria=@Carroceria, " +
														" ColorExterior=@ColorExterior, " +
														" ColorInterior=@ColorInterior, " +
														" Estado=@Estado, " +
														" Combustible=@Combustible, " +
														" Transmision=@Transmision, " +
														" Descripcion=@Descripcion " +
														" WHERE ID=@ID;");
						cmd.Parameters.AddWithValue("@ID", ID);
						cmd.Parameters.AddWithValue("@Titulo", Titulo);
						cmd.Parameters.AddWithValue("@Tipo", Tipo);
						cmd.Parameters.AddWithValue("@Año", Año);
						cmd.Parameters.AddWithValue("@Marca", Marca);
						cmd.Parameters.AddWithValue("@Modelo", Modelo);
						cmd.Parameters.AddWithValue("@KM", KM);
						cmd.Parameters.AddWithValue("@Precio", Precio);
						cmd.Parameters.AddWithValue("@Carroceria", Carroceria);
						cmd.Parameters.AddWithValue("@ColorExterior", ColorExterior);
						cmd.Parameters.AddWithValue("@ColorInterior", ColorInterior);
						cmd.Parameters.AddWithValue("@Estado", Estado);
						cmd.Parameters.AddWithValue("@Combustible", Combustible);
						cmd.Parameters.AddWithValue("@Transmision", Transmision);
						cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				MessageBox.Show(e1.Message);
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool RemoveLOG(string Tabla,string ID)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("DELETE FROM ", Tabla, " WHERE ID=@ID;");
						cmd.Parameters.AddWithValue("@ID", ID);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				MessageBox.Show(e1.Message);
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static Dictionary<int,string> GetLocations()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				Dictionary<int, string> Locations = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Location;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
						Locations.Add(Convert.ToInt32(SQLiteReader[0].ToString()), SQLiteReader[1].ToString());
					SQLiteReader.Close();
				}
				conn.Close();
				return Locations;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static List<string> GetLocationNames()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<string> Locations = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT Name FROM Location;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
						Locations.Add(SQLiteReader[0].ToString());
					SQLiteReader.Close();
				}
				conn.Close();
				return Locations;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static bool AddNewLocation(string Location, bool Silent = false)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "INSERT INTO Location (Name) VALUES (@Name);";
						cmd.Parameters.AddWithValue("@Name", Location);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				if(!Silent) if (e1.Message.Contains("UNIQUE")) { MessageBox.Show("La ubicacion ya existe."); }
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool RemoveLocation(string Location)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("DELETE FROM Location WHERE Name=@Name;");
						cmd.Parameters.AddWithValue("@Name", Location);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool RemoveAllLocations()
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("DELETE FROM Location;");
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static bool AddNewTelegram(string ID, string Chat)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "INSERT INTO Telegram (ID,  Chat) VALUES (@ID,  @Chat);";
						cmd.Parameters.AddWithValue("@ID", ID);
						cmd.Parameters.AddWithValue("@Chat", Chat);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (SqliteException e1)
			{
				if (e1.Message.Contains("UNIQUE")) { MessageBox.Show("El Telegram (ID o Chat) ya existe."); }
				return false;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static List<TeleID> GetTelegramList()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<TeleID> Lista = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Telegram;";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
					{
						Lista.Add(new TeleID(
													SQLiteReader["ID"].ToString(),
													SQLiteReader["Chat"].ToString(),
													SQLiteReader["Habilitado"].ToString()
													));
					}
					SQLiteReader.Close();
				}
				conn.Close();
				return Lista;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static List<TeleID> GetTelegramListNotBlocked()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();
				List<TeleID> Lista = new();

				using (var cmd = new SqliteCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT * FROM Telegram WHERE Habilitado='SI';";
					SqliteDataReader SQLiteReader = cmd.ExecuteReader();
					while (SQLiteReader.Read())
					{
						Lista.Add(new TeleID(
													SQLiteReader["ID"].ToString(),
													SQLiteReader["Chat"].ToString(),
													SQLiteReader["Habilitado"].ToString()
													));
					}
					SQLiteReader.Close();
				}
				conn.Close();
				return Lista;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return new();
			}
		}

		public static bool RemoveTelegram(string ID)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("DELETE FROM Telegram WHERE ID=@ID;");
						cmd.Parameters.AddWithValue("@ID", ID);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static TeleID GetTelegram(string ID)
		{
			try
			{
				TeleID Tele;
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "SELECT * FROM Telegram WHERE ID=@ID;";
						cmd.Parameters.AddWithValue("@ID", ID);
						SqliteDataReader SQLiteReader = cmd.ExecuteReader();
						SQLiteReader.Read();

						Tele = new TeleID(
											SQLiteReader["ID"].ToString(),
											SQLiteReader["Chat"].ToString(),
											SQLiteReader["Habilitado"].ToString()
											);
					}
					conn.Close();
				}
				return Tele;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return null;
			}
		}

		public static bool UpdateTelegram(string ID, string KEY, string VALUE)
		{
			try
			{
				using (var conn = new SqliteConnection(connectionString: strConnection))
				{
					conn.Open();
					using (var cmd = new SqliteCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = string.Concat("UPDATE Telegram SET ", KEY, "=@value WHERE ID=@ID;");
						cmd.Parameters.AddWithValue("@ID", ID);
						cmd.Parameters.AddWithValue("@value", VALUE);
						cmd.ExecuteNonQuery();
					}
					conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return false;
			}
		}

		public static void CrearTablasSiNoExisten()
		{
			try
			{
				using var conn = new SqliteConnection(connectionString: strConnection);
				conn.Open();

				List<string> CreateList = new()
				{
					"CREATE TABLE IF NOT EXISTS 'Automovil' (" +
					"'ID'	INTEGER UNIQUE," +
					"'Titulo'	TEXT DEFAULT ' '," +
					"'Tipo'	TEXT NOT NULL," +
					"'Año'	TEXT NOT NULL," +
					"'Marca'	TEXT NOT NULL," +
					"'Modelo'	TEXT NOT NULL," +
					"'KM'	TEXT NOT NULL," +
					"'Precio'	TEXT NOT NULL," +
					"'Carroceria'	TEXT NOT NULL," +
					"'ColorExterior'	TEXT NOT NULL," +
					"'ColorInterior'	TEXT NOT NULL," +
					"'Estado'	TEXT NOT NULL," +
					"'Combustible'	TEXT NOT NULL," +
					"'Transmision'	TEXT NOT NULL," +
					"'Descripcion'	TEXT," +
					"PRIMARY KEY('ID' AUTOINCREMENT)" +
					");",
					"CREATE TABLE IF NOT EXISTS 'Config' (" +
					"'UltimaUbicacion' TEXT DEFAULT 0," +
					"'UltimaPublicacion' TEXT DEFAULT 0," +
					"'PublicacionesTotalHistorico' TEXT DEFAULT 0," +
					"'PublicacionesMensuales' TEXT DEFAULT 0," +
					"'PublicacionesDiarias' TEXT DEFAULT 0," +
					"'DateUltimaPublicacion' TEXT DEFAULT '1/1/0001 00:00:00'," +
					"'Editar_Descripcion_IA' TEXT DEFAULT 'false'," +
					"'IM_Voltear' TEXT DEFAULT 'false'," +
					"'IM_Inclinar' TEXT DEFAULT 'true'," +
					"'IM_Mover' TEXT DEFAULT 'true'," +
					"'IM_Recortar' TEXT DEFAULT 'true'," +
					"'IM_QuitarMetadatos' TEXT DEFAULT 'true'," +
					"'IM_EditarAlpha' TEXT DEFAULT 'true'," +
					"'IM_EditarBrillo' TEXT DEFAULT 'true'," +
					"'IM_EditarContraste' TEXT DEFAULT 'true'," +
					"'IM_EditarTemperatura' TEXT DEFAULT 'false'," +
					"'IM_Pixelear' TEXT DEFAULT 'false'," +
					"'IM_Marcos' TEXT DEFAULT 'false'," +
					"'IM_Redimensionar' TEXT DEFAULT 'false'," +
					"'IM_Ruido' TEXT DEFAULT 'false'," +
					"'IM_CambiarColores' TEXT DEFAULT 'false'," +
					"'IM_Desenfoque' TEXT DEFAULT 'true'," +
					"'IM_EdicionGeneral' TEXT DEFAULT 'true'," +
					"'IM_RuidoGausiano' TEXT DEFAULT 'true'" +
					");",
					"CREATE TABLE IF NOT EXISTS 'Location' ( " +
					"'ID'	INTEGER NOT NULL DEFAULT 0 UNIQUE," +
					"'Name'	TEXT NOT NULL UNIQUE," +
					"'Tipo'	TEXT DEFAULT 'Vehiculo'," +
					"PRIMARY KEY('ID' AUTOINCREMENT)" +
					");",
					"CREATE TABLE IF NOT EXISTS 'Secrets' (" +
					"'ID'	INTEGER," +
					"'Usuario'	TEXT NOT NULL UNIQUE," +
					"'Clave'	TEXT NOT NULL," +
					"'Habilitado'	TEXT DEFAULT 'SI'," +
					"'MinutosDeEspera'	TEXT DEFAULT '15        (minutos)'," +
					"'UltimaPublicacionDate'	TEXT DEFAULT '1/1/0001 00:00:00'," +
					"PRIMARY KEY('ID' AUTOINCREMENT)" +
					");",
					"CREATE TABLE IF NOT EXISTS 'Telegram' (" +
					"'ID'	TEXT NOT NULL UNIQUE," +
					"'Chat'	TEXT NOT NULL UNIQUE," +
					"'Habilitado'	TEXT DEFAULT 'SI'" +
					");"
				};

				foreach (string Table in CreateList)
					using (SqliteCommand cmd = new(Table, conn))
						cmd.ExecuteNonQuery();

				//inicializo config
				Int32 countConfig = 0;
				using (SqliteCommand cmd = new("SELECT COUNT(*) FROM Config;", conn))
					countConfig = Convert.ToInt32(cmd.ExecuteScalar().ToString());
				if (countConfig == 0)
					RestartConfiguration();

				//inicializo locations
				Int32 countLoc = 0;
				using (SqliteCommand cmd = new("SELECT COUNT(1) FROM Location;", conn))
					countLoc = Convert.ToInt32(cmd.ExecuteScalar().ToString());
				if (countLoc == 0)
					using (SqliteCommand cmd = new("INSERT INTO Location (Name) VALUES ('Jujuy'), ('Saladillo'), ('Chascomus'), ('Calamuchita'), ('Merlo'), ('Pilar'), ('Santa teresita'), ('San clemente'), ('Mar chiquita');", conn))
						cmd.ExecuteNonQuery();

				List<string> NuevosCamposList = new()
				{
					//"ALTER TABLE 'Config' ADD COLUMN 'Editar_Descripcion_IA' TEXT DEFAULT 'false'",
				};

				foreach (string Table in NuevosCamposList)
					try
					{
						using (SqliteCommand cmd = new(Table, conn))
							cmd.ExecuteNonQuery();
					} catch (Exception) { }

				conn.Close();
				conn.Dispose();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}















	}
}








//"ALTER TABLE Config (" +
//"ADD COLUMN IF NOT EXISTS UltimaUbicacion TEXT DEFAULT '0'," +
// "ADD COLUMN IF NOT EXISTS UltimaPublicacion TEXT DEFAULT '0'," +
// "ADD COLUMN IF NOT EXISTS PublicacionesTotalHistorico TEXT DEFAULT '0'," +
// "ADD COLUMN IF NOT EXISTS PublicacionesMensuales TEXT DEFAULT '0'," +
// "ADD COLUMN IF NOT EXISTS PublicacionesDiarias TEXT DEFAULT '0'," +
// "ADD COLUMN IF NOT EXISTS DateUltimaPublicacion TEXT DEFAULT '1/1/0001 00:00:00'," +
// "ADD COLUMN IF NOT EXISTS Editar_Descripcion_IA TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Voltear TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_Inclinar TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_Mover TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_Recortar TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_QuitarMetadatos TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_EditarAlpha TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_EditarBrillo TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_EditarContraste TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_EditarTemperatura TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Pixelear TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Marcos TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Redimensionar TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Ruido TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_CambiarColores TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_Desenfoque TEXT DEFAULT 'false'," +
// "ADD COLUMN IF NOT EXISTS IM_EdicionGeneral TEXT DEFAULT 'true'," +
// "ADD COLUMN IF NOT EXISTS IM_RuidoGausiano TEXT DEFAULT 'false'" +
//");",