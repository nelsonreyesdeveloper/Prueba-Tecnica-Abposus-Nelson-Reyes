using AuthenticationProyect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;


namespace AuthenticationProyect.Controllers
{
    [Authorize(Roles = "CLIENTE")]
    public class NegocioClienteController : Controller
    {
        private readonly PRUEBATECNICANELSONREYESContext _context;
 

        public NegocioClienteController(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
         
        }



        // GET: Negocio
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var negocios = _context.Negocios
                .Where(n => n.UsuarioId == Convert.ToInt32(userName)) 
                .Include(x => x.Usuario)
                .AsQueryable();

          
            var pagedNegocios = await negocios.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedNegocios);
        }

    }
}
