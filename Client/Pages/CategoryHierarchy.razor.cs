using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace CategoryManegementTool.Client.Pages
{
    public partial class CategoryHierarchy
    {
        private List<Category> Categories { get; set; } = ApplicationCacheService.AllCategories;

        private string List { get; set; } = "All";

        private string Search { get; set; }
        private bool IsNotMain { get; set; } = false;
        private bool IsDeleted { get; set; } = false;
        private bool IsCompare { get; set; } = false;

        private readonly string newRootCategory = "null";

        private List<Category> GetRootCategories()
        {
            return Categories.Where(category => category.ParrentCategoryId == null).ToList();
        }

        private bool _shouldRenderer { get; set; }
        protected override bool ShouldRender()
        {
            return _shouldRenderer || base.ShouldRender();
        }

        public void RenderWholePage()
        {
            _shouldRenderer = true;
            StateHasChanged();
        }

        private bool GetIsNotMain() 
        {
            if(List == "Compare")
            {
                return true;
            }
            return !IsNotMain;
        }

        private void ChageList()
        {
            switch (List) 
            {
                case "All":
                    Categories = ApplicationCacheService.AllCategories;
                    IsDeleted = false;
                    IsNotMain = false;
                    IsCompare = false;
                    break;

                case "Edited":
                    Categories = ApplicationCacheService.EditedCategories;
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Added":
                    Categories = ApplicationCacheService.AddedCategories;
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Deleted":
                    Categories = ApplicationCacheService.DeletedCategories;
                    IsDeleted = true;
                    IsNotMain = true;
                    IsCompare = false;
                    break;

                case "Compare":
                    Categories = new();
                    Categories.AddRange(ApplicationCacheService.AllCategories);
                    Categories.AddRange(ApplicationCacheService.DeletedCategories);
                    IsDeleted = false;
                    IsNotMain = true;
                    IsCompare = true;
                    break;
            }
            RenderWholePage();
        }

        private void FilterCategoies() 
        {
            if(Search == string.Empty)
            {
                IsNotMain = false;
                ChageList();
            }
            else
            {
                switch (List)
                {
                    case "All":
                        Categories = ApplicationCacheService.AllCategories
                            .Where(category => category.SerchAllData(Search))
                            .ToList();
                        break;

                    case "Edited":
                        Categories = ApplicationCacheService.EditedCategories
                            .Where(category => category.SerchAllData(Search))
                            .ToList();
                        break;

                    case "Added":
                        Categories = ApplicationCacheService.AddedCategories
                            .Where(category => category.SerchAllData(Search))
                            .ToList();
                        break;

                    case "Deleted":
                        Categories = ApplicationCacheService.DeletedCategories
                            .Where(category => category.SerchAllData(Search))
                            .ToList();
                        break;
                }
                IsNotMain = true;
                OnInitialized();
                RenderWholePage();
            }
        }
    }
}
