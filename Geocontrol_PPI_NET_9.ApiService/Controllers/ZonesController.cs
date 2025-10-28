using Dapper;
using Geocontrol_PPI_NET_9.Models.Auth;
using Geocontrol_PPI_NET_9.Models.Notations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Geocontrol_PPI_NET_9.ApiService.Controllers
{
    public class ZonesController : Controller
    {
        #region Attributes
        private readonly SqlConnection _connection;
        #endregion


        /// <summary>
        /// Controlador de usuarios
        /// </summary>
        /// <param name="connection"></param>
        public ZonesController(SqlConnection connection)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var NotationZones = await _connection.QueryAsync<Zone>(
                    "pa_zonas_marcacion_retornar",
                    commandType: CommandType.StoredProcedure
                );
                return Ok(NotationZones.ToList());
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

        [HttpGet("api/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var NotationZones = await _connection.QueryAsync<Zone>(
                    "pa_zonas_marcacion_retornar_detalle",
                    commandType: CommandType.StoredProcedure,
                    param: new { zonmar_consecutivoP = id }
                );
                return Ok(NotationZones.FirstOrDefault());
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
        public async Task<IActionResult> Post([FromBody] Zone zm)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
            {
                { "zonmar_nombre", zm.zonmar_nombre},
                { "zonmar_coordenadas", zm.zonmar_coordenadas},
                { "zonmar_hora_inicio", zm.zonmar_hora_inicio},
                { "zonmar_hora_fin", zm.zonmar_hora_fin},
                { "zonmar_observacion", zm.zonmar_observacion},
                { "zonmar_estado", zm.zonmar_estado}
            };

                var result = await _connection.QueryAsync<int>("pa_zonas_marcacion_ingresar",
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

        [HttpPut]
        [Route("api/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] Zone zm)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "zonmar_nombre", zm.zonmar_nombre},
                    { "zonmar_coordenadas", zm.zonmar_coordenadas},
                    { "zonmar_hora_inicio", zm.zonmar_hora_inicio},
                    { "zonmar_hora_fin", zm.zonmar_hora_fin},
                    { "zonmar_observacion", zm.zonmar_observacion},
                    { "zonmar_estado", zm.zonmar_estado}
                };

                var result = await _connection.QueryAsync<int>("pa_zonas_marcacion_actualizar",
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

        [HttpDelete("api/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>()
                {
                    { "zonmar_consecutivoP", id}
                };

                var result = await _connection.QueryAsync<int>("pa_zonas_marcacion_eliminar",
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
