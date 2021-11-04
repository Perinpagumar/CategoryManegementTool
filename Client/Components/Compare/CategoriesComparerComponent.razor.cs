using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.Compare
{
    public partial class CategoriesComparerComponent
    {
        [Parameter]
        public Category Category { get; set; } = new();

        [Parameter]
        public Category EditedCategory { get; set; } = new();

        private LanguageEntry GetEditedLanguageEntryVersion(LanguageEntry languageEntry)
        {
            return EditedCategory.LanguageEntries
                .Where(l => l.Language == languageEntry.Language)
                .FirstOrDefault();
        }

        private List<LanguageEntry> GetAddedLanguageEntries()
        {
            var edited = new List<LanguageEntry>();
            foreach (var languageEntry in EditedCategory.LanguageEntries)
            {
                if (!Category.LanguageEntries.Where(l => l.Language == languageEntry.Language).Any())
                {
                    edited.Add(languageEntry);
                }
            }
            return edited;
        }

        private CategoryAttribute GetEditedCategoryAttributeVersion(CategoryAttribute attribute)
        {
            return EditedCategory.CategoryAttributes
                .Where(a => a.Id == attribute.Id)
                .FirstOrDefault();
        }

        private List<CategoryAttribute> GetAddedAttributes()
        {
            var added = new List<CategoryAttribute>();
            foreach (var attribute in EditedCategory.CategoryAttributes)
            {
                if (!Category.CategoryAttributes.Where(a => a.Id == attribute.Id).Any())
                {
                    added.Add(attribute);
                }
            }
            return added;
        }
    }
}
