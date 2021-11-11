
using CategoryManagementTool.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryManegementTool.Shared.Models
{
    public class CategoryAttribute
    {
        public string Id { get; set; }

        public List<LanguageEntry> LanguageEntries { get; set; } = new();

        public DataType DataType { get; set; }

        public string ValidationRegex { get; set; }

        public List<LanguageEntry> RegexDescriptions { get; set; } = new();

        public bool IsIncludedInPreview { get; set; }

        public UnitType? UnitType { get; set; }

        public bool IsRequired { get; set; }

        public PresentationType PresentationType { get; set; }

        public bool IsFilter { get; set; }

        public List<LanguageEntry> PossibleValues { get; set; } = new();

        public string GetTextFromLanguage(Language language)
        {
            return this.LanguageEntries.Where(entry => entry.Language == language)
                .Select(entry => entry.Text)
                .ToList()
                .ElementAt(0);
        }

        public bool SerchAllData(string input)
        {
            if (this.Id.ToString().ToLower().Contains(input))
            {
                return true;
            }

            foreach (var languageEntry in LanguageEntries ?? new List<LanguageEntry>())
            {
                if (languageEntry.SerchAllData(input))
                {
                    return true;
                }
            }
            return false;
        }

        public CategoryAttribute Clone()
        {
            return JsonConvert.DeserializeObject<CategoryAttribute>(JsonConvert.SerializeObject(this));
        }

        public bool IsValid()
        {
            if(LanguageEntriesAreValid(LanguageEntries) && RegexIsValid() && PossibleValuesAreValid() && IsNotUndefined())
            {
                return true;
            }
            return false;
        }

        private bool RegexIsValid() 
        {
            if(!string.IsNullOrEmpty(ValidationRegex) || !string.IsNullOrWhiteSpace(ValidationRegex))
            {
                return LanguageEntriesAreValid(RegexDescriptions);
            }

            return true;
        }

        private bool IsNotUndefined()
        {
            return (DataType != DataType.Undefined && PresentationType != PresentationType.Undefined)
                ? true
                : false;
        }

        private bool PossibleValuesAreValid()
        {
            if(PossibleValues != null)
            {
                foreach(var possibleValue in PossibleValues ?? new List<LanguageEntry>())
                {
                    if (possibleValue.Language == Language.Undefined && string.IsNullOrEmpty(possibleValue.Text) && string.IsNullOrEmpty(possibleValue.Value))
                    {
                        return false;
                    }
                }
                return true;
            }
            return true;
        }
        private bool LanguageEntriesAreValid(List<LanguageEntry> thisLanguageEntry)
        {
            var entryLanguages = thisLanguageEntry.Select(languageEntry => languageEntry.Language);

            var languages = Enum.GetValues(typeof(Language))
                    .Cast<Language>()
                    .ToList();
            foreach (var language in languages)
            {
                if (entryLanguages.Where(l => l == language).Count() > 1)
                {
                    return false;
                }
            }

            foreach (var languagEntry in thisLanguageEntry)
            {
                if (languagEntry.IsValid() == false)
                {
                    return false;
                }
            }

            if (entryLanguages.Contains(Language.German) && entryLanguages.Contains(Language.English))
            {
                return true;
            }

            return false;
        }
    }
}
