using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Pages
{
    public partial class Export
    {
        [Parameter]
        public string List { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        private string OutputJson { get; set; }

        private List<Category> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            switch (List)
            {
                case "All":
                    Categories = ApplicationCacheService.AllCategories;
                    break;

                case "Edited":
                    Categories = ApplicationCacheService.EditedCategories;
                    break;

                case "Added":
                    Categories = ApplicationCacheService.AddedCategories;
                    break;

                case "Deleted":
                    Categories = ApplicationCacheService.DeletedCategories;
                    break;
            }
                await ExportCategories();
        }
        private async Task ExportCategories()
        {
            await HttpClient.PostAsJsonAsync<List<Category>>("/CategoryExport", Categories);
            var options = new JsonSerializerOptions { WriteIndented = true };
            OutputJson = System.Text.Json.JsonSerializer.Serialize(Categories, options);
        }  
    }
    
}
