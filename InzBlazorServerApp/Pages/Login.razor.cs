using Inz.DTOModel;
using Inz.Model;
using Microsoft.AspNetCore.Components;

namespace InzBlazorServerApp.Pages
{
    public partial class Login
    {
        [Inject]
        private TypedHttpClient TypedHttpClient { get; set; }
        private string LoginUsername { get; set; } = null!;
        private string Password { get; set; } = null!;
        private bool IsPatient { get; set; }
        private string Token { get; set; } = null!;
        private bool IsLoading { get; set; } = false;

        private async Task GetToken(string login, string password, bool isDoctor)
        {
            IsLoading = true;
            LoginDTO loginDTO = new LoginDTO()
            {
                Login = login,
                Password = password,
                PersonType = isDoctor ? PersonType.Doctor : PersonType.Patient
            };

            Token = await TypedHttpClient.GetLoginToken(loginDTO);
            IsLoading = false;
        }

    }
}
