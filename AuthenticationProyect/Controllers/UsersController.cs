using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenticationProyect.Models;

using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;



namespace AuthenticationProyect.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsersController : Controller
    {
        private readonly PRUEBATECNICANELSONREYESContext _context;

        public UsersController(PRUEBATECNICANELSONREYESContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1; 
            int pageSize = 10; 

            var users = _context.Users.Include(u => u.Tipo);

            var pagedUsers = await users.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedUsers);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "NombreTipoUsuario");
            return View();
        }

        // POST: Users/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,TipoId,Correo,Contrasenia")] User user)
        {
            if (ModelState.IsValid)
            {

                if (_context.Users.Any(u => u.Correo == user.Correo))
                {
                    ModelState.AddModelError("Correo", "Este correo electrónico ya está registrado.");
                    ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "Id", user.TipoId);
                    return View(user);
                }

                user.Contrasenia = BCrypt.Net.BCrypt.HashPassword(user.Contrasenia);

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "Id", user.TipoId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "NombreTipoUsuario", user.TipoId);
            return View(user);
        }

        // POST: Users/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,TipoId,Correo,Contrasenia")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            var userToUpdate = _context.Users.Find(id);
            if (ModelState.IsValid)
            {
             
                if (userToUpdate.Correo != user.Correo)
                {
                    if (_context.Users.Any(u => u.Correo == user.Correo))
                    {
                        ModelState.AddModelError("Correo", "Este correo electrónico ya está registrado.");
                        ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "Id", userToUpdate.TipoId);
                        return View(userToUpdate);
                    }

                    if (_context.Users.Any(u => u.Correo == user.Correo && u.Id != userToUpdate.Id))
                    {
                        ModelState.AddModelError("Correo", "No puede cambiar su correo electrónico a uno que no le pertenece.");
                        ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "Id", userToUpdate.TipoId);
                        return View(userToUpdate);
                    }
                }

                userToUpdate.Nombres = user.Nombres;
                userToUpdate.Apellidos = user.Apellidos;
                userToUpdate.TipoId = user.TipoId;
                userToUpdate.Correo = user.Correo; 
                _context.Update(userToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userToUpdate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["TipoId"] = new SelectList(_context.UserTipos, "Id", "Id", userToUpdate.TipoId);
            return View(userToUpdate);
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PRUEBATECNICANELSONREYESContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    
            if (user.Id == Convert.ToInt32(userId))
            {
                  
                ModelState.AddModelError("Cuenta", "No puede eliminar su propia cuenta.");
              
                return View(user);

            }

            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
