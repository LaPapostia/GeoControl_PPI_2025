using Dapper;
using Geocontrol_PPI_NET_9.Models;
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

        [HttpPost]
        [Route("api/[controller]/ChangePassword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] NewPassword newPassword)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Identificacion", newPassword.usu_identification},
                    { "Contrasenia", newPassword.usu_contrasenia_nueva}
                };

                var result = await _connection.QueryAsync<bool>("pa_usuario_cambiar_contrasenia",
                    commandType: CommandType.StoredProcedure,
                    param: parameters);

                return Ok(new StandardResponse { result = true });
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
