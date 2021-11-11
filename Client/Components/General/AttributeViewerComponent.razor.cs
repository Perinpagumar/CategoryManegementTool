using CategoryManegementTool.Client.Services;
using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.General
{
    public partial class AttributeViewerComponent
    {
        [Parameter]
        public CategoryAttribute CategoryAttribute {get; set;}

        [Parameter]
        public EventCallback RenderWholePage { get; set; }

        [Parameter]
        public bool IsView { get; set; }

        private bool _showViewAttribute { get; set; }
        private bool _showEditAttribute { get; set; }

        private void ViewAttribute()
        {
            _showViewAttribute = true;
        }

        private void EditAttribute()
        {
            _showEditAttribute = true;
        }

        private async Task DeleteAttributeAsync()
        {
            ApplicationCacheService.SelectedCategory.CategoryAttributes.Remove(CategoryAttribute);
            await RenderWholePage.InvokeAsync();
        }
    }
}
