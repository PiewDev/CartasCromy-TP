using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCromy
{
    public class Mazo
    {
        public string Nombre { get; set; }
        public List<string> NombreAtributos { get; set; }
        public List<Cartas> Cartas { get; set; }
        private List<string> _infoData;
        public Mazo()
        {
            NombreAtributos = new List<string>();
            Cartas = new List<Cartas>();
        }

        public void CargarMazo(string nombre)
        {
            try
            {
                this.Nombre = nombre;
                ObtenerDatos();
                CargarAtributos();
                CargarCartas();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Mazo(string nombre, List<string> attrs, List<Cartas> cartas) : this()
        {
            Nombre = nombre;
            NombreAtributos = attrs;
            Cartas = cartas;
        }
        public void MazclarCartas()
        {
            List<Cartas> arrDes = new List<Cartas>();
            Random randNum = new Random();

            while (this.Cartas.Count > 0)
            {
                int val = randNum.Next(0, this.Cartas.Count - 1);
                arrDes.Add(this.Cartas[val]);
                this.Cartas.RemoveAt(val);
            }
            this.Cartas = arrDes;
            

        }

        private void CargarAmarillayRoja()
        {
            this.Cartas.Add(new Cartas { Nombre = "Carta_amarilla", Codigo = "amarilla", Tipo = EnumCarta.amarillo });
            this.Cartas.Add(new Cartas { Nombre = "Carta_roja", Codigo = "roja", Tipo = EnumCarta.rojo });
        }
        private void ObtenerDatos()
        {
            List<string> fileStream = null;
            try
            {
                var baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
                if (baseDir.Last() != '\\')
                    baseDir += '\\';
                var data = System.IO.Directory.GetFiles($"{baseDir}Mazos\\{this.Nombre}");
                var file = data.First(f => f.Split('\\').Last() == "informacion.txt");
                var info = new System.IO.FileInfo(file);
                fileStream = System.IO.File.ReadAllLines(info.FullName, System.Text.Encoding.Default).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            _infoData = fileStream;
        }
       
        private void CargarAtributos()
        {

            if (_infoData != null)
            {
                var cartaFields = _infoData[1];
                var attrs = cartaFields.Split('|');
                for (int i = 0; i < attrs.Count(); i++)
                    if (i >= 2)
                        this.NombreAtributos.Add(this.convertirStrAUTF8(attrs[i]));
            }

        }

        private string convertirStrAUTF8(string str)
        {

            byte[] c = System.Text.Encoding.Default.GetBytes(str);

            var convertido = System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, c);

            return System.Text.Encoding.UTF8.GetString(convertido);
        }

        private void CargarCartas()
        {
            if (_infoData != null)
            {
                for (int j = 0; j < _infoData.Count(); j++)
                {
                    if (j > 1)
                    {
                        var carta = _infoData[j];
                        if (carta != string.Empty)
                        {
                            var attrs = carta.Split('|');

                            var nuevaCarta = new Cartas()
                            {
                                Tipo = EnumCarta.normal,
                                Codigo = attrs[0],
                                Nombre = attrs[1]
                            };

                            for (int i = 2; i < attrs.Count(); i++)
                            {

                                var strConvertido = this.NombreAtributos[i - 2];

                                var carac = new Caracteristicas()
                                {
                                    Propiedad = strConvertido,
                                    Valor = float.Parse(attrs[i])
                                };

                                nuevaCarta.Atributos.Add(carac);
                            }

                            this.Cartas.Add(nuevaCarta);
                        }
                    }
                }
                this.CargarAmarillayRoja();
            }
        }
    }
}
