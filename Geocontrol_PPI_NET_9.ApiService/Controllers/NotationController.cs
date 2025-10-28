using Dapper;
using Geocontrol_PPI_NET_9.Models.Notations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Geocontrol_PPI_NET_9.ApiService.Controllers
{
    public class NotationController : Controller
    {

        #region Attributes
        private readonly SqlConnection _connection;
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Controlador de usuarios
        /// </summary>
        /// <param name="connection"></param>
        public NotationController(SqlConnection connection)
        {
            /// Assign the first parameter to the private attribute
            _connection = connection;
        }


        [HttpGet("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(
        [FromQuery] int? identification,
        [FromQuery] int? zone_id,
        [FromQuery] string? start_date,
        [FromQuery] string? finish_date)
        {
            try
            {
                var parameters = new
                {
                    Cedula = identification,
                    Zona = zone_id,
                    FechaInicio = start_date,
                    FechaFin = finish_date
                };

                var notations = await _connection.QueryAsync<Notation>(
                    "pa_marcacion_retornar",
                    commandType: CommandType.StoredProcedure,
                    param: parameters
                );

                return Ok(notations.ToList());
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Notation not)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "Consecutivo", not.mar_consecutivoP},
                    { "Cedula", not.marusu_cedula},
                    { "Zona", not.marzonmar_consecutivo},
                    { "Archivo", not.mar_archivo},
                    { "Valida", not.mar_estado},
                    { "Observacion", not.mar_observacion},
                    { "Crear", not.Creation}
                };

                var result = await _connection.QueryAsync<int>("pa_marcacion_crear_editar",
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

    }
}
