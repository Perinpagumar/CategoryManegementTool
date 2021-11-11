using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
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

        private Language MainLanguage { get; set; } = ApplicationCacheService.MainLanguage;

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

        private bool AttributeParametersAreNotEqual(CategoryAttribute original, CategoryAttribute edited)
        {
            return original.DataType != edited.DataType ||
            original.ValidationRegex != edited.ValidationRegex ||
            original.IsIncludedInPreview != edited.IsIncludedInPreview ||
            original.UnitType != edited.UnitType ||
            original.IsRequired != edited.IsRequired ||
            original.PresentationType != edited.PresentationType ||
            original.IsFilter != edited.IsFilter ||
            LanguageEntrisAreNotEqual(original.LanguageEntries, edited.LanguageEntries) ||
            LanguageEntrisAreNotEqual(original.RegexDescriptions, edited.RegexDescriptions) ||
            PossibleValuesAreNotEqual(original.PossibleValues, edited.PossibleValues);
        }

        private bool LanguageEntrisAreNotEqual(List<LanguageEntry> original, List<LanguageEntry> edited)
        {
            if (edited.Count() > 0)
            {
                foreach (var languageEntry in original ?? new List<LanguageEntry>())
                {
                    var entry = edited.Where(l => l.Language == languageEntry.Language).ToList();
                    if (entry.Count() > 0)
                    {
                        if (entry.First().Text != languageEntry.Text)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool PossibleValuesAreNotEqual(List<LanguageEntry> original, List<LanguageEntry> edited)
        {
            if (edited.Count() > 0)
            {
                foreach (var languageEntry in original ?? new List<LanguageEntry>())
                {
                    var entry = edited.Where(l => l.Language == languageEntry.Language && l.Value == languageEntry.Value).ToList();
                    if (entry.Count() > 0)
                    {
                        if (entry.First().Text != languageEntry.Text)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
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
