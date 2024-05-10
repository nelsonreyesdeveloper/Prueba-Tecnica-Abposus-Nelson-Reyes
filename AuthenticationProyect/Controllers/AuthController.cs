using Microsoft.AspNetCore.Mvc;
using AuthenticationProyect.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationProyect.Controllers
{
    public class AuthController : Controller
    {

        private readonly PRUEBATECNICANELSONREYESContext _DBContext;

        public AuthController(PRUEBATECNICANELSONREYESContext context)
        {
            _DBContext = context;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {

                if (User.IsInRole("ADMINISTRADOR"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Items");
                }

            }
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Index(User _user)
        {

            var _usuario = await _DBContext.Users.Include(x => x.Tipo).FirstOrDefaultAsync(x => x.Correo == _user.Correo);

            if (_usuario != null && BCrypt.Net.BCrypt.Verify(_user.Contrasenia, _usuario.Contrasenia))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, _usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, _usuario.Nombres),
                    new Claim("Email", _usuario.Correo),
                    new Claim(ClaimTypes.Role, _usuario.Tipo.NombreTipoUsuario)

                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                                                          CookieAuthenticationDefaults.AuthenticationScheme,
                                                                                                                   new ClaimsPrincipal(claimsIdentity),
                                                                                                                                                                                               authProperties);
                string _role = _usuario.Tipo.NombreTipoUsuario;

                if (_role == "ADMINISTRADOR")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Items");
                }

            }

            else
            {
                ViewBag.Error = "Usuario o contraseña incorrecta";
            }

            

            return View();

        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("Index", "Auth");
        }
    }
}
