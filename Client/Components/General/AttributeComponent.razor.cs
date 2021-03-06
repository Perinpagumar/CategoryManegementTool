using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.General
{
    public partial class AttributeComponent
    {
        [Parameter]
        public CategoryAttribute CategoryAttribute { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool IsAdd { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        private List<DataType> DataTypes = Enum.GetValues(typeof(DataType))
            .Cast<DataType>()
            .ToList();

        private List<PresentationType> PresentationTypes = Enum.GetValues(typeof(PresentationType))
            .Cast<PresentationType>()
            .ToList();

        private List<UnitType> UnitTypes = Enum.GetValues(typeof(UnitType))
            .Cast<UnitType>()
            .ToList();

        private void AddLanguageEntry()
        {
            if (IsAdd)
            {
                ApplicationCacheService.NewCategoryAttribute.LanguageEntries.Add(new LanguageEntry());
            }
            else
            {
                ApplicationCacheService.SelectedCategory.CategoryAttributes
                .First(attribute => attribute.Id == CategoryAttribute.Id).LanguageEntries.Add(new LanguageEntry());
            }
        }

        private void AddRegexDescription()
        {
            if (IsAdd)
            {
                ApplicationCacheService.NewCategoryAttribute.RegexDescriptions.Add(new LanguageEntry());
            }
            else
            {
                ApplicationCacheService.SelectedCategory.CategoryAttributes
            .First(attribute => attribute.Id == CategoryAttribute.Id).RegexDescriptions.Add(new LanguageEntry());
            }
        }

        private void AddPossibleValue()
        {
            if (IsAdd)
            {
                ApplicationCacheService.NewCategoryAttribute.PossibleValues.Add(new LanguageEntry());
            }
            else
            {
                ApplicationCacheService.SelectedCategory.CategoryAttributes
            .First(attribute => attribute.Id == CategoryAttribute.Id).PossibleValues.Add(new LanguageEntry());
            }
        }
    }
}
