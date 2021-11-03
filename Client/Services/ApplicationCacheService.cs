using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Services
{
    public static class ApplicationCacheService
    {
        public static List<Category> OriginalCategories { get; set; } = new();
        public static List<Category> AllCategories { get; set; } = new();
        public static List<Category> EditedCategories { get; set; } = new();
        public static List<Category> AddedCategories { get; set; } = new();
        public static List<Category> DeletedCategories { get; set; } = new();

        public static Language MainLanguage { get; set; } = Language.German;
        public static Category SelectedCategory { get; set; } = new();

        public static List<Category> GetCategoriesFromAllLists()
        {
            var fromAll = new List<Category>();
            fromAll.AddRange(OriginalCategories);
            fromAll.AddRange(AllCategories);
            fromAll.AddRange(EditedCategories);
            fromAll.AddRange(AddedCategories);
            fromAll.AddRange(DeletedCategories);
            return fromAll;
        }
    }
}
