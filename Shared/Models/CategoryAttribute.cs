﻿
using CategoryManagementTool.Shared.Enums;
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

        public string? ValidationRegex { get; set; }

        public List<LanguageEntry>? RegexDescriptions { get; set; } = new();

        public bool IsIncludedInPreview { get; set; }

        public UnitType? UnitType { get; set; }

        public bool IsRequired { get; set; }

        public PresentationType PresentationType { get; set; }

        public bool IsFilter { get; set; }

        public List<LanguageEntry>? PossibleValues { get; set; } = new();

        public bool SerchAllData(string input)
        {
            if (this.Id.ToString().ToLower().Contains(input))
            {
                return true;
            }

            foreach (var languageEntry in LanguageEntries)
            {
                if (languageEntry.SerchAllData(input))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsValid()
        {
            if(LanguageEntriesAreValid(LanguageEntries) && RegexIsValid() && IsNotUndefined())
            {
                return true;
            }
            return false;
        }

        private bool RegexIsValid() 
        {
            if(ValidationRegex != null)
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

            private bool LanguageEntriesAreValid(List<LanguageEntry> thisLanguageEntry)
        {
            var language = thisLanguageEntry.Select(languageEntry => languageEntry.Language);

            foreach (var languagEntry in thisLanguageEntry)
            {
                if (languagEntry.IsValid() == false)
                {
                    return false;
                }
            }

            if (language.Contains(Language.German) && language.Contains(Language.English))
            {
                return true;
            }

            return false;
        }
    }
}