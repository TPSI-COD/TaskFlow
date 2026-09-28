using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel dados)
        {
            if (!ModelState.IsValid)
            {
                return View(dados);
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == dados.Email);

            if (usuario == null)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");

                return View(dados);
            }

            bool senhaValida = BCrypt.Net.BCrypt.Verify(
                dados.Senha,
                usuario.SenhaHash
            );

            if (!senhaValida)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");

                return View(dados);
            }

                        var claims = new List<Claim>
        {
    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
    new Claim(ClaimTypes.Name, usuario.Nome),
    new Claim(ClaimTypes.Email, usuario.Email)
};

    var identidade = new ClaimsIdentity(
    claims,
    CookieAuthenticationDefaults.AuthenticationScheme
);

    var propriedades = new AuthenticationProperties
    {
    IsPersistent = false
};

    await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    new ClaimsPrincipal(identidade),
    propriedades
);

    return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
    public IActionResult Cadastrar(CadastroUsuarioViewModel dados)
    {
    if (!ModelState.IsValid)
    {
        return View(dados);
    }

    var usuarioExistente = _context.Usuarios
        .FirstOrDefault(u => u.Email == dados.Email);

    if (usuarioExistente != null)
    {
        ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

        return View(dados);
    }

    var usuario = new Usuario
    {
        Nome = dados.Nome,
        Email = dados.Email,
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(dados.Senha),
        DataCadastro = DateTime.Now
    };

    _context.Usuarios.Add(usuario);

    _context.SaveChanges();

    return RedirectToAction("Index", "Home");
}
            [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToAction("Index", "Home");
    }

    }
}