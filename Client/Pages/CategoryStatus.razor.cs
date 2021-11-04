﻿using CategoryManagementTool.Shared.Enums;
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
            if (ApplicationCacheService.OriginalCategories.Count() > 0)
            {
                var originalCategory = ApplicationCacheService.OriginalCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
                if (originalCategory.Count() > 0)
                {
                    Category = originalCategory.First();
                    Status = "Original";
                    GetStatus();
                }
            }
        }

        private void GetStatus() 
        {
            if (ApplicationCacheService.EditedCategories.Count() > 0)
            {
                var editedCategory = ApplicationCacheService.EditedCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
                if (editedCategory.Count() > 0)
                {
                    EditedCategory = editedCategory.First().Clone();
                    Hidden = false;
                    Status = "Edited";
                }
            }

            if (ApplicationCacheService.DeletedCategories.Count() > 0)
            {
                var deletedCategory = ApplicationCacheService.DeletedCategories
                .Where(category => category.Id == CategoryId)
                .ToList();
                if (deletedCategory.Count() > 0)
                {
                    Status += " & Deleted";
                }
            }
        }
    }
}
