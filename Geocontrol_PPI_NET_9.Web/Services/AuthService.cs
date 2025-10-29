namespace Geocontrol_PPI_NET_9.Web.Services
{
    public class AuthService
    {
        public bool IsLoggedIn { get; private set; } = false;

        public event Action OnChange;

        public void Login()
        {
            IsLoggedIn = true;
            NotifyStateChanged();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
