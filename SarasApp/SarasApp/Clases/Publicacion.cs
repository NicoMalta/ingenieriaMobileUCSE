using System;
using System.Collections.Generic;
using System.Text;

namespace SarasApp
{
    public class Publicacion
    {
        public int idPublicacion { get; set; }
        public int Destacar { get; set; }
        public int Estado { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int idUserPublico { get; set; }
        public int? idGrupoPu { get; set; }
        
    }
}
