using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenticationProyect.Models;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace AuthenticationProyect.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class NegocioController : Controller
    {
        private readonly PRUEBATECNICANELSONREYESContext _context;

        public NegocioController(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
        }

        // GET: Negocio
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var negocios = _context.Negocios.Include(x => x.Usuario);
            var pagedNegocios = await negocios.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedNegocios);
        }


        // GET: Negocio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Negocios == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios.Include(x => x.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negocio == null)
            {
                return NotFound();
            }

            return View(negocio);
        }

        // GET: Negocio/Create
        public IActionResult Create()
        {

            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "NombreCompleto");
            return View();
        }

        // POST: Negocio/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreNegocio,Descripcion,FechaCreacion,UsuarioId")] Negocio negocio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(negocio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "NombreCompleto", negocio.UsuarioId);
            return View(negocio);
        }

        // GET: Negocio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Negocios == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios.FindAsync(id);
            if (negocio == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "NombreCompleto", negocio.UsuarioId);
            return View(negocio);
        }

        // POST: Negocio/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreNegocio,Descripcion,FechaCreacion,UsuarioId")] Negocio negocio)
        {
            if (id != negocio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(negocio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NegocioExists(negocio.Id))
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

            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "NombreCompleto", negocio.UsuarioId);
            return View(negocio);
        }

        // GET: Negocio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Negocios == null)
            {
                return NotFound();
            }

            var negocio = await _context.Negocios.Include(x => x.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negocio == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.Users, "Id", "NombreCompleto", negocio.UsuarioId);
            return View(negocio);
        }

        // POST: Negocio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Negocios == null)
            {
                return Problem("Entity set 'PRUEBATECNICANELSONREYESContext.Negocios'  is null.");
            }
            var negocio = await _context.Negocios.FindAsync(id);
            if (negocio != null)
            {
                _context.Negocios.Remove(negocio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NegocioExists(int id)
        {
          return _context.Negocios.Any(e => e.Id == id);
        }
    }
}
