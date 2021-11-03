using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.Compare
{
    public partial class CategoryCompareViewerComponent
    {
        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        [Parameter]
        public bool IsDeleted { get; set; }

        [Parameter]
        public bool IsNotMain { get; set; }

        private List<Category> Categories { get; set; }

        private List<Category> EditedCategories { get; set; }

        private List<Category> AddedCategories { get; set; }

        private List<Category> DeletedCategories = ApplicationCacheService.DeletedCategories;

        protected override void OnInitialized()
        {
            Categories = ApplicationCacheService.AllCategories
                .Where(category => !ApplicationCacheService.EditedCategories.Contains(category))
                .Where(category => !ApplicationCacheService.AddedCategories.Contains(category))
                .ToList();

            EditedCategories = ApplicationCacheService.EditedCategories
                .Where(category => !ApplicationCacheService.DeletedCategories.Contains(category))
                .ToList();

            AddedCategories = ApplicationCacheService.AddedCategories
                .Where(category => !ApplicationCacheService.EditedCategories.Contains(category))
                .Where(category => !ApplicationCacheService.DeletedCategories.Contains(category))
                .ToList();
        }
    }
}
