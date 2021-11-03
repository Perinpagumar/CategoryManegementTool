using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace CategoryManegementTool.Client.Components.General
{
    public partial class CategoryTreeViewerComponent
    {
        [Parameter]
        public List<Category> AllCategories { get; set; }

        [Parameter]
        public List<Category> LevelCategories { get; set; }

        [Parameter]
        public int Level { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        [Parameter]
        public bool IsDeleted { get; set; }

        [Parameter]
        public bool IsNotMain { get; set; }

        private Language MainLanguage { get; set; } = ApplicationCacheService.MainLanguage;

        private List<Category> GetChilldCategories(string parentCategoryId)
        {
            return AllCategories.Where(category => category.ParrentCategoryId == parentCategoryId).ToList();
        }

        private void DeleteCategory(Category category)
        {
            ApplicationCacheService.AllCategories.Remove(category);
            ApplicationCacheService.DeletedCategories.Add(category);
        }
    }
}