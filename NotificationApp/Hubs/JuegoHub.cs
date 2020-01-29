using JuegoCromy;
using System.Linq;

namespace NotificationApp.Hubs
{   
    using Microsoft.AspNet.SignalR;
    using System.Collections.Generic;
    

    /// <summary>
    /// Hub de notificaciones de la aplicación.
    /// </summary>
    public class JuegoHub : Hub
    {
        private static Juego juego = new Juego();


        public void CrearPartida(string usuario, string partida, string mazo)
        {
            Partidas Partida = null;

            try
            {
                // Notifico a los otros usuarios de la nueva partida.
                Partida = new Partidas() { Usuario = usuario, Nombre = partida, Mazo = mazo, Activa = true};
                Partida.Preparar();
                Partida.Jugar.OnFinJuego += Juego_OnFin;
            }
            catch(System.Exception e)
            {
                Partida = null;
                Clients.Caller.mostrarError($"[ERROR: 404] No se encontró el archivo de información.");
            }

            if(Partida != null)
            {
                Partida.Jugar.Jugador1 = new Jugador() { ConectionID = Context.ConnectionId, Nombre = usuario };
                juego.ListPartidas.Add(Partida);
                Clients.Others.agregarPartida(Partida);
                Clients.Caller.cleanErrors();
                Clients.Caller.esperarJugador();
            }
        }

        public void UnirsePartida(string usuario, string partida)
        {
            var Match = juego.ReturnPartida(partida);
            Match.Jugar.Jugador2 = new Jugador() { ConectionID = Context.ConnectionId, Nombre = usuario };
            Clients.All.eliminarPartida(Match.Nombre);
            Match.Activa = false;
            Match.Comenzar();
            this.DibujarTablero(Match);
        }

        private void DibujarTablero(Partidas Match)
        {
            var jugador1 = new { Nombre = Match.Jugar.Jugador1.Nombre, Cartas = Match.Jugar.Jugador1.Mazo };
            var jugador2 = new { Nombre = Match.Jugar.Jugador2.Nombre, Cartas = Match.Jugar.Jugador2.Mazo };


            Clients.Client(Match.Jugar.Jugador1.ConectionID).dibujarTablero(jugador1, jugador2, Match.Jugar.MazoCompleto);
            Clients.Client(Match.Jugar.Jugador2.ConectionID).dibujarTablero(jugador1, jugador2, Match.Jugar.MazoCompleto);
        }

        public void enviarMensaje(string mensaje, string jugador)
        {
            var partida = juego.ListPartidas.Find(p => p.Jugar.Jugador1.Nombre == jugador || p.Jugar.Jugador2.Nombre == jugador);
            partida.enviarMensaje(mensaje, jugador);
            Clients.Client(partida.Jugar.Jugador1.ConectionID).refrescarChat(partida.ObtenerChat().Last());
            Clients.Client(partida.Jugar.Jugador2.ConectionID).refrescarChat(partida.ObtenerChat().Last());
        }

        public void ObtenerPartidas()
        {
          
          Clients.Caller.agregarPartidas(juego.ListaDePartidas());
        }

        public void ObtenerMazos()
        {
           var baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
           var mazos = System.IO.Directory.GetDirectories($"{baseDir}\\Mazos\\");
            var listaMazos = new List<string>();
            foreach(var mazo in mazos)
            {
                var mazoParsed = new System.IO.DirectoryInfo(mazo).Name;
                listaMazos.Add(mazoParsed);
            }

           Clients.Caller.agregarMazos(listaMazos);
        }

        public void Cantar(string idAtributo, string idCarta)
        {

            var id = Context.ConnectionId;
            var partida = juego.ListPartidas.Find(p => p.Jugar.Jugador1.ConectionID == id || p.Jugar.Jugador2.ConectionID == id);
            var ganador = partida.Jugar.CompararCartas(idAtributo);
            var perdedor = partida.Jugar.Jugador1 == ganador ? partida.Jugar.Jugador2 : partida.Jugar.Jugador1;
            var TipoCartaGanadora = ganador.RetornarCartaJuego().Tipo;
            partida.Jugar.AcomodarCartas(ganador);
            if (TipoCartaGanadora == EnumCarta.amarillo)
            {
                Clients.Client(ganador.ConectionID).ganarManoPorTarjetaAmarilla();
                Clients.Client(perdedor.ConectionID).perderManoPorTarjetaAmarilla();
            }
            else if (TipoCartaGanadora == EnumCarta.rojo)
            {
                Clients.Client(ganador.ConectionID).ganarManoPorTarjetaRoja();
                Clients.Client(perdedor.ConectionID).perderManoPorTarjetaRoja();

            }
            else
            {
                Clients.Client(ganador.ConectionID).ganarMano();
                Clients.Client(perdedor.ConectionID).perderMano();
            }

                           
            
        }


        public void Juego_OnFin(object winner, object perdedor, JuegoCromy.JuegoCromy sender)
        {
            Jugador ganador = (Jugador)winner;
            Jugador loser = (Jugador)perdedor;

            Clients.Client(ganador.ConectionID).ganar();
            Clients.Client(loser.ConectionID).perder();
        }
    }

    
}