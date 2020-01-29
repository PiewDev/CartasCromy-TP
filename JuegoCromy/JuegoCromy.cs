using System;
using System.Collections.Generic;
using System.Linq;

namespace JuegoCromy
{

    public delegate void JuegoFinEventHandler(object winner, object perdedor, JuegoCromy sender);

    public class JuegoCromy
    {
        public Mazo MazoCompleto { get; set; }
        public Jugador Jugador1 { get; set; }
        public Jugador Jugador2 { get; set; }
        public event JuegoFinEventHandler OnFinJuego;

        public JuegoCromy()
        {
            this.MazoCompleto = new Mazo();
            this.Jugador1 = new Jugador();
            this.Jugador2 = new Jugador();
        }

        

        public void CargarMazo(string nombre)
        {
            try
            {
                this.MazoCompleto.CargarMazo(nombre);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    

        public void RepartirCartas()
        {
            if ((Jugador1.ConectionID != null || Jugador2.ConectionID != null) && this.MazoCompleto != null)
            { 
                this.MazoCompleto.MazclarCartas();
                int Contador = 0;
                foreach (var carta in this.MazoCompleto.Cartas)
                {
                    Contador += 1;
                    if ((Contador % 2) == 0)
                    {
                        Jugador1.AñadirCartas(carta);
                    }
                    else
                    {
                        Jugador2.AñadirCartas(carta);
                    }

                }
            }
        }

        public Jugador CompararCartas(string caracteritica)
        {
            if (this.Jugador1.Mazo[0].Tipo == EnumCarta.rojo || this.Jugador2.Mazo[0].Tipo == EnumCarta.rojo)
            {
                return this.Jugador1.Mazo[0].Tipo == EnumCarta.rojo ? Jugador1 : Jugador2;
            }
            else
            {
                if (this.Jugador1.Mazo[0].Tipo == EnumCarta.amarillo || this.Jugador2.Mazo[0].Tipo == EnumCarta.amarillo)
                {
                    return this.Jugador1.Mazo[0].Tipo == EnumCarta.amarillo ? Jugador1 : Jugador2;
                }
                else
                {
                    var Caracteristica1 = this.Jugador1.Mazo[0].Atributos.Where(x => x.Propiedad == caracteritica).Single();
                    var Caracteristica2 = this.Jugador2.Mazo[0].Atributos.Where(x => x.Propiedad == caracteritica).Single();
                    if (Caracteristica1.Valor >= Caracteristica2.Valor)
                    {
                        return Jugador1;
                    }
                    else
                    {
                        return Jugador2;
                    }

                }
            }


        }


        public void AcomodarCartas(Jugador Ganador)
        {


            if (Ganador == this.Jugador1)
            {
                if (Ganador.Mazo[0].Tipo == EnumCarta.rojo)
                {
                    this.Jugador1.Mazo.Add(this.Jugador2.Mazo[0]);
                    this.Jugador2.Mazo.Remove(this.Jugador2.Mazo[0]);
                    if (Jugador2.Mazo.Count !=0)
                        this.Jugador1.Mazo.Add(this.Jugador2.Mazo[0]);
                }
                else
                {
                    this.Jugador1.Mazo.Add(this.Jugador1.Mazo[0]);
                    this.Jugador1.Mazo.Add(this.Jugador2.Mazo[0]);
                }
                if (Jugador2.Mazo.Count != 0)
                    this.Jugador2.Mazo.Remove(this.Jugador2.Mazo[0]);
                this.Jugador1.Mazo.Remove(this.Jugador1.Mazo[0]);
            }
            else
            {
                if (Ganador.Mazo[0].Tipo == EnumCarta.rojo)
                {
                    this.Jugador2.Mazo.Add(this.Jugador1.Mazo[0]);
                    this.Jugador1.Mazo.Remove(this.Jugador1.Mazo[0]);
                    if (Jugador1.Mazo.Count != 0)
                        this.Jugador2.Mazo.Add(this.Jugador1.Mazo[0]);
                }
                else
                {
                    this.Jugador2.Mazo.Add(this.Jugador1.Mazo[0]);
                    this.Jugador2.Mazo.Add(this.Jugador2.Mazo[0]);
                }

                if (Jugador2.Mazo.Count != 0)
                    this.Jugador1.Mazo.Remove(this.Jugador1.Mazo[0]);
                this.Jugador2.Mazo.Remove(this.Jugador2.Mazo[0]);

            }

            Jugador winner = null;
            Jugador perdedor = null;

            if (Jugador1.Mazo.Count == 0)
            {
                winner = Jugador2;
                perdedor = Jugador1;
            }
            else if(Jugador2.Mazo.Count == 0)
            {
                winner = Jugador1;
                perdedor = Jugador2;
            }

            if (OnFinJuego != null && winner != null && perdedor != null)
            {
                this.OnFinJuego(winner, perdedor, this);
            }
        }

        
 
        
           
            



        
    }

   }