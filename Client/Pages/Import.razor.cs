using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class Import
    {
        private string InputJson { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        private void ImportCategories()
        {
            ApplicationCacheService.OriginalCategories.AddRange(JsonConvert.DeserializeObject<List<Category>>(InputJson));
            ApplicationCacheService.AllCategories.AddRange(JsonConvert.DeserializeObject<List<Category>>(InputJson));
            NavigationManager.NavigateTo("/categories");
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
