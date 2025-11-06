using Dapper;
using Geocontrol_PPI_NET_9.Models;
using Geocontrol_PPI_NET_9.Models.Auth;
using Geocontrol_PPI_NET_9.Models.Notations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Geocontrol_PPI_NET_9.ApiService.Controllers
{
    public class AuthController : Controller
    {
        #region Attributes
        private readonly SqlConnection _connection;
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Controlador de autorización de códigos
        /// </summary>
        /// <param name="connection"></param>
        public AuthController(SqlConnection connection)
        {
            /// Assign the first parameter to the private attribute
            _connection = connection;
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
                return Ok(new AuthResult() { Result = resultAuth.FirstOrDefault() });
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
