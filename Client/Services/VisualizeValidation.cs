using CategoryManagementTool.Shared.Enums;
using CategoryManegementTool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManegementTool.Client.Services
{
    public static  class VisualizeValidation
    {
        public static ConsoleColor TextNotEmpty(string text)
        {
            if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.White;
        }

        public static ConsoleColor LanguageNotUndefined(Language input)
        {
            if (input == Language.Undefined)
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.White;
        }

        public static ConsoleColor DataTypeNotUndefined(DataType input)
        {
            if (input == DataType.Undefined)
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.White;
        }

        public static ConsoleColor PresentationTypeNotUndefined(PresentationType input)
        {
            if (input == PresentationType.Undefined)
            {
                return ConsoleColor.Red;
            }

            return ConsoleColor.White;
        }  
    }
}
