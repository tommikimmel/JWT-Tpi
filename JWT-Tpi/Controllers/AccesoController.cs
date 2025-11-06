using JWT_Tpi.Custom;
using JWT_Tpi.Models;
using JWT_Tpi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_Tpi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly JWTTpiContext _context;
        private readonly Utilidades _utilidades;

        public AccesoController(JWTTpiContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario()
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Contraseña = _utilidades.encriptarSHA256(objeto.Contraseña)
            };
            await _context.Usuarios.AddAsync(modeloUsuario);
            await _context.SaveChangesAsync();

            if(modeloUsuario.IdUsuario != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true});
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false});
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _context.Usuarios
                                            .Where(u => u.Correo == objeto.Correo &&
                                                        u.Contraseña == _utilidades.encriptarSHA256(objeto.Contraseña))
                                            .FirstOrDefaultAsync();
            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.GenerarJWT(usuarioEncontrado)});
        }

    }
}
