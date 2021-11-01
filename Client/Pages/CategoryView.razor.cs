using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class CategoryView
    {
        [Parameter]
        public string CategoryId { get; set; }

        private Category Category { get; set; } = new();

        protected override void OnInitialized()
        {
            var categories = ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .ToList();

            if (categories.Count() > 0)
            {
                Category = categories[0];
            }
            else
            {
                var deletedCategories = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .ToList();

                if (deletedCategories.Count() > 0)
                {
                    Category = deletedCategories[0].Clone();
                }
            }
        }
        private List<List<CategoryAttribute>> GetParentAttributes()//Prtotype
        {
            return ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == Category.ParrentCategoryId)
                .Select(category => category.CategoryAttributes)
                .ToList();
        }
    }
}
