using Geocontrol_PPI_NET_9.Models.Routes;

namespace Geocontrol_PPI_NET_9.Web.Services.Permissions
{
    public class PermissionService
    {
        AuthService authService;

        public PermissionService(AuthService authService)
        {
            this.authService = authService;
        }

        // Simulación de permisos por ruta
        private readonly List<RouteModel> allowedRoutes =
        [
            new RouteModel()
            {
                Path = "/map",
                Name = "Mapa",
                Icon = "fas fa-map"
            },
            new RouteModel()
            {
                Path = "/profile",
                Name = "Perfil",
                Icon = "fas fa-user"
            },
            new RouteModel()
            {
                Path = "/users",
                Name = "Usuarios",
                Icon = "fas fa-users"
            },

            new RouteModel()
            {
                Path = "/reports",
                Name = "Reportes",
                Icon = "fas fa-reports"
            }
        ];

        public (bool canNavigate, string navName) HasAccess(string route)
        {
            try
            {
                if (route.Equals("/"))
                    return (true, "Inicio");

                var findRoute = allowedRoutes.FirstOrDefault(x => x.Path.Equals(route));

                return findRoute != null && authService.IsLoggedIn
                    ? (true, findRoute.Name)
                    : (false, string.Empty);

            }
            catch (Exception)
            {
                throw;
            }

        }
    }

}
