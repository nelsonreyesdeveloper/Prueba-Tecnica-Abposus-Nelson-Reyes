using System;
using System.Collections.Generic;

namespace AuthenticationProyect.Models
{
    public partial class UserTipo
    {
        public UserTipo()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string NombreTipoUsuario { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
