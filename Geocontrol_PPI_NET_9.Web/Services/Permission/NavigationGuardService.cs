
using Geocontrol_PPI_NET_9.Web.Services.Permissions;
using Microsoft.AspNetCore.Components;

namespace Geocontrol_PPI_NET_9.Web.Services.Permission
{
    public class NavigationGuardService
    {
        private readonly NavigationManager navigation;
        private readonly PermissionService permissionService;

        public string CurrentRoute { get; private set; }

        public event Action OnChange;

        public NavigationGuardService(NavigationManager navigation, PermissionService permissionService)
        {
            this.navigation = navigation;
            this.permissionService = permissionService;
        }

        public void NavigateTo(string route)
        {
            try
            {
                var (canNavigate, navName) = permissionService.HasAccess(route);
                CurrentRoute = navName;

                if (canNavigate)
                {
                    navigation.NavigateTo(route);
                }
                else
                {
                    navigation.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }

        }
    }

}
