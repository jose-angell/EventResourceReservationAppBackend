using EventResourceReservationApp.Application.DTOs.Auth;
using EventResourceReservationApp.Application.Services;
using EventResourceReservationApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }
        // ------------------------------------------------------------------
        // MÉTODO DE REGISTRO
        // ------------------------------------------------------------------
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Crear el objeto ApplicationUser
            var user = new ApplicationUser
            {
                UserName = model.Email, // Usamos Email como UserName por simplicidad
                Email = model.Email,
                FirsName = model.FirstName,
                LastName = model.LastName,
                SecondLastName = model.SecondLastName
            };

            // 2. Crear el usuario en Identity (hashing de la contraseña)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Error al registrar usuario: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            // 3. Asignar el Rol por Defecto (CLIENT)
            await _userManager.AddToRoleAsync(user, "Client");
            //if (await _userManager.AddToRoleAsync(user, "Client"))
            //{
            //    _logger.LogInformation("Usuario {Email} registrado y asignado al rol 'Client'.", user.Email);
            //}
            //else
            //{
            //    // Manejar error si no se pudo asignar el rol (ej: el rol no existe)
            //    _logger.LogError("Error al asignar rol 'Client' al usuario {Email}.", user.Email);
            //    // NOTA: Podrías optar por eliminar el usuario aquí si la asignación de rol es crítica.
            //}

            // Opcional: Iniciar sesión y devolver el token inmediatamente
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return Ok(new AuthResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                Token = token,
                Roles = roles
            });
        }

        // ------------------------------------------------------------------
        // MÉTODO DE LOGIN
        // ------------------------------------------------------------------
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            // 1. Buscar usuario por email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Evita enumeración de usuarios: No decimos si el email o la contraseña es incorrecta
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            // 2. Verificar contraseña (usa PasswordHasher de Identity)
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            // 3. Obtener Roles y generar Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            _logger.LogInformation("Usuario {Email} inició sesión correctamente.", user.Email);

            // 4. Devolver la respuesta (token y datos básicos del usuario)
            return Ok(new AuthResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                Token = token,
                Roles = roles
            });
        }

    }
}
