using System;
using System.Collections.Generic;
using System.Linq;

namespace JuegoCromy
{
    public class Cartas
    {
        public List<Caracteristicas> Atributos { get; set; }
        public string Codigo { get; set; }
        public EnumCarta Tipo { get; set; }
        public string Nombre { get; set; }

        public Cartas()
        {
            this.Atributos = new List<Caracteristicas>();                      
        }

       
       
    }
}
