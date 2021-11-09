using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
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

        private readonly List<string> Validation = new(new string[]{
            "General Validation",
            "LanguageEntries must contain Languages: German and English!",
            "LanguageEntry Text can't be empty!",
            "LanguageEntry Language can't be Undefined!",
            "LanguageEntries Language each can't be used more than once, except in PossibleValues",
            "___",
            "If Attributes not empty:",
            "RegexDescriptions can't be emty if ValidationRegex is not null!",
            "PresentatioType can't be Undefined"
        });

        protected override async Task OnInitializedAsync()
        {
            ApplicationCacheService.AllCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("all"));
            ApplicationCacheService.EditedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("edited"));
            ApplicationCacheService.AddedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("added"));
            ApplicationCacheService.SelectedCategory = new();
            RenderWholePage();

            if (CategoryId == null)
            {
                if (ParrentCategoryId == "null") 
                {
                    ParrentCategoryId = null;
                }
                ApplicationCacheService.SelectedCategory.Id = await GetObjectId();
                ApplicationCacheService.SelectedCategory.ParentCategoryId = ParrentCategoryId;
                IsAdd = true;
            }
            else
            {
                var category = ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .First();
                if (category != null)
                {
                    ApplicationCacheService.SelectedCategory = category.Clone();
                    foreach (var a in ApplicationCacheService.SelectedCategory.CategoryAttributes ?? new List<CategoryAttribute>())
                    {
                        if (a.PossibleValues == null)
                        {
                            a.PossibleValues = new();
                        }

                        if (a.LanguageEntries == null)
                        {
                            a.LanguageEntries = new();
                        }

                        if (a.RegexDescriptions == null)
                        {
                            a.RegexDescriptions = new();
                        }
                    }
                }
            }

            NotValidDialogShow = true;
            RenderWholePage();
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
                .Where(category => category.Id.ToString() == Category.ParentCategoryId)
                .Select(category => category.CategoryAttributes)
                .ToList();
        }

        private void ToParentCategory() 
        {
            if(!IsAdd && Category.ParentCategoryId != null)
            {
                NavigationManager.NavigateTo("/categories/edit/" + Category.ParentCategoryId);
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

        private async Task SaveChangesAsync()
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
                        .First();
                    ApplicationCacheService.AllCategories.Remove(category);
                    ApplicationCacheService.AllCategories.Add(Category);


                    var editedCategory = ApplicationCacheService.EditedCategories
                    .Where(category => category.Id == Category.Id)
                    .ToList();
                    if (editedCategory.Count() > 0)
                    {
                        ApplicationCacheService.EditedCategories.Remove(editedCategory.First());

                    }
                    ApplicationCacheService.EditedCategories.Add(Category);

                    var addedCategory = ApplicationCacheService.AddedCategories
                        .Where(category => category.Id == Category.Id)
                        .ToList();
                    if (addedCategory.Count() > 0)
                    {
                        ApplicationCacheService.AddedCategories.Remove(addedCategory.First());
                        ApplicationCacheService.AddedCategories.Add(Category);
                    }
                }
                await localStore.SetItemAsync("all", JsonConvert.SerializeObject(ApplicationCacheService.AllCategories));
                await localStore.SetItemAsync("edited", JsonConvert.SerializeObject(ApplicationCacheService.EditedCategories));
                await localStore.SetItemAsync("added", JsonConvert.SerializeObject(ApplicationCacheService.AddedCategories));
                NavigationManager.NavigateTo("/");
            }
            else
            {
                TriggerDialog();
                RenderWholePage();
            }
        }

        private void CancelChanges() 
        {
            NavigationManager.NavigateTo("/");
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
                    await GetObjectId();
                }
            }

            return id;
        }
    }
}
