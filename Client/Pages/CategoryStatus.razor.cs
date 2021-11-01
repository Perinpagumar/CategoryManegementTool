using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class CategoryStatus
    {
        [Parameter]
        public string CategoryId { get; set; }

        private Language MainLanguage { get; set; } = ApplicationCacheService.MainLanguage;
        private Category? Category { get; set; } = new();

        private Category? EditedCategory { get; set; } = new();

        private bool Hidden { get; set; } = true;

        private string Status { get; set; } = "New Added";
        protected override void OnInitialized()
        {
            var originalCategory = ApplicationCacheService.OriginalCategories
                .Where(category => category.Id == CategoryId)
                .First();
            if (originalCategory != null)
            {
                Category = originalCategory;
                Status = "Original";
                GetStatus();
            }
        }

        private void GetStatus() 
        {
            if(ApplicationCacheService.DeletedCategories.Count() > 0)
            {
                var deletedCategory = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id == CategoryId)
                .First();
                if (deletedCategory != null)
                {
                    Status = "Deleted";
                }
            }
            else
            {
                if (ApplicationCacheService.EditedCategories.Count() > 0)
                {
                    var editedCategory = ApplicationCacheService.EditedCategories
                    .Where(category => category.Id == CategoryId)
                    .First();
                    if (editedCategory != null)
                    {
                        EditedCategory = editedCategory;
                        Hidden = false;
                        Status = "Edited";
                        GetDifferences();
                    }
                }
            }
        }

        private void GetDifferences()
        {
            
        }
    }
}
