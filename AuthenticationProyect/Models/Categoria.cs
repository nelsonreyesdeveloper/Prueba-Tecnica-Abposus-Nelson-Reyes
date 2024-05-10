using System;
using System.Collections.Generic;

namespace AuthenticationProyect.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string NombreCategoria { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
