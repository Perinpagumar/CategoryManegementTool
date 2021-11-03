using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components
{
    public partial class CompareLanguageEntryComponent
    {
        [Parameter]
        public LanguageEntry Original { get; set; } = new();

        [Parameter]
        public LanguageEntry Edited { get; set; } = new();

        private ConsoleColor TextIsEdited()
        {
            if(Original.Text == Edited.Text)
            {
                return ConsoleColor.White;
            }

            return ConsoleColor.Yellow;
        }
    }
}
