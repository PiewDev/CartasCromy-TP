using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassJuego
{
    public class Bot : Jugador
    {
        public List<Cartas> MazoPosible { get; set; }
        List<Cartas> MazoImposible = new List<Cartas>();
        public string Propiedad { get; private set; }
        public int Valor { get; private set; }

        public Bot(string nombre):base(nombre)
        {
            this.MazoPosible = new List<Cartas>();
        }

        public Cartas ObtenerPromedio(List<Cartas> MazoCartas, Cartas CartaActual)
        {
            MazoImposible.Add(MazoCartas.Where(x => x == CartaActual).Single());
            if (MazoCartas.Where(x => x == CartaActual).FirstOrDefault() != null)
            {
                MazoPosible.Remove(MazoCartas.Where(x => x == CartaActual).FirstOrDefault());
            }
            
            var Carta = new Cartas();
            Carta = MazoCartas.First();
            foreach (var mazo in MazoCartas)
            {
                if (!(mazo == MazoCartas.First()))
                {
                    foreach (var propiedad in mazo.Atributos)
                    {
                        var Ind = Carta.Atributos.FindIndex(x => x.Propiedad == propiedad.Propiedad);
                        Carta.Atributos[Ind].Valor += propiedad.Valor;

                    }
                }

            }
            foreach (var caracteristica in Carta.Atributos)
            {
                caracteristica.Valor = caracteristica.Valor / MazoCartas.Count();
            }

            return Carta;

        }

      

        public string SeleccionarCaracteristica(Cartas CartaActual)
        {

            var Cartaprom = ObtenerPromedio(this.MazoPosible, CartaActual);
            foreach (var atributo in CartaActual.Atributos)
            {
                var Ind = Cartaprom.Atributos.FindIndex(x => x.Propiedad == atributo.Propiedad);
                Cartaprom.Atributos[Ind].Valor -= atributo.Valor;
            }
            return Cartaprom.Atributos.Where(x => x.Valor == Cartaprom.Atributos.Min(z => z.Valor)).First().Propiedad;


        }
    }

}



