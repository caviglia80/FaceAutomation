using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using AForge.Imaging;
using AForge.Imaging.ComplexFilters;
using AForge;
using static System.Net.Mime.MediaTypeNames;
using AForge.Imaging.Filters;
using AForge.Math.Random;

namespace FaceAutomation2
{
	public class IMG
	{

		//private static Image xxxxxxx(Image image)
		//{
		//	try
		//	{

		//		return image;
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show(ex.StackTrace);
		//		return image;
		//	}
		//}

		private static System.Drawing.Image IMAGEN_ORIGINAL;

		public static void EditarImagen(FileInfo Finfo, string ID)
		{
			try
			{
				IMAGEN_ORIGINAL = System.Drawing.Image.FromFile(Finfo.FullName);

				if (bool.Parse(BDQuery.GetConfiguration("IM_Voltear").ToString()) && !Finfo.Name.Contains("ED1")) IMAGEN_ORIGINAL = Voltear(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Inclinar").ToString()) && !Finfo.Name.Contains("ED1")) IMAGEN_ORIGINAL = Inclinar(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Mover").ToString()) && !Finfo.Name.Contains("ED1")) IMAGEN_ORIGINAL = Mover(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Recortar").ToString()) && !Finfo.Name.Contains("ED1")) IMAGEN_ORIGINAL = Recortar(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Marcos").ToString())) IMAGEN_ORIGINAL = AgregarMarcos(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_QuitarMetadatos").ToString())) IMAGEN_ORIGINAL = QuitarMetadatos(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Redimensionar").ToString())) IMAGEN_ORIGINAL = Redimensionar(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_EditarAlpha").ToString())) IMAGEN_ORIGINAL = EditarAlpha(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_EditarBrillo").ToString())) IMAGEN_ORIGINAL = EditarBrillo(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_EditarContraste").ToString())) IMAGEN_ORIGINAL = EditarContraste(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_EditarTemperatura").ToString())) IMAGEN_ORIGINAL = EditarTemperatura(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Pixelear").ToString())) IMAGEN_ORIGINAL = Pixelear(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Ruido").ToString())) IMAGEN_ORIGINAL = Ruido(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_CambiarColores").ToString())) IMAGEN_ORIGINAL = CambiarColores(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_Desenfoque").ToString())) IMAGEN_ORIGINAL = Desenfoque(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_EdicionGeneral").ToString())) IMAGEN_ORIGINAL = EdicionGeneral(IMAGEN_ORIGINAL);
				if (bool.Parse(BDQuery.GetConfiguration("IM_RuidoGausiano").ToString())) IMAGEN_ORIGINAL = RuidoGausiano(IMAGEN_ORIGINAL);

				string name = Globals.Rando.Next(0, 100000).ToString();
				if (Finfo.Name.Contains("ED1"))
					name += "Z" + name;

				string FileName = string.Concat(Globals.IMFolder_Temporales.Replace("xxx", ID), name, Finfo.Extension);
				ComprimirImagen(IMAGEN_ORIGINAL, FileName, (long)Globals.Rando.Next(50, 60));
				IMAGEN_ORIGINAL.Dispose();
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static System.Drawing.Image Ruido(System.Drawing.Image image)
		{
			try
			{
				bool RuidoGris = Convert.ToBoolean(Globals.Rando.Next(0, 2));
				int valorRuido = Globals.Rando.Next(0, 40);
				Bitmap bmp = (Bitmap)image;

				// Creamos un bitmap con el mismo tamaño que el original
				// Este bitmap será el retorno de la función
				Bitmap bmp2 = new(bmp.Width, bmp.Height);

				int rojo, verde, azul, alfa;
				int ValorRandomRuido;
				// Recorremos la matriz (imagen)
				for (var i = 0; i <= bmp.Width - 1; i++)
				{
					for (var j = 0; j <= bmp.Height - 1; j++)
					{
						ValorRandomRuido = Globals.Rando.Next(-(valorRuido - 1), valorRuido + 1);
						rojo = bmp.GetPixel(i, j).R + ValorRandomRuido;

						if (RuidoGris == false)
							ValorRandomRuido = Globals.Rando.Next(-(valorRuido - 1), valorRuido + 1);
						verde = bmp.GetPixel(i, j).G + ValorRandomRuido;

						if (RuidoGris == false)
							ValorRandomRuido = Globals.Rando.Next(-(valorRuido - 1), valorRuido + 1);
						azul = bmp.GetPixel(i, j).B + ValorRandomRuido;

						// Si hay valores mayores de 255 los pasamos a 255
						// Si hay valores menores de 0 los pasamos a 0
						if (rojo < 0)
							rojo = 0;
						if (rojo > 255)
							rojo = 255;
						if (verde < 0)
							verde = 0;
						if (verde > 255)
							verde = 255;
						if (azul < 0)
							azul = 0;
						if (azul > 255)
							azul = 255;

						// El canal alfa lo conservamos
						alfa = bmp.GetPixel(i, j).A;

						// Pintamos bmp3 con los colores obtenidos
						bmp2.SetPixel(i, j, Color.FromArgb(alfa, rojo, verde, azul));
					}
				}
				image.Dispose();
				// Retornas el bitmap con ruido
				return (System.Drawing.Image)bmp2;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Inclinar(System.Drawing.Image image)
		{
			try
			{
				//create an empty Bitmap image
				Bitmap bmp = new Bitmap(image.Width, image.Height);

				//turn the Bitmap into a Graphics object
				Graphics gfx = Graphics.FromImage(bmp);

				//now we set the rotation point to the center of our image
				gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

				//now rotate the image
				gfx.RotateTransform((float)Globals.NextFloat(-6, 6));

				gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

				//set the InterpolationMode to HighQualityBicubic so to ensure a high
				//quality image once it is transformed to the specified size
				gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				//now draw our new image onto the graphics object
				gfx.DrawImage(image, new System.Drawing.Point(0, 0));

				image.Dispose();

				//dispose of our Graphics object
				gfx.Dispose();

				//return the image
				return (System.Drawing.Image)bmp;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Desenfoque(System.Drawing.Image imagen)
		{
			try
			{
				Bitmap image = new Bitmap(imagen);

				// Crear un filtro de desenfoque gaussiano con un radio de 5 y una desviación estándar de 1.5
				GaussianBlur filter = new GaussianBlur(1.5, Globals.Rando.Next(0, 10));

				// Aplicar el filtro a la imagen
				Bitmap filteredImage = filter.Apply(image);

				return (System.Drawing.Image)filteredImage;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return imagen;
			}
		}

		private static System.Drawing.Image RuidoGausiano(System.Drawing.Image imagen)
		{
			try
			{
				Bitmap image = new Bitmap(imagen);
				image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), PixelFormat.Format24bppRgb);
				// aplicar ruido aditivo a la imagen
				AdditiveNoise noiseFilter = new AdditiveNoise(new GaussianGenerator(0, Globals.Rando.Next(1, 20)));
				Bitmap imagenConRuido = (Bitmap)noiseFilter.Apply(image);
				return (System.Drawing.Image)imagenConRuido;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return imagen;
			}
		}

		private static System.Drawing.Image EdicionGeneral(System.Drawing.Image imagen)
		{
			Bitmap image = new(imagen);

			// Crear un objeto Random para generar valores aleatorios
			Random rand = new();

			// Crear una copia de la imagen original
			Bitmap nuevaImagen = new(image.Width, image.Height);

			// Iterar sobre cada píxel de la imagen
			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					// Obtener el valor RGB del píxel original
					Color originalColor = image.GetPixel(x, y);
					int r = originalColor.R;
					int g = originalColor.G;
					int b = originalColor.B;

					// Generar valores aleatorios para cambiar el valor de cada canal de color
					int rRand = rand.Next(-50, 50);
					int gRand = rand.Next(-50, 50);
					int bRand = rand.Next(-50, 50);

					// Sumar los valores aleatorios a los canales de color originales
					r += rRand;
					g += gRand;
					b += bRand;

					// Asegurarse de que los valores RGB están dentro del rango válido (0-255)
					r = Math.Max(0, Math.Min(255, r));
					g = Math.Max(0, Math.Min(255, g));
					b = Math.Max(0, Math.Min(255, b));

					// Crear un nuevo color con los valores RGB modificados
					Color nuevoColor = Color.FromArgb(r, g, b);

					// Asignar el nuevo color al píxel correspondiente en la nueva imagen
					nuevaImagen.SetPixel(x, y, nuevoColor);
				}
			}
			return nuevaImagen;
		}


		private static System.Drawing.Image Redimensionar (System.Drawing.Image image)
		{
			try
			{
				int rm = Globals.Rando.Next(-50, 50);
				Bitmap img = new(image, new Size(image.Width + rm, image.Height + rm));
				image.Dispose();
				float Res = Globals.NextFloat(70.2f, 99.5f);
				img.SetResolution(Res, Res);
				return img;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static void ComprimirImagen(System.Drawing.Image image, string FileSave, long lCompression)  // lCompression= 0 a 100
		{
			try
			{
				System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
				eps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, lCompression);
				System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
				image.Save(FileSave, ici, eps);
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
			}
		}

		private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			try
			{
				int j;
				System.Drawing.Imaging.ImageCodecInfo[] encoders;
				encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

				for (j= 0; j < encoders.Length; j++)
					if (encoders[j].MimeType == mimeType)
						return encoders[j];
				return null;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return null;
			}
		}

		private static System.Drawing.Image QuitarMetadatos(System.Drawing.Image image)
		{
			try
			{
				foreach (int prop in image.PropertyIdList)
					image.RemovePropertyItem(prop);
				return image;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image AgregarMarcos(System.Drawing.Image image)
		{
			try
			{
				Bitmap bitmap1 = (Bitmap)image;
				int alto = bitmap1.Height;
				int ancho = bitmap1.Width;

				switch (Globals.Rando.Next(0,7))
				{
					case 0:
						for (int x = 0; x < bitmap1.Width - 1; x++)
						{
							bitmap1.SetPixel(x, 1, Color.White);
							bitmap1.SetPixel(x, 2, Color.White);
							bitmap1.SetPixel(x, 3, Color.White);
							bitmap1.SetPixel(x, 4, Color.White);
							bitmap1.SetPixel(x, 5, Color.White);
						}
						break;
					case 1:
						for (int x = 0; x < bitmap1.Width - 1; x++)
						{
							bitmap1.SetPixel(x, alto - 5, Color.White);
							bitmap1.SetPixel(x, alto - 4, Color.White);
							bitmap1.SetPixel(x, alto - 3, Color.White);
							bitmap1.SetPixel(x, alto - 2, Color.White);
							bitmap1.SetPixel(x, alto - 1, Color.White);
						}
						break;
					case 2:
						for (int x = 0; x < bitmap1.Width - 1; x++)
						{
							bitmap1.SetPixel(x, 1, Color.White);
							bitmap1.SetPixel(x, 2, Color.White);
							bitmap1.SetPixel(x, 3, Color.White);
							bitmap1.SetPixel(x, 4, Color.White);
							bitmap1.SetPixel(x, 5, Color.White);

							bitmap1.SetPixel(x, alto - 5, Color.White);
							bitmap1.SetPixel(x, alto - 4, Color.White);
							bitmap1.SetPixel(x, alto - 3, Color.White);
							bitmap1.SetPixel(x, alto - 2, Color.White);
							bitmap1.SetPixel(x, alto - 1, Color.White);
						}
						break;
					case 3:
						for (int y = 0; y < bitmap1.Height - 1; y++)
						{
							bitmap1.SetPixel(1, y, Color.White);
							bitmap1.SetPixel(2, y, Color.White);
							bitmap1.SetPixel(3, y, Color.White);
							bitmap1.SetPixel(4, y, Color.White);
							bitmap1.SetPixel(5, y, Color.White);
						}
						break;
					case 4:
						for (int y = 0; y < bitmap1.Height - 1; y++)
						{
							bitmap1.SetPixel(ancho - 5, y, Color.White);
							bitmap1.SetPixel(ancho - 4, y, Color.White);
							bitmap1.SetPixel(ancho - 3, y, Color.White);
							bitmap1.SetPixel(ancho - 2, y, Color.White);
							bitmap1.SetPixel(ancho - 1, y, Color.White);
						}
						break;
					case 5:
						for (int y = 0; y < bitmap1.Height - 1; y++)
						{
							bitmap1.SetPixel(1, y, Color.White);
							bitmap1.SetPixel(2, y, Color.White);
							bitmap1.SetPixel(3, y, Color.White);
							bitmap1.SetPixel(4, y, Color.White);
							bitmap1.SetPixel(5, y, Color.White);

							bitmap1.SetPixel(ancho - 5, y, Color.White);
							bitmap1.SetPixel(ancho - 4, y, Color.White);
							bitmap1.SetPixel(ancho - 3, y, Color.White);
							bitmap1.SetPixel(ancho - 2, y, Color.White);
							bitmap1.SetPixel(ancho - 1, y, Color.White);
						}
						break;
					default:
						break;
				}
				return (System.Drawing.Image)bitmap1;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Mover(System.Drawing.Image image)
		{
			try
			{
				//	'dejar los primeros 2 en 0, ya que mueven la imagen pero mantienen dimensiones
				Rectangle CropRect = new();

				switch (Globals.Rando.Next(1, 3))
				{
					case 1:
						CropRect = new Rectangle(0, Convert.ToInt32(Globals.Rando.Next(40, 70) / 2), image.Width, image.Height);
						break;

					case 2:
						CropRect = new Rectangle(Convert.ToInt32(Globals.Rando.Next(40, 70) / 2), 0, image.Width, image.Height);
						break;
				}

				Bitmap CropImage = new(CropRect.Width, CropRect.Height);
				using (var grp = Graphics.FromImage(CropImage))
					grp.DrawImage(image, new Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel);
				image.Dispose();
				image = (System.Drawing.Image)new Bitmap(CropImage, CropImage.Width, CropImage.Height);
				CropImage.Dispose();
				return image;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Voltear(System.Drawing.Image image) 
		{
			try
			{
				switch (Globals.Rando.Next(0, 2))
				{
					case 1:
						image.RotateFlip(RotateFlipType.RotateNoneFlipX);
						break;
				}
				return image;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Recortar(System.Drawing.Image image)
		{
			try
			{
				// dejar los primeros 2 en 0, ya que mueven la imagen pero mantienen dimensiones
				Rectangle CropRect = new Rectangle();
				switch (Globals.Rando.Next(1, 5))
				{
					case 1:
						{
							CropRect = new Rectangle(0, 0, image.Width + System.Convert.ToInt32(Globals.Rando.Next(40, 120) / (double)2), image.Height);
							break;
						}

					case 2:
						{
							CropRect = new Rectangle(0, 0, image.Width, image.Height + System.Convert.ToInt32(Globals.Rando.Next(40, 120) / (double)2));
							break;
						}

					case 3:
						{
							CropRect = new Rectangle(0, 0, image.Width - System.Convert.ToInt32(Globals.Rando.Next(40, 120) / (double)2), image.Height);
							break;
						}

					case 4:
						{
							CropRect = new Rectangle(0, 0, image.Width, image.Height - System.Convert.ToInt32(Globals.Rando.Next(40, 120) / (double)2));
							break;
						}
				}

				Bitmap CropImage = new Bitmap(CropRect.Width, CropRect.Height);
				using (var grp = Graphics.FromImage(CropImage))
					grp.DrawImage(image, new Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel);
				image.Dispose();
				Bitmap imagen = new Bitmap(CropImage, CropImage.Width, CropImage.Height);
				CropImage.Dispose();
				return (System.Drawing.Image)imagen;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image Pixelear(System.Drawing.Image image) //agrega puntos negros cuando x e y son pares
		{
			try
			{
				Bitmap bmp = (Bitmap)image;
				switch (Globals.Rando.Next(0, 2))
				{
					case 0:
						switch (Globals.Rando.Next(0, 5))
						{
							case 0:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) == 0)
											if ((y % 2) == 0)
												bmp.SetPixel(x, y, Color.DimGray);
									}
								break;
							case 1:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) == 0)
											if ((y % 2) == 0)
												bmp.SetPixel(x, y, Color.Black);
									}
								break;
							case 2:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) == 0)
											if ((y % 2) == 0)
												bmp.SetPixel(x, y, Color.Gray);
									}
								break;
							case 3:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) == 0)
											if ((y % 2) == 0)
												bmp.SetPixel(x, y, Color.DarkGray);
									}
								break;
							case 4:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) == 0)
											if ((y % 2) == 0)
												bmp.SetPixel(x, y, Color.DarkSlateGray);
									}
								break;
							default:
								break;
						}
						break;
					case 1:
						switch (Globals.Rando.Next(0, 5))
						{
							case 0:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) != 0)
											if ((y % 2) != 0)
												bmp.SetPixel(x, y, Color.DimGray);
									}
								break;
							case 1:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) != 0)
											if ((y % 2) != 0)
												bmp.SetPixel(x, y, Color.Black);
									}
								break;
							case 2:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) != 0)
											if ((y % 2) != 0)
												bmp.SetPixel(x, y, Color.Gray);
									}
								break;
							case 3:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) != 0)
											if ((y % 2) != 0)
												bmp.SetPixel(x, y, Color.DarkGray);
									}
								break;
							case 4:
								for (int x = 0; x < bmp.Width - 1; x++)
									for (int y = 0; y < bmp.Height - 1; y++)
									{
										Color oldPixelColour = bmp.GetPixel(x, y);
										if ((x % 2) != 0)
											if ((y % 2) != 0)
												bmp.SetPixel(x, y, Color.DarkSlateGray);
									}
								break;
							default:
								break;
						}
						break;
				}
		
				return (System.Drawing.Image)bmp;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image EditarTemperatura(System.Drawing.Image image)
		{
			try
			{
				//float[][] matrizContraste = {
				//new float[] { 1, 0, 0, 0, 0}, // aplica a RED						{1, verde, violeta, 0, 0}      0.3f max
				//new float[] { 0, 1, 0, 0, 0}, // aplica a GREEN						{rojo, 1, azul, 0, 0}
				//new float[] { 0, 0, 1, 0, 0}, // aplica a BLUE						{0, verde, 1, 0, 0}
				//new float[] { 0, 0, 0, 1.0f, 0}, // canal alpha						{0, verde, 0, 1.0f, 0}
				//new float[] { 0, 0, 0, 0, 1}};

				float cantidad = (float)Globals.NextFloat(1.0f, 4.0f) / 10;

				float[][] matriz = {
			new float[] { 1, cantidad, cantidad, 0, 0},
			new float[] { cantidad, 1, cantidad, 0, 0},
			new float[] { 0, 0, 1f + cantidad , 0, 0},
			new float[] { 0, 0, 0, 1.0f, 0},
			new float[] { 0, 0, 0, 0, 1}};

				// Creamos los atributos en función de nuestra matriz
				// para aplicar a la imagen
				System.Drawing.Imaging.ColorMatrix cmx = new System.Drawing.Imaging.ColorMatrix(matriz);
				System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
				imgAttr.SetColorMatrix(cmx);

				// Trasladamos la imagen a un bitmap ya con el efecto de contraste
				// y brillo aplicado para poder trabajar sobre eso
				Bitmap bmpCanvas = new Bitmap(image.Width, image.Height, image.PixelFormat);
				Graphics g = Graphics.FromImage(bmpCanvas);
				g.DrawImage(image, new Rectangle(System.Drawing.Point.Empty, bmpCanvas.Size),
							0, 0,
							image.Width, image.Height,
							GraphicsUnit.Pixel, imgAttr);

				// Liberamos recursos
				g.Dispose();
				imgAttr.Dispose();
				image.Dispose();
				// Retornamos la imagen con el filtro aplicado
				return (System.Drawing.Image)bmpCanvas;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image EditarContraste(System.Drawing.Image image)   //	  0.5f a 1.7f max (S/E 1.0f)
		{
			try
			{
				float p_Contraste = Globals.NextFloat(0.5f, 1.7f);

				// Creamos una matriz para aplicar contraste y brillo
				// a una imagen dada.
				float[][] matrizContraste = {
		   new float[] {p_Contraste, 0, 0, 0, 0}, // aplica a RED 
           new float[] {0, p_Contraste, 0, 0, 0}, // aplica a GREEN
           new float[] {0, 0, p_Contraste, 0, 0}, // aplica a BLUE
           new float[] {0, 0, 0, 1.0f, 0},        // canal alpha
           new float[] { 0, 0, 0, 0, 1}};

				// Creamos los atributos en función de nuestra matriz
				// para aplicar a la imagen
				System.Drawing.Imaging.ColorMatrix cmx = new System.Drawing.Imaging.ColorMatrix(matrizContraste);
				System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
				imgAttr.SetColorMatrix(cmx);

				// Trasladamos la imagen a un bitmap ya con el efecto de contraste
				// y brillo aplicado para poder trabajar sobre eso
				Bitmap bmpCanvas = new Bitmap(image.Width, image.Height, image.PixelFormat);
				Graphics g = Graphics.FromImage(bmpCanvas);
				g.DrawImage(image, new Rectangle(System.Drawing.Point.Empty, bmpCanvas.Size),
							0, 0,
							image.Width, image.Height,
							GraphicsUnit.Pixel, imgAttr);

				// Liberamos recursos
				g.Dispose();
				imgAttr.Dispose();
				image.Dispose();
				// Retornamos la imagen con el filtro aplicado
				return (System.Drawing.Image)bmpCanvas;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image EditarBrillo(System.Drawing.Image image)
		{
			try
			{
				float brilloAjustado = Globals.NextFloat(0.8f, 1.2f) - 1.0f;            // 0.8f a 1.2f max (S/E 1.0f) Globals.NextFloat(0.8f, 1.2f)
																						// Creamos una matriz para aplicar contraste y brillo
																						// a una imagen dada.
				float[][] matriz = {
		   new float[] {1, 0, 0, 0, 0}, // aplica a RED 
           new float[] {0, 1, 0, 0, 0}, // aplica a GREEN
           new float[] {0, 0, 1, 0, 0}, // aplica a BLUE
           new float[] {0, 0, 0, 1.0f, 0},        // canal alpha 
           new float[] {brilloAjustado, brilloAjustado, brilloAjustado, 0, 1}};

				// Creamos los atributos en función de nuestra matriz
				// para aplicar a la imagen
				System.Drawing.Imaging.ColorMatrix cmx = new System.Drawing.Imaging.ColorMatrix(matriz);
				System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
				imgAttr.SetColorMatrix(cmx);

				// Trasladamos la imagen a un bitmap ya con el efecto de contraste
				// y brillo aplicado para poder trabajar sobre eso
				Bitmap bmpCanvas = new Bitmap(image.Width, image.Height, image.PixelFormat);
				Graphics g = Graphics.FromImage(bmpCanvas);
				g.DrawImage(image, new Rectangle(System.Drawing.Point.Empty, bmpCanvas.Size),
							0, 0,
							image.Width, image.Height,
							GraphicsUnit.Pixel, imgAttr);

				// Liberamos recursos
				g.Dispose();
				imgAttr.Dispose();
				image.Dispose();
				// Retornamos la imagen con el filtro aplicado
				return (System.Drawing.Image)bmpCanvas;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image EditarAlpha(System.Drawing.Image image)
		{
			try
			{
				// Creamos una matriz para aplicar contraste y brillo
				// a una imagen dada.
				float[][] matrizContraste = {
		   new float[] {1, 0, 0, 0, 0}, // aplica a RED 
           new float[] {0, 1, 0, 0, 0}, // aplica a GREEN
           new float[] {0, 0, 1, 0, 0}, // aplica a BLUE
           new float[] {0, 0, 0, (float)Globals.NextFloat(0.6f, 1.0f), 0},   // canal alpha //  // 0.5f a 1.0f max (S/E 1.0f) Globals.NextFloat(-6, 6)
           new float[] {0, 0, 0, 0, 1}};

				// Creamos los atributos en función de nuestra matriz
				// para aplicar a la imagen
				System.Drawing.Imaging.ColorMatrix cmx = new System.Drawing.Imaging.ColorMatrix(matrizContraste);
				System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
				imgAttr.SetColorMatrix(cmx);

				// Trasladamos la imagen a un bitmap ya con el efecto de contraste
				// y brillo aplicado para poder trabajar sobre eso
				Bitmap bmpCanvas = new Bitmap(image.Width, image.Height, image.PixelFormat);
				Graphics g = Graphics.FromImage(bmpCanvas);
				g.DrawImage(image, new Rectangle(System.Drawing.Point.Empty, bmpCanvas.Size),
							0, 0,
							image.Width, image.Height,
							GraphicsUnit.Pixel, imgAttr);

				// Liberamos recursos
				g.Dispose();
				imgAttr.Dispose();
				image.Dispose();
				// Retornamos la imagen con el filtro aplicado
				return (System.Drawing.Image)bmpCanvas;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

		private static System.Drawing.Image CambiarColores(System.Drawing.Image image)
		{
			try
			{
				float[][] matriz = {
			   new float[] {(float)Globals.NextFloat(1.0f, 1.2f),  0,  0,  0, 0},        // red scaling factor of 2
			   new float[] {0, (float)Globals.NextFloat(1.0f, 1.2f),  0,  0, 0},        // green scaling factor of 1
			   new float[] {0,  0, (float)Globals.NextFloat(1.0f, 1.2f),  0, 0},        // blue scaling factor of 1
			   new float[] {0,  0,  0, 1, 0},											// alpha scaling factor of 1
			   new float[] { (float)Globals.NextFloat(.0f, .2f), (float)Globals.NextFloat(.0f, .2f), (float)Globals.NextFloat(.0f, .2f), 0, 1}};

				// Creamos los atributos en función de nuestra matriz
				// para aplicar a la imagen
				System.Drawing.Imaging.ColorMatrix cmx = new System.Drawing.Imaging.ColorMatrix(matriz);
				System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
				//imgAttr.SetColorMatrix(cmx);

				imgAttr.SetColorMatrix(
									   cmx,
									   ColorMatrixFlag.Default,
									   ColorAdjustType.Bitmap);

				Bitmap bmpCanvas = new Bitmap(image.Width, image.Height, image.PixelFormat);
				Graphics g = Graphics.FromImage(bmpCanvas);
				g.DrawImage(image, new Rectangle(System.Drawing.Point.Empty, bmpCanvas.Size),
							0, 0,
							image.Width, image.Height,
							GraphicsUnit.Pixel, imgAttr);

				// Liberamos recursos
				g.Dispose();
				imgAttr.Dispose();
				image.Dispose();
				// Retornamos la imagen con el filtro aplicado
				return (System.Drawing.Image)bmpCanvas;
			}
			catch (Exception ex)
			{
				Globals.DebugSendMessaje(Globals.GetFullTextException(ex));
				return image;
			}
		}

	}
}




//List<int> PropListID = new();
//foreach (var Pitem in image.PropertyItems)
//	PropListID.Add(Pitem.Id);

//System.Drawing.Imaging.PropertyItem propItem;
//foreach (int PropiedadID in PropListID)
//{
//	propItem = image.GetPropertyItem(PropiedadID);
//	propItem.Id = Convert.ToInt32(propItem.Id - 1);
//	propItem.Type = 2;
//	propItem.Value = Encoding.ASCII.GetBytes(Globals.Rando.Next(100, 900000).ToString());
//	propItem.Len = propItem.Value.Length;
//	image.SetPropertyItem(propItem);
//}





//private static Image ByteArrayToImage(byte[] arregloDeBytes)
//{
//	System.IO.MemoryStream ms = new System.IO.MemoryStream(arregloDeBytes);
//	Image imagen = Image.FromStream(ms);
//	return imagen;
//}