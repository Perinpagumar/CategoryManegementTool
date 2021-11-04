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

        private void ImportCategories()
        {
            var newCategories = JsonConvert.DeserializeObject<List<Category>>(InputJson);
            var allCategories = ApplicationCacheService.GetCategoriesFromAllLists();
            foreach(var category in newCategories)
            {
                foreach(var a in category.CategoryAttributes)
                {
                    if(a.PossibleValues == null)
                    {
                        a.PossibleValues = new();
                    }

                    if (a.LanguageEntries == null)
                    {
                        a.LanguageEntries = new();
                    }

                    if (a.RegexDescriptions == null)
                    {
                        a.RegexDescriptions = new();
                    }
                }
                if(!allCategories.Where(c => c.Id == category.Id).Any())
                {
                    ApplicationCacheService.OriginalCategories.Add(category);
                    ApplicationCacheService.AllCategories.Add(category);
                }
            }
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
