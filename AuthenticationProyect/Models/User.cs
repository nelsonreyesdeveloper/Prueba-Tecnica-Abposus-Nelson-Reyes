using System;
using System.Collections.Generic;

namespace AuthenticationProyect.Models
{
    public partial class User
    {
        public User()
        {
            Negocios = new HashSet<Negocio>();
        }

        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        public int? TipoId { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }

        public virtual UserTipo Tipo { get; set; }
        public virtual ICollection<Negocio> Negocios { get; set; }
    }
}
