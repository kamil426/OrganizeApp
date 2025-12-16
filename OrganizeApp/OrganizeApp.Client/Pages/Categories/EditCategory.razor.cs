using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Category.Commands;

namespace OrganizeApp.Client.Pages.Categories
{
    public partial class EditCategory
    {
        private EditCategoryCommand _category = new EditCategoryCommand();

        private bool _isLoading = false;
        private bool _isPageLoading = true;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public ICategoryHttpRepository CategoryHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterBeforeSendAsyncEvent();
            Interceptor.RegisterAfterSendAsyncEvent();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshLoginStatusService.RefreshLoginHeader();
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                _category = await CategoryHttpRepository.GetEditCategory(Id, authState.GetUserId());
                _isPageLoading = false;

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Edit(AuthenticationState authState)
        {
            try
            {
                _isLoading = true;
                _category.UserId = authState.GetUserId();
                await CategoryHttpRepository.Edit(_category);
                NavigationManager.NavigateTo("/categories");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ReturnToMyCategories()
        {
            NavigationManager.NavigateTo("/categories");
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
    }
}
