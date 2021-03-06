using CategoryManegementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Components.Compare
{
    public partial class CompareLanguageEntryComponent
    {
        [Parameter]
        public LanguageEntry Original { get; set; } = new();

        [Parameter]
        public LanguageEntry Edited { get; set; } = new();

        [Parameter]
        public bool IsPossibleValue { get; set; } = false;

        private ConsoleColor TextIsEdited()
        {
            if (Original.Text == Edited.Text)
            {
                return ConsoleColor.White;
            }

            return ConsoleColor.Yellow;
        }

        private ConsoleColor ValueIsEdited()
        {
            if (Original.Value == Edited.Value)
            {
                return ConsoleColor.White;
            }

            return ConsoleColor.Yellow;
        }
    }
}
