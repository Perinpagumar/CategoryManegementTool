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
        public string AttributeId { get; set; }

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
            if(ApplicationCacheService.NewCategoryAttribute.Id == null)
            {
                if (AttributeId == null)
                {
                    var attribute = ApplicationCacheService.SelectedCategory.CategoryAttributes
                        .Where(attribute => attribute.Id == AttributeId)
                        .First();
                    if (attribute != null)
                    {
                        attribute.LanguageEntries.Remove(LanguageEntry);
                        attribute.RegexDescriptions.Remove(LanguageEntry);
                        attribute.PossibleValues.Remove(LanguageEntry);
                    }
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
