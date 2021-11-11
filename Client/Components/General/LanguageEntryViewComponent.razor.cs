using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.General
{
    public partial class LanguageEntryViewComponent
    {
        [Parameter]
        public LanguageEntry LanguageEntry { get; set; }

        [Parameter]
        public CategoryAttribute Attribute { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool IsAdd { get; set; }

        [Parameter]
        public bool IsLanguageEntry { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        private readonly List<Language> Languages = Enum.GetValues(typeof(Language))
            .Cast<Language>()
            .ToList();

        private async Task DeleteAsync()
        {
            if(!IsAdd)
            {
                if (Attribute != null)
                {
                        Attribute.LanguageEntries.Remove(LanguageEntry);
                        Attribute.RegexDescriptions.Remove(LanguageEntry);
                        Attribute.PossibleValues.Remove(LanguageEntry);
                }
                else
                {
                    ApplicationCacheService.SelectedCategory.LanguageEntries.Remove(LanguageEntry);
                }
            }
            else
            {
                ApplicationCacheService.NewCategoryAttribute.LanguageEntries.Remove(LanguageEntry);
                ApplicationCacheService.NewCategoryAttribute.RegexDescriptions.Remove(LanguageEntry);
                ApplicationCacheService.NewCategoryAttribute.PossibleValues.Remove(LanguageEntry);
            }
            await RenderWholePage.InvokeAsync();
        }
    }
}
