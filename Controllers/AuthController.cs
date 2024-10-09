using futapi.Data;
using futapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace futapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FutblogContext _context;

        public AuthController(FutblogContext context)
        {
            _context = context;
        }

        // Registro de nuevos usuarios
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            // Verificar si el usuario ya existe
            if (await _context.Users.AnyAsync(u => u.Email == userRegisterDto.Email))
            {
                return BadRequest(new {message = "Este correo ya está en uso.", success = false});
            }

            var newUser = new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password), // Hashea la contraseña
                Role = "User" // Rol por defecto
            };

            // Agregar el nuevo usuario a la base de datos
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Usuario registrado con éxito.", success = true});
        }

        // Inicio de sesión
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            // Buscar el usuario en la base de datos por su correo
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);

            // Verificar si el usuario existe
            if (dbUser == null)
            {
                return Unauthorized(new {message = "Credenciales incorrectas.", success = false});
            }

            // Verificar la contraseña usando BCrypt
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, dbUser.Password);

            // Verificar si la contraseña es válida
            if (!isPasswordValid)
            {
                return Unauthorized(new {message = "Credenciales incorrectas.", success = false});
            }

            return Ok(new {message = "Inicio de sesión exitoso.", success = true});
        }
    }
}
