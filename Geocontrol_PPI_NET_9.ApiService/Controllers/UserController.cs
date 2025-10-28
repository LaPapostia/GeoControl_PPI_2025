using Dapper;
using Geocontrol_PPI_NET_9.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Geocontrol_PPI_NET_9.ApiService.Controllers
{
    public class UserController : Controller
    {
        #region Attributes
        private readonly SqlConnection _connection;
        #endregion


        /// <summary>
        /// Controlador de usuarios
        /// </summary>
        /// <param name="connection"></param>
        public UserController(SqlConnection connection)
        {
            /// Assign the first parameter to the private attribute
            _connection = connection;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("api/Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Autenticate([FromBody] Authentication request)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Cedula", request.Identification },
                    { "Contrasenia", request.Password}
                };

                var resultAuth = await _connection.QueryAsync<bool>(
                    "pa_validar_usuario",
                    commandType: CommandType.StoredProcedure,
                    param: parameters
                );
                return Ok(new AuthResult () { Result = resultAuth.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        [HttpGet]
        [Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _connection.QueryAsync<User>(
                    "pa_usuario_retornar",
                    commandType: CommandType.StoredProcedure
                );
                return Ok(users.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        [HttpGet("api/[controller]/{identification}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByIdentification(int identification)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "usu_cedulaP", identification}
                };

                var users = await _connection.QueryAsync<User>(
                    "pa_usuario_retornar_detalle",
                    commandType: CommandType.StoredProcedure,
                    param: parameters
                );
                return Ok(users.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostUser([FromBody] User userRequest)
        {
            try
            {
                var parameters = new
                {
                    Cedula = userRequest.usu_cedulaP,
                    Nombres = userRequest.usu_nombres,
                    Apellidos = userRequest.usu_apellidos,
                    Correo = userRequest.usu_correo,
                    Contrasenia = userRequest.usu_contrasenia,
                    Tipo = userRequest.usu_tipo,
                    AplicaZona = userRequest.usu_aplica_zona,
                    Activo = userRequest.usu_estado,
                    Crear = userRequest.Creation
                };

                var users = await _connection.QueryAsync<int>(
                    "pa_usuario_crear_editar",
                    commandType: CommandType.StoredProcedure,
                    param: parameters
                );

                return Ok(new { message = "Operación ejecutada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        [HttpDelete("api/[controller]/${identification}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int identification)
        {
            try
            {
                var parameters = new
                {
                    Cedula = identification
                };

                var users = await _connection.QueryAsync<int>(
                    "pa_usuario_eliminar",
                    commandType: CommandType.StoredProcedure,
                    param: parameters
                );

                return Ok(new { message = "Operación ejecutada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }
    }
}
