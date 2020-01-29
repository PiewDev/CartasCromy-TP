using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCromy
{
    public class Juego
    {
        public List<Partidas> ListPartidas { get; set; }

        public Juego()
        {
            this.ListPartidas = new List<Partidas>();
        }
        
        public Partidas ReturnPartida(string Nombre)
        {
            return this.ListPartidas.Where(x => x.Nombre == Nombre && x.Activa).FirstOrDefault();
        }

        public void EliminarPartida(Partidas Partida)
        {
            this.ListPartidas.Find(x => x == Partida).Activa = false;
        }

        public List<Partidas> ListaDePartidas()
        {
            return this.ListPartidas.Where(x => x.Activa).ToList();
        }

    }
}
