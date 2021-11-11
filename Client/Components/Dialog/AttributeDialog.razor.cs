using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.Dialog
{
    public partial class AttributeDialog
    {
        [Parameter]
        public CategoryAttribute CategoryAttribute { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        [Parameter]
        public EventCallback OnModalClose { get; set; }

        [Parameter]
        public bool IsView { get; set; }

        [Parameter]
        public bool IsAdd { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }

        private string Title { get; set; } = "Edit Attribute";

        public CategoryAttribute NewCategoryAttribute { get => ApplicationCacheService.NewCategoryAttribute; }

        private CategoryAttribute Clone { get; set; }

        private bool _showValidation { get; set; }

        private readonly List<string> Validation = new(new string[]{
            "LanguageEntries must contain Languages: German and English!",
            "LanguageEntry Text can't be empty!",
            "LanguageEntry Language can't be Undefined!",
            "LanguageEntries Language each can't be used more than once",
            "___",
            "If Validationregex is checked:",
            "Regexdescriptions can't be empty!",
            "Regexdescriptions Language each can't be used more than once",
        });

        protected override async Task OnInitializedAsync()
        {
            if(IsAdd)
            {
                Title = "Add Attribute";
                ApplicationCacheService.NewCategoryAttribute.Id = await GetObjectId();
            }

            if(IsView)
            {
                Title = "View Attribute";
            }

            Clone = CategoryAttribute.Clone();
            await RenderWholePage.InvokeAsync();
        }

        private async Task CloseAsync()
        {
            ResetAttribute();
            ApplicationCacheService.NewCategoryAttribute = new();
            await OnModalClose.InvokeAsync();
        }

        private async Task SaveAsync()
        {
            if(CategoryAttribute.IsValid())
            {
                if(IsAdd)
                {
                    ApplicationCacheService.SelectedCategory.CategoryAttributes.Add(CategoryAttribute);
                }
                await OnModalClose.InvokeAsync();
                await RenderWholePage.InvokeAsync();
            }
            else
            {
                _showValidation = true;
            }
        }

        private void ResetAttribute() 
        {
            CategoryAttribute.LanguageEntries = Clone.LanguageEntries;
            CategoryAttribute.DataType = Clone.DataType;
            CategoryAttribute.ValidationRegex = Clone.ValidationRegex;
            CategoryAttribute.RegexDescriptions = Clone.RegexDescriptions;
            CategoryAttribute.IsIncludedInPreview = Clone.IsIncludedInPreview;
            CategoryAttribute.UnitType = Clone.UnitType;
            CategoryAttribute.IsRequired = Clone.IsRequired;
            CategoryAttribute.PresentationType = Clone.PresentationType;
            CategoryAttribute.IsFilter = Clone.IsFilter;
            CategoryAttribute.PossibleValues = Clone.PossibleValues;
        }

        private async Task<string> GetObjectId()
        {
            string id = await HttpClient.GetStringAsync("/objectid");
            if (ApplicationCacheService.GetCategoriesFromAllLists().Where(c => c.Id == id).Any())
            {
                if (ApplicationCacheService.GetCategoriesFromAllLists().Where(c => c.CategoryAttributes.Any(a => a.Id == id)).Any())
                {
                    id = await GetObjectId();
                }
            }
            return id;
        }

        private void CloseValidation()
        {
            _showValidation = false;
        }
    }
}
