using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Task.Commands;

namespace OrganizeApp.Client.Pages.Categories
{
    public partial class AddCategory
    {
        private AddCategoryCommand _category = new AddCategoryCommand();

        private bool _isPageLoading = true;
        private bool _isLoading = false;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Inject]
        public ICategoryHttpRepository CategoryHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshLoginStatusService.RefreshLoginHeader("/category/add");
                _isPageLoading = false;
                _category.Color = "rgb(255, 255, 255)";
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterBeforeSendAsyncEvent();
            Interceptor.RegisterAfterSendAsyncEvent();
            await base.OnInitializedAsync();
        }

        private async Task Add(AuthenticationState authContext)
        {
            try
            {
                _isLoading = true;
                _category.UserId = authContext.GetUserId();
                await CategoryHttpRepository.Add(_category);
                NavigationManager.NavigateTo("/categories");
            }
            finally
            {
                _isLoading = false;
            }
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
        private void Return(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/categories");
        }
    }
}
