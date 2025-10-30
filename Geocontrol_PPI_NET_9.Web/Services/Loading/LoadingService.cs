using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Web.Services.Loading
{
    public class LoadingService
    {
        public bool IsVisible { get; private set; }
        public string Message { get; private set; } = "Cargando...";

        public event Func<Task>? OnChangeAsync;

        public async Task ShowAsync(string? message = null)
        {
            Message = message ?? "Cargando...";
            IsVisible = true;
            await NotifyStateChangedAsync();
        }

        public async Task HideAsync()
        {
            IsVisible = false;
            await NotifyStateChangedAsync();
        }

        private async Task NotifyStateChangedAsync()
        {
            if (OnChangeAsync != null)
                await OnChangeAsync.Invoke();
        }
    }
}
