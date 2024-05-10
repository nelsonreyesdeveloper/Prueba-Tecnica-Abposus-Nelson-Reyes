using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenticationProyect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace AuthenticationProyect.Controllers
{
    [Authorize(Roles = "CLIENTE")]
    public class ItemsController : Controller
    {

        private readonly PRUEBATECNICANELSONREYESContext _context;

        public ItemsController(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(int? page, string searchName, string searchCategory)
        {
            int pageNumber = page ?? 1;
            int pageSize = 20;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var itemsQuery = _context.Items
                .Where(i => i.Negocio.UsuarioId == Convert.ToInt32(userId))
                .Include(i => i.Categoria)
                .Include(i => i.Negocio).
                AsQueryable();

          
            if (!string.IsNullOrEmpty(searchName))
            {
                itemsQuery = itemsQuery.Where(i => i.NombreItem.ToLower().Contains(searchName.Trim().ToLower()));  // Trim and perform case-insensitive search
            }

            if (!string.IsNullOrEmpty(searchCategory))
            {
                itemsQuery = itemsQuery.Where(i => i.Categoria.Id == Convert.ToInt32(searchCategory));
            }

            var items = await itemsQuery.ToPagedListAsync(pageNumber, pageSize);

           
             ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NombreCategoria");
                
            

            return View(items);
        }
        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Categoria)
                .Include(i => i.Negocio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NombreCategoria");
            ViewData["NegocioId"] = new SelectList(_context.Negocios.Where(n => n.UsuarioId == Convert.ToInt32(userName))
                .Include(x => x.Usuario), "Id", "NombreNegocio");
            return View();
        }

        // POST: Items/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreItem,Descripcion,CategoriaId,Precio,NegocioId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NombreCategoria");
            ViewData["NegocioId"] = new SelectList(_context.Negocios.Where(n => n.UsuarioId == Convert.ToInt32(userName))
                .Include(x => x.Usuario), "Id", "NombreNegocio");


            return View();
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NombreCategoria");
            ViewData["NegocioId"] = new SelectList(_context.Negocios.Where(n => n.UsuarioId == Convert.ToInt32(userName))
                .Include(x => x.Usuario), "Id", "NombreNegocio");

            var items= await _context.Items
               .Include(i => i.Categoria)
               .Include(i => i.Negocio)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(item);
           
        }

        // POST: Items/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreItem,Descripcion,CategoriaId,Precio,NegocioId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NombreCategoria");
            ViewData["NegocioId"] = new SelectList(_context.Negocios.Where(n => n.UsuarioId == Convert.ToInt32(userName))
                .Include(x => x.Usuario), "Id", "NombreNegocio");
            return View();
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Categoria)
                .Include(i => i.Negocio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'PRUEBATECNICANELSONREYESContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return _context.Items.Any(e => e.Id == id);
        }
    }
}
