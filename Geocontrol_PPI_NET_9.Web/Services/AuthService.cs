namespace Geocontrol_PPI_NET_9.Web.Services
{
    public class AuthService
    {
        public bool IsLoggedIn { get; private set; } = false;
        public string UserLogged { get; private set; } = string.Empty;
        public int UserType { get; private set; } = 1;

        public event Action OnChange;

        public void Login(string Identification, int type)
        {
            IsLoggedIn = true;
            UserLogged = Identification;
            UserType= type;
            NotifyStateChanged();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            UserLogged = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
