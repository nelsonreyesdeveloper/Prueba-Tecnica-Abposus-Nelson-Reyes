using AuthenticationProyect.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProyect.Controllers
{

    [Authorize(Roles = "ADMINISTRADOR")]
    public class HomeController : Controller
    {
        private readonly PRUEBATECNICANELSONREYESContext _context;

      
        public HomeController(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
        }


    
        public async Task<IActionResult> Index(int? page, string searchforname)
        {
            int pageNumber = page ?? 1;
            int pageSize = 20;

            var negociosQuery = _context.Negocios.Include(x=> x.Usuario).AsQueryable(); 

            if (!string.IsNullOrEmpty(searchforname))
            {
                negociosQuery = negociosQuery.Where(n => n.NombreNegocio.Contains(searchforname));
            }

            var pagedNegocios = await negociosQuery.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedNegocios);
        }


        
        public async Task<IActionResult> GetNegocios(int page = 1, string searchTerm = "")
        {
            int pageNumber = page;
            int pageSize = 20;
            var negociosQuery = _context.Negocios.Include(x => x.Usuario).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                negociosQuery = negociosQuery.Where(n => n.NombreNegocio.Contains(searchTerm));
            }

            var pagedNegocios = await negociosQuery.ToPagedListAsync(pageNumber, pageSize);

          
            var negociosList = pagedNegocios.ToList(); 
            var negociosData = negociosList.Select(n => new
            {
                id = n.Id,
                nombreNegocio = n.NombreNegocio,
                usuario = n.Usuario != null ? n.Usuario.NombreCompleto : "SIN DUEÑO",
                descripcion = n.Descripcion,
                fechaCreacion = n.FechaCreacion,
            });

            int pageCount = pagedNegocios.PageCount;
            int pageNumberList = pagedNegocios.PageNumber;

       
            var responseData = new
            {
                negociosData = negociosData,
                pageCount = pageCount,
                pageNumberList = pageNumberList

            };


            return Json(responseData); // Return JSON data
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
