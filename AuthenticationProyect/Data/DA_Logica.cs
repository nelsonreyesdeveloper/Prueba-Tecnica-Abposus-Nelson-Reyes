using AuthenticationProyect.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProyect.Data
{
    public class DA_Logica
    {
        private readonly PRUEBATECNICANELSONREYESContext _context;


        public DA_Logica(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
        }

        public async Task<List<User>> ListaUsuarios()
        {
            List<User> users = await _context.Users.Include(x => x.Tipo).ToListAsync();

            return users;
        }

        public async Task<User> ValidarUsuario(string email, string password)
        {
            List<User> users = await ListaUsuarios();
            User user = users.FirstOrDefault(item => item.Correo == email && item.Contrasenia == password);

            return user;
        }

    }
}
