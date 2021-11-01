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
            var category = ApplicationCacheService.AllCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .First();

            if (category != null)
            {
                Category = category;
            }
            else
            {
                var deletedCategory = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id.ToString() == CategoryId)
                .First();

                if (deletedCategory != null)
                {
                    Category = deletedCategory.Clone();
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
