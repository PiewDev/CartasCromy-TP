using System;
using System.Collections.Generic;
using System.Linq;

namespace JuegoCromy
{
    public class Jugador
    {

        public string ConectionID { get; set; }
        public string Nombre { get; set; }
        public List<Cartas> Mazo{ get; set; }

        public Jugador()
        {
            
            this.Mazo = new List<Cartas>();
        }

        public void AñadirCartas(Cartas carta)
        {
            this.Mazo.Add(carta);
        }
        public Cartas RetornarCartaJuego()
        {
            return this.Mazo[0];
        }

    }
}
