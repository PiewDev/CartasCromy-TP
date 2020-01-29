using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JuegoCromy;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace JuegoTests
{
    [TestClass]

    public class JuegoTests
    {
        [TestMethod]
        public void PartidaPermiteMezclarMazo()
        {
            Mazo Carta = new Mazo();
            Carta.CargarMazo("Armas de videojuegos");
            Cartas[] CartasOrdenadas = new Cartas[Carta.Cartas.Count];
            Carta.Cartas.CopyTo(CartasOrdenadas);
            Carta.MazclarCartas();

            bool Iguales = this.comprobarCartasNoMezcladas(CartasOrdenadas.ToList(), Carta.Cartas);


            Assert.IsFalse(Iguales, "El mazo no se mezcló");
        }

        private bool comprobarCartasNoMezcladas(List<Cartas> cartasOriginales, List<Cartas> cartasDespuesDeMezclar)
        {
            bool Iguales = true;
            for (int i = 0; i < cartasOriginales.Count; i++)
            {
                Iguales = cartasOriginales[i] == cartasDespuesDeMezclar[i];
                if (!Iguales)
                    break;
            }

            return Iguales;
        }

        [TestMethod]
        public void PartidaPermiteRepartirCartas()
        {
            Mazo Carta = new Mazo();
            Carta.CargarMazo("Armas de videojuegos");
            
            int cont = 0;
            bool repetidas = true;
            foreach (var item1 in Carta.Cartas)
            {
                if (repetidas)
                {
                    foreach (var item2 in Carta.Cartas)
                    {
                        cont += item1 == item2 ? 1 : 0;
                    }

                    repetidas = (cont == 1);
                    cont = 0;
                }
            }

            bool CartasIguales;
            var juego = new JuegoCromy.JuegoCromy();
            juego.Jugador1.ConectionID = "a";
            juego.Jugador2.ConectionID = "a";
            juego.MazoCompleto = Carta;
            juego.RepartirCartas();
            CartasIguales = (juego.Jugador1.Mazo.Count == juego.Jugador2.Mazo.Count);


            Assert.IsTrue(CartasIguales, "Los jugadores no tienen la misma cantidad de cartas");
            Assert.IsTrue(repetidas, "Hay dos cartas iguales");
        }

        [TestMethod]
        public void NoPermiteRepartirSinJugadores()
        {
            var juego = new JuegoCromy.JuegoCromy();

            Mazo Carta = new Mazo();
            juego.MazoCompleto = Carta;

            Carta.CargarMazo("Armas de videojuegos");

            Cartas[] CartasOrdenadas = new Cartas[Carta.Cartas.Count];
            Carta.Cartas.CopyTo(CartasOrdenadas);

            juego.RepartirCartas(); // mezcla y reparte las cartas

            bool repartido = juego.Jugador1.Mazo.Count != 0;
            repartido = repartido && juego.Jugador2.Mazo.Count != 0;

            bool noMezclado = this.comprobarCartasNoMezcladas(CartasOrdenadas.ToList(), juego.MazoCompleto.Cartas);


            Assert.IsTrue(noMezclado && !repartido);
        }

        [TestMethod]
        public void NoPuedoRepartirNiMezclarSinMazo()
        {
            var juego = new JuegoCromy.JuegoCromy();
            juego.Jugador1 = new Jugador() { ConectionID = "12432", Mazo = new List<Cartas>(), Nombre = "Juan" };
            juego.Jugador2 = new Jugador() { ConectionID = "12434", Mazo = new List<Cartas>(), Nombre = "Marcos" };

            bool mazoNoExiste = juego.MazoCompleto.Cartas.Count == 0;
            juego.RepartirCartas(); // Mezcla y reparte
            mazoNoExiste = juego.MazoCompleto.Cartas.Count == 0; // si despues de repartir sigue siendo 0 y no rompio, no permitio Mezclar / Repartir
            bool jugadorSinMano = juego.Jugador1.Mazo.Count == 0 && juego.Jugador2.Mazo.Count == 0;


            Assert.IsTrue(mazoNoExiste && jugadorSinMano);
        }
        private Juego HacerPartida()
        {
            Juego juego = new Juego();
            Jugador Jug1 = new Jugador() { ConectionID = "1", Nombre = "Pepe" };
            Jugador Jug2 = new Jugador() { ConectionID = "2", Nombre = "Franco" };
            var Partida = new Partidas() { Usuario = "Pepe", Nombre = "Partida de pepe", Mazo = "Armas de videojuegos", Activa = true };
            Partida.Preparar();
            Partida.Jugar.Jugador1 = Jug1;
            Partida.Jugar.Jugador2 = Jug2;
            juego.ListPartidas.Add(Partida);
            

            return juego;
        }


        [TestMethod]
        public void CargarJugador()
        {
            var juego = HacerPartida();
            var Partida = juego.ReturnPartida("Partida de pepe");
            bool Cargajugador = Partida.Jugar.Jugador1.ConectionID == "1" && Partida.Jugar.Jugador2.ConectionID == "2";
            Assert.IsTrue(Cargajugador, "No se cargaron bien los jugadores");
                      
        }

        [TestMethod]
        public void CargarMazo()
        {
            var juego = HacerPartida();
            var Partida = juego.ReturnPartida("Partida de pepe");
            juego.EliminarPartida(Partida);
            bool MazoLleno = Partida.Jugar.MazoCompleto.Cartas.Count() != 0;
            Assert.IsTrue(MazoLleno, "No se cargó correctamente el Mazo");


        }
        [TestMethod]
        public void ExistenciaPartida()
        {
            var juego = HacerPartida();
            var Partida = juego.ReturnPartida("Partida de pepe");
            bool PartidaExiste = juego.ListPartidas.Find(x => x.Nombre == "Partida de pepe") != null;
            Assert.IsTrue(PartidaExiste, "La partida no se guardó");


        }
      





        [TestMethod]
        public void CompararCartas()
        {
            var p1 = new Jugador() { Nombre = "juan", ConectionID = "12345142" };
            var p2 = new Jugador() { Nombre = "marcos", ConectionID = "12345123" };


            var juego = new Partidas() { Activa = true, Nombre = "nueva partida", Usuario = p1.Nombre, Mazo = "Armas de videojuegos" };
            juego.Preparar();
            juego.Jugar.Jugador1 = p1;
            juego.Jugar.Jugador2 = p2;

            juego.Comenzar();

            var carta = p1.Mazo[0];
            var carta2 = p2.Mazo[0];

            Jugador ganoManoEsperado = null;
            var attr = carta.Atributos[0].Propiedad;

            if (carta.Atributos[0].Valor >= carta.Atributos[1].Valor)
                ganoManoEsperado = p1;
            else
                ganoManoEsperado = p2;

            var ganoManoObtenido = juego.Jugar.CompararCartas(attr);

            Assert.AreEqual(ganoManoEsperado, ganoManoObtenido);
        }
       
    }
}
