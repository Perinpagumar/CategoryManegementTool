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

        protected override async Task OnInitializedAsync()
        {
            var deleted = ApplicationCacheService.DeletedCategories.Select(c => c.Id);
            var edited = ApplicationCacheService.EditedCategories.Select(c => c.Id);
            var added = ApplicationCacheService.AddedCategories.Select(c => c.Id);

            Categories = ApplicationCacheService.AllCategories
                .Where(c => !edited.Contains(c.Id))
                .Where(c => !added.Contains(c.Id))
                .Where(c => !deleted.Contains(c.Id))
                .ToList();

            EditedCategories = ApplicationCacheService.EditedCategories
                .Where(c => !added.Contains(c.Id))
                .Where(c => !deleted.Contains(c.Id))
                .ToList();

            AddedCategories = ApplicationCacheService.AddedCategories
                .Where(c => !edited.Contains(c.Id))
                .Where(c => !deleted.Contains(c.Id))
                .ToList();
            await RenderWholePage.InvokeAsync();
        }
    }
}
