using Dapper;
using Geocontrol_PPI_NET_9.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

    }
}
