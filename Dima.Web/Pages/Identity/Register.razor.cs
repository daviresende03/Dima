using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity
{
    public partial class RegisterPage : ComponentBase
    {
        #region Dependencies
        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;

        [Inject]
        public IAccountHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        #endregion

        #region Properties
        public bool IsBusy { get; set; }
        public RegisterRequest InputModel { get; set; } = new();
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if(user.Identity is { IsAuthenticated: true })
                NavigationManager.NavigateTo("/");
        }
        #endregion

        #region Methods
        public async Task OnValueSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await Handler.RegisterAsync(InputModel);
                if(result.IsSuccess)
                    NavigationManager.NavigateTo("/login");
                else
                    SnackBar.Add(result.Message, Severity.Error);
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
