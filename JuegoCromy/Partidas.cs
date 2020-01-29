using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoCromy
{
    public class Partidas
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Mazo { get; set; }
        public JuegoCromy Jugar { get; set; }
        public bool Activa { get; set; }
        private List<string> _chat;


        public void enviarMensaje(string mensaje, string jugador)
        {
            _chat.Add($"<b>[{ jugador }]:</b>   { mensaje }");
        }

        public List<string> ObtenerChat()
        {
            return _chat;
        }

        public Partidas()
        {
            _chat = new List<string>();
        }

        public void Preparar()
        {
            try
            {
                this.Jugar = new JuegoCromy();
                Jugar.CargarMazo(Mazo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Comenzar()
        {
            Jugar.RepartirCartas();
        }
    }
}
