﻿using CategoryManagementTool.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryManegementTool.Shared.Models
{
    public class Category
    {
        [Required]
        public string Id { get; set; }

        public List<LanguageEntry> LanguageEntries { get; set; } = new();
        public string ParrentCategoryId { get; set; }
        public List<CategoryAttribute>? CategoryAttributes { get; set; } = new();

        public string GetTextFromLanguage(Language language) 
        {
            return this.LanguageEntries.Where(entry => entry.Language == language)
                .Select(entry => entry.Text)
                .ToList()
                .ElementAt(0);
        }

        public Category Clone() 
        {
           return JsonConvert.DeserializeObject<Category>(JsonConvert.SerializeObject(this));
        }

        public bool SerchAllData(string input)
        {
            input = input.ToLower();
            if(this.Id.ToString().ToLower().Contains(input))
            {
                return true;
            }

            foreach(var languageEntry in LanguageEntries) 
            {
                if(languageEntry.SerchAllData(input))
                {
                    return true;
                }
            }

            foreach(var attribute in CategoryAttributes) 
            {
                if(attribute.SerchAllData(input))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsValid() 
        {
            var messages = new List<string>();
            if(LanguageEntriesAreValid() && CategoryAttributesAreValid())
            {
                return true;
            }
            return false;
        }

        private bool LanguageEntriesAreValid()
        {
            var language = LanguageEntries.Select(languageEntry => languageEntry.Language);

            foreach(var languagEntry in LanguageEntries) 
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

        private bool CategoryAttributesAreValid() 
        {
            foreach(var attribute in CategoryAttributes) 
            {
                if (attribute.IsValid() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}