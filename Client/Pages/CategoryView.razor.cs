using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
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

        protected override async Task OnInitializedAsync()
        {
            ApplicationCacheService.AllCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("all"));
            ApplicationCacheService.DeletedCategories = JsonConvert.DeserializeObject<List<Category>>(await localStore.GetItemAsync<string>("deleted"));
            var category = ApplicationCacheService.AllCategories
                .Where(category => category.Id == CategoryId)
                .ToList();

            if (category.Count() > 0)
            {
                Category = category.First().Clone();
            }
            else
            {
                var deletedCategory = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id == CategoryId)
                .ToList();

                if (deletedCategory.Count() > 0)
                {
                    Category = deletedCategory.First().Clone();
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
