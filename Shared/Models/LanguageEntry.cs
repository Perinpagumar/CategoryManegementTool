using CategoryManagementTool.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryManegementTool.Shared.Models
{
    public class LanguageEntry
    {
        public Language Language { get; set; }

        [Required]
        public string Text { get; set; }
        public string Value { get; set; }

        public bool SerchAllData(string input)
        {
            if (Language.ToString().ToLower().Contains(input) || Text.ToLower().Contains(input))
            {
                return true;
            }
            return false;
        }

        public bool IsValid() 
        {
            if(Language != Language.Undefined && Text != null)
            {
                return true;
            }
            return false;
        }
    }
}
