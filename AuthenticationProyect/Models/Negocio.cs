using System;
using System.Collections.Generic;

namespace AuthenticationProyect.Models
{
    public partial class Negocio
    {
        public Negocio()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string NombreNegocio { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? UsuarioId { get; set; }

        public virtual User Usuario { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
