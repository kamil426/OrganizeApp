using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OrganizeApp.Client.Components;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Category.Dtos;
using OrganizeApp.Shared.Common.Models;

namespace OrganizeApp.Client.Pages.Categories
{
    public partial class Categories
    {
        private static IComponentRenderMode _rendermode = new InteractiveAutoRenderMode(prerender: false);
        private bool _isLoading = true;
        private IList<CategoryDto> _categoriesList;
        private bool _isDeleteTrybeEnabled = false;
        private List<int> _deletingCategoriesId = new();
        private IJSObjectReference _jsModule;
        private Modal _modalDeleteCategories;

        [Inject]
        public ICategoryHttpRepository CategoryHttpRepository { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

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
                await RefreshLoginStatusService.RefreshLoginHeader("/categories");
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
                await RefreshCategories();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RefreshCategories()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated is false)
            {
                NavigationManager.NavigateTo("/login");
                return;
            }
            _categoriesList = await CategoryHttpRepository.GetCategories(authState.GetUserId());
            _isLoading = false;
            StateHasChanged();
        }

        private void Delete(MouseEventArgs args)
        {
            if (_isDeleteTrybeEnabled && _deletingCategoriesId.Count > 0)
            {
                _modalDeleteCategories.Open();
                return;
            }
            _isDeleteTrybeEnabled = true;
        }

        private void AddCategoriesToListDeletingItems(int id, ChangeEventArgs e)
        {
            if (Convert.ToBoolean(e.Value))
            {
                _deletingCategoriesId.Add(id);
                return;
            }

            var uncheckTask = _deletingCategoriesId.SingleOrDefault(x => x == id);
            if (uncheckTask is 0)
                return;

            _deletingCategoriesId.Remove(uncheckTask);
        }

        private async void DeleteCategories(AuthenticationState authState)
        {
            if (_deletingCategoriesId.Count > 0)
                foreach (var categoryId in _deletingCategoriesId)
                {
                    await CategoryHttpRepository.Delete(categoryId, authState.GetUserId());
                    var taskToDelete = _categoriesList.SingleOrDefault(x => x.Id == categoryId);
                    if (taskToDelete is not null)
                        _categoriesList.Remove(taskToDelete);
                }
            _modalDeleteCategories.Close();
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        private void AddCategory(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/categories/add");
        }

        private void EditCategory(int categoryId)
        {
            NavigationManager.NavigateTo($"/categories/edit/{categoryId}");
        }

        private void CancelDelete()
        {
            _modalDeleteCategories.Close();
        }

        private async void CancelDeleteTrybe()
        {
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
            _deletingCategoriesId.Clear();
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
    }
}
