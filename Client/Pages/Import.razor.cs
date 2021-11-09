using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class Import
    {
        private string InputJson { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        private async Task ImportCategoriesAsync()
        {
            var newCategories = RemoveDuplicates();
            ApplicationCacheService.OriginalCategories.AddRange(newCategories);
            ApplicationCacheService.AllCategories.AddRange(newCategories);
            await localStore.SetItemAsync("original", JsonConvert.SerializeObject(ApplicationCacheService.OriginalCategories));
            await localStore.SetItemAsync("all", JsonConvert.SerializeObject(ApplicationCacheService.AllCategories));
            NavigationManager.NavigateTo("/");
        }

        private async Task SingleUpload(InputFileChangeEventArgs e)
        {
            MemoryStream ms = new();
            await e.File.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();
            InputJson = System.Text.Encoding.UTF8.GetString(bytes);
        }

        private List<Category> RemoveDuplicates()
        {
            var newCategories = JsonConvert.DeserializeObject<List<Category>>(InputJson);
            var allCategories = ApplicationCacheService.GetCategoriesFromAllLists();
            var filteredCategories = new List<Category>();
            foreach (var category in newCategories)
            {
                filteredCategories.AddRange(allCategories.Where(c => c.Id == category.Id).ToList());
            }

            foreach (var category in filteredCategories)
            {
                newCategories.Remove(category);
            }

            return newCategories;
        }
    }
}
