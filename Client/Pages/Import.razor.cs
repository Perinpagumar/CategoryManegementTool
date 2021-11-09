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
            var newCategories = JsonConvert.DeserializeObject<List<Category>>(InputJson); ;
            ApplicationCacheService.OriginalCategories = newCategories;
            ApplicationCacheService.AllCategories = newCategories;
            await localStore.SetItemAsync("original", JsonConvert.SerializeObject(ApplicationCacheService.OriginalCategories));
            await localStore.SetItemAsync("all", JsonConvert.SerializeObject(ApplicationCacheService.AllCategories));
            await localStore.SetItemAsStringAsync("edited", "[]");
            await localStore.SetItemAsStringAsync("added", "[]");
            await localStore.SetItemAsStringAsync("deleted", "[]");
            NavigationManager.NavigateTo("/");
        }

        private async Task SingleUpload(InputFileChangeEventArgs e)
        {
            MemoryStream ms = new();
            await e.File.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();
            InputJson = System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
