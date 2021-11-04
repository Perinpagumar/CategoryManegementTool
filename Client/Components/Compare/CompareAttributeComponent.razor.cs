using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.Compare
{
    public partial class CompareAttributeComponent
    {
        [Parameter]
        public CategoryAttribute Original { get; set; } = new();

        [Parameter]
        public CategoryAttribute Edited { get; set; } = new();

        private LanguageEntry GetEditedLanguageEntryVersion(LanguageEntry languageEntry, List<LanguageEntry> languageEntries)
        {
            return languageEntries
                .Where(l => l.Language == languageEntry.Language)
                .FirstOrDefault();
        }

        private List<LanguageEntry> GetAddedLanguageEntries(List<LanguageEntry> originalLanguageEntries, List<LanguageEntry> editedLanguageEntries)
        {
            var edited = new List<LanguageEntry>();
            if (originalLanguageEntries != null && editedLanguageEntries != null)
            {
                foreach (var languageEntry in editedLanguageEntries)
                {
                    if (!originalLanguageEntries.Where(l => l.Language == languageEntry.Language).Any())
                    {
                        edited.Add(languageEntry);
                    }
                }
            }
            return edited;
        }

        private LanguageEntry GetEditedPossibleValueVersion(LanguageEntry languageEntry)
        {
            return Edited.PossibleValues
                .Where(l => l.Language == languageEntry.Language && l.Value == languageEntry.Value)
                .FirstOrDefault();
        }

        private List<LanguageEntry> GetAddedPssibleValues()
        {
            var edited = new List<LanguageEntry>();
            if (Original.PossibleValues != null && Edited.PossibleValues != null)
            {
                foreach (var languageEntry in Edited.PossibleValues)
                {
                    if (!Original.PossibleValues.Where(l => l.Language == languageEntry.Language && l.Value == languageEntry.Value).Any())
                    {
                        edited.Add(languageEntry);
                    }
                }
            }
            return edited;
        }

        private ConsoleColor BooleanEdited(bool original, bool edited)
        {
            if (original.Equals(edited))
            {
                return ConsoleColor.White;
            }
            return ConsoleColor.Yellow;
        }

        private ConsoleColor TextEdited(string original, string edited)
        {
            if (original == edited)
            {
                return ConsoleColor.White;
            }
            return ConsoleColor.Yellow;
        }

        private ConsoleColor DataTypeEdited()
        {
            if (Original.DataType == Edited.DataType)
            {
                return ConsoleColor.White;
            }
            return ConsoleColor.Yellow;
        }

        private ConsoleColor UnitTypeEdited()
        {
            if (Original.UnitType == Edited.UnitType)
            {
                return ConsoleColor.White;
            }
            return ConsoleColor.Yellow;
        }

        private ConsoleColor PresentationTypeEdited()
        {
            if (Original.PresentationType == Edited.PresentationType)
            {
                return ConsoleColor.White;
            }
            return ConsoleColor.Yellow;
        }
    }
}
