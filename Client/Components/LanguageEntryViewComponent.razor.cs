using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components
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
        public bool IsLanguageEntry { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        private readonly List<Language> Languages = Enum.GetValues(typeof(Language))
            .Cast<Language>()
            .ToList();

        private async Task DeleteAsync()
        {
            if (AttributeId != null)
            {
                var attribute = ApplicationCacheService.SelectedCategory.CategoryAttributes
                    .Where(attribute => attribute.Id.ToString() == AttributeId)
                    .ToList();
                if (attribute != null)
                {
                    attribute[0].LanguageEntries.Remove(LanguageEntry);
                    attribute[0].RegexDescriptions.Remove(LanguageEntry);
                    attribute[0].PossibleValues.Remove(LanguageEntry);
                }
            }
            else
            {
                ApplicationCacheService.SelectedCategory.LanguageEntries.Remove(LanguageEntry);
            }
            await RenderWholePage.InvokeAsync();
        }
    }
}
