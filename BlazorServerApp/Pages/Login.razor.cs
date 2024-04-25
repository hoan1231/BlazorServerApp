using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using BlazorServerApp.Services;

namespace BlazorServerApp.Pages
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
    public partial class Login
    {
        [CascadingParameter]
        public EventCallback notify { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILocalStorageService _localStorageService { get; set; }
        [Inject] private IPackageBzService packageBzService { get; set; }

        public LoginModel login = new LoginModel { UserName="",PassWord="113333" };
        private bool loading = false;
        private string Error = "66666";
        async Task LoginUser(EditContext editContext)
        {
            loading = true;
            var result = await packageBzService.GetInfoCustomer(login.UserName, login.PassWord);
            if(result!= null && result.UserName==login.UserName)
            {
                await _localStorageService.SetItemAsync("authToken", "Success");
                await _localStorageService.SetItemAsync("userName", result.UserName);
                await _localStorageService.SetItemAsync("userId", result.UserId);
                await notify.InvokeAsync();
                loading = false;
                NavigationManager.NavigateTo("/packagedata");
            }
            else
            {
                loading = false;
                Error = "Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng thử lại";
            }
            //isLogin = true;


        }
    }
}
