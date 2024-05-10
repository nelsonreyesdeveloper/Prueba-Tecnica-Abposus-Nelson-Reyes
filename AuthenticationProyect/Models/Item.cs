using System;
using System.Collections.Generic;

namespace AuthenticationProyect.Models
{
    public partial class Item
    {
        public int Id { get; set; }
        public string NombreItem { get; set; }
        public string Descripcion { get; set; }
        public int? CategoriaId { get; set; }
        public decimal? Precio { get; set; }
        public int? NegocioId { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Negocio Negocio { get; set; }
    }
}
