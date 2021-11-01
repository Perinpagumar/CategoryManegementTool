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
        private Category Category { get; set; } = new();

        private Category EditedCategory { get; set; } = new();

        private bool Hidden { get; set; } = true;

        private string Status { get; set; } = "New Added";
        protected override void OnInitialized()
        {
            var originalCategories = ApplicationCacheService.OriginalCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
            if (originalCategories.Count > 0)
            {
                Category = originalCategories.First();
                Status = "Original";
                GetStatus();
            }
        }

        private void GetStatus() 
        {
            var deletedCategories = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
            if (deletedCategories.Count > 0)
            {
                Status = "Deleted";
            }
            else
            {
                var editedCategories = ApplicationCacheService.EditedCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
                if (editedCategories.Count > 0)
                {
                    EditedCategory = editedCategories.First();
                    Hidden = false;
                    Status = "Edited";
                }
            }
        }
    }
}
