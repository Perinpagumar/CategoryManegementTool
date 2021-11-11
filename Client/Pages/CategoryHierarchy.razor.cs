using Blazored.LocalStorage;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class CategoryHierarchy
    {
        private List<Category> Categories { get; set; } = ApplicationCacheService.AllCategories;

        private string List { get; set; } = "All";

        private string Search { get; set; }
        private bool IsNotMain { get; set; } = false;
        private bool IsDeleted { get; set; } = false;
        private bool IsCompare { get; set; } = false;

        private readonly string newRootCategory = "null";

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await UpdateLocalStorage();
            await ReadLocalStorage();
            Categories = ApplicationCacheService.AllCategories;
            RenderWholePage();
        }

        private async Task ReadLocalStorage()
        {
            ApplicationCacheService.OriginalCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("original"));
            ApplicationCacheService.AllCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("all"));
            ApplicationCacheService.EditedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("edited"));
            ApplicationCacheService.AddedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("added"));
            ApplicationCacheService.DeletedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("deleted"));
        }

        private async Task UpdateLocalStorage()
        {
            if (string.IsNullOrEmpty(await localStore.GetItemAsync<string>("original")))
            {
                await localStore.SetItemAsync("original", "[]");
            }
            if (string.IsNullOrEmpty(await localStore.GetItemAsync<string>("all")))
            {
                await localStore.SetItemAsync("all", "[]");
            }
            if (string.IsNullOrEmpty(await localStore.GetItemAsync<string>("edited")))
            {
                await localStore.SetItemAsync("edited", "[]");
            }
            if (string.IsNullOrEmpty(await localStore.GetItemAsync<string>("added")))
            {
                await localStore.SetItemAsync("added", "[]");
            }
            if (string.IsNullOrEmpty(await localStore.GetItemAsync<string>("deleted")))
            {
                await localStore.SetItemAsync("deleted", "[]");
            }
        }

        private async void ClearLocalStorage()
        {
            await localStore.ClearAsync();
            await OnInitializedAsync();
        }

        private List<Category> GetRootCategories()
        {
            return Categories.Where(category => category.ParentCategoryId == null).ToList();
        }

        private bool _shouldRenderer { get; set; }
        protected override bool ShouldRender()
        {
            return _shouldRenderer || base.ShouldRender();
        }

        public void RenderWholePage()
        {
            _shouldRenderer = true;
            StateHasChanged();
        }

        private bool GetIsNotMain() 
        {
            if(List == "Compare")
            {
                return true;
            }
            return !IsNotMain;
        }

        private void ChageList()
        {
            switch (List) 
            {
                case "All":
                    Categories = ApplicationCacheService.AllCategories;
                    IsDeleted = false;
                    IsNotMain = false;
                    IsCompare = false;
                    break;

                case "Edited":
                    Categories = ApplicationCacheService.EditedCategories;
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Added":
                    Categories = ApplicationCacheService.AddedCategories;
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Deleted":
                    Categories = ApplicationCacheService.DeletedCategories;
                    IsDeleted = true;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Compare":
                    Categories = new();
                    Categories.AddRange(ApplicationCacheService.AllCategories);
                    Categories.AddRange(ApplicationCacheService.DeletedCategories);
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = true;
                    break;
            }
            RenderWholePage();
        }

        private void FilterCategoies() 
        {
            if(Search == string.Empty)
            {
                IsNotMain = false;
                ChageList();
            }
            else
            {
                switch (List)
                {
                    case "All":
                        Categories = ApplicationCacheService.AllCategories
                            .Where(category => category.SerchAllData(Search) == true)
                            .ToList();
                        break;

                    case "Edited":
                        Categories = ApplicationCacheService.EditedCategories
                            .Where(category => category.SerchAllData(Search) == true)
                            .ToList();
                        break;

                    case "Added":
                        Categories = ApplicationCacheService.AddedCategories
                            .Where(category => category.SerchAllData(Search) == true)
                            .ToList();
                        break;

                    case "Deleted":
                        Categories = ApplicationCacheService.DeletedCategories
                            .Where(category => category.SerchAllData(Search) == true)
                            .ToList();
                        break;
                }
                IsNotMain = true;
                base.OnInitialized();
                RenderWholePage();
            }
        }
    }
}
