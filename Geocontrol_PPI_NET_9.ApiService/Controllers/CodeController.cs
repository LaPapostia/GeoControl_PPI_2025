using Dapper;
using Geocontrol_PPI_NET_9.Models;
using Geocontrol_PPI_NET_9.Models.Auth;
using Geocontrol_PPI_NET_9.Models.Notations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Geocontrol_PPI_NET_9.ApiService.Controllers
{
    public class CodeController : Controller
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
        public CodeController(SqlConnection connection)
        {
            /// Assign the first parameter to the private attribute
            _connection = connection;
        }


        [HttpPost]
        [Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AuthCode authCode)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Identificacion", authCode.codusu_cedula},
                    { "Codigo", authCode.cod_code},
                    { "Vencimiento", authCode.cod_fecha_expiracion}
                };

                var result = await _connection.QueryAsync<int>("pa_codigo_auth_ingresar",
                    commandType: CommandType.StoredProcedure,
                    param: parameters);

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
        [Route("api/[controller]/Validate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateCode([FromBody] AuthCode authCode)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Identificacion", authCode.codusu_cedula},
                    { "Codigo", authCode.cod_code}
                };

                var result = await _connection.QueryAsync<bool>("pa_codigo_auth_validar",
                    commandType: CommandType.StoredProcedure,
                    param: parameters);

                return Ok(new StandardResponse { result = result.FirstOrDefault() });
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
