using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class CategoryEditAndAdd
    {
        [Parameter]
        public string CategoryId { get; set; }

        [Parameter]
        public string ParrentCategoryId { get; set; }

        private bool _shouldRenderer { get; set; }

        private Category Category 
        { 
            get => ApplicationCacheService.SelectedCategory; 
        }

        private bool IsAdd { get; set; } = false;

        private bool NotValidDialogShow { get; set; } = true;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }

    private readonly List<string> Validation = new(new string[]{"LanguageEntries must contain Languages: German and English!",
            "LanguageEntry Text can't be empty!",
            "LanguageEntry Language can't be Undefined!",
            "___",
            "If Attributes not empty:",
            "LanguageEntries must contain Languages: German and English!",
            "LanguageEntry Text can't be empty!",
            "LanguageEntry Language can't be Undefined!",
            "Dropdown Selects can't be Undefined!",
            "RegexDescriptions can't be emty if ValidationRegex is not null!" });

        protected override async Task OnInitializedAsync()
        {
            ApplicationCacheService.SelectedCategory = new();

            if (CategoryId == null)
            {
                if (ParrentCategoryId == "null") 
                {
                    ParrentCategoryId = null;
                }
                ApplicationCacheService.SelectedCategory.Id = await GetObjectId();
                ApplicationCacheService.SelectedCategory.ParrentCategoryId = ParrentCategoryId;
                IsAdd = true;
            }
            else
            {
                var categories = ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .ToList();
                if (categories != null)
                {
                    ApplicationCacheService.SelectedCategory = categories.First().Clone();
                }
            }

            NotValidDialogShow = true;
        }

        protected override bool ShouldRender()
        {
            return _shouldRenderer || base.ShouldRender();
        }

        public void RenderWholePage()
        {
            _shouldRenderer = true;
            StateHasChanged();
        }

        private List<List<CategoryAttribute>> GetParentAttributes()//Prtotype
        {
            return ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == Category.ParrentCategoryId)
                .Select(category => category.CategoryAttributes)
                .ToList();
        }

        private void ToParentCategory() 
        {
            if(!IsAdd)
            {
                NavigationManager.NavigateTo("/categories/edit/" + Category.ParrentCategoryId);
                OnInitializedAsync();
                RenderWholePage();
            }
        }

        private void AddLanguageEntry() 
        {
            ApplicationCacheService.SelectedCategory.LanguageEntries.Add(new LanguageEntry());
        }

        private async Task AddCategoryAttributeAsync() 
        {
            var attribute = new CategoryAttribute();
            attribute.Id = await GetObjectId();
            ApplicationCacheService.SelectedCategory.CategoryAttributes.Add(attribute);
        }

        private void SaveChanges()
        {
            if (Category.IsValid())
            {
                if (IsAdd)
                {
                    ApplicationCacheService.AllCategories.Add(Category);
                    ApplicationCacheService.AddedCategories.Add(Category);
                }
                else
                {
                    var category = ApplicationCacheService.AllCategories
                        .Where(category => category.Id == Category.Id)
                        .ToList().First();
                    ApplicationCacheService.AllCategories.Remove(category);
                    ApplicationCacheService.AllCategories.Add(Category);
                    var editedCategory = ApplicationCacheService.EditedCategories
                        .Where(category => category.Id == Category.Id)
                        .ToList();
                    if(editedCategory.Count() != 0)
                    {
                        ApplicationCacheService.EditedCategories.Remove(editedCategory.First());
                    }
                    ApplicationCacheService.EditedCategories.Add(Category);
                }
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                TriggerDialog();
                RenderWholePage();
            }
        }

        private void CancelChanges() 
        {
            NavigationManager.NavigateTo("/categories");
        }

        private void TriggerDialog()
        {
            NotValidDialogShow = false;
        }

        private void CloseDialog()
        {
            NotValidDialogShow = true;
        }

        private async Task<string> GetObjectId()
        {
            string id = await HttpClient.GetStringAsync("/objectid");
            if(!ApplicationCacheService.AllCategories.Where(c=> c.Id == id).Any())
            {
                if(ApplicationCacheService.AllCategories.Where(c => c.CategoryAttributes.Any(a => a.Id == id)).Any())
                {
                    GetObjectId();
                }
            }

            return id;
        }
    }
}
