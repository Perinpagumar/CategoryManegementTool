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
    public partial class CategoryComponent
    {
        [Parameter]
        public Category Category { get; set; }

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        [Parameter]
        public bool IsDeleted { get; set; }

        [Parameter]
        public bool IsNotMain { get; set; }

        private Language MainLanguage { get; set; } = ApplicationCacheService.MainLanguage;

        private bool GetIsDelete()
        {
            return IsNotMain
                ? true
                : false;
        }

        private bool GetIsRestore()
        {
            if (IsDeleted)
            {
                var list = ApplicationCacheService.DeletedCategories.Where(category => category.Id == Category.ParrentCategoryId).ToList();
                return list.Count() > 0
                    ? true
                    : false;
            }
            return true;
        }

        private async Task DeleteAsync()
        {
            var categories = GetAllCategories(Category);
            categories.Add(Category);
            foreach (var category in categories)
            {
                ApplicationCacheService.AllCategories.Remove(category);
                ApplicationCacheService.DeletedCategories.Add(category);
            }
            await RenderWholePage.InvokeAsync();
        }

        private async Task RestoreCategoryAsync()
        {
            var categories = GetAllCategories(Category);
            categories.Add(Category);
            foreach (var category in categories)
            {
                ApplicationCacheService.DeletedCategories.Remove(category);
                ApplicationCacheService.AllCategories.Add(category);
            }
            await RenderWholePage.InvokeAsync();
        }

        private List<Category> GetAllCategories(Category thisCategory)
        {
            var allCategories = new List<Category>();
            var childCategories = new List<Category>();
            if (IsDeleted)
            {
                allCategories.AddRange(ApplicationCacheService.DeletedCategories.Where(category => category.ParrentCategoryId == thisCategory.Id.ToString()).ToList());
            }
            else
            {
                allCategories.AddRange(ApplicationCacheService.AllCategories.Where(category => category.ParrentCategoryId == thisCategory.Id.ToString()).ToList());
            }

            if (allCategories.Count() > 0)
            {
                foreach (var category in allCategories)
                {
                    childCategories.AddRange(GetAllCategories(category));
                }
            }
            allCategories.AddRange(childCategories);
            return allCategories;
        }
    }
}
