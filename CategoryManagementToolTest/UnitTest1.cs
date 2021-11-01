using CategoryManegementTool.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace CategoryManagementToolTest
{
    public class UnitTest1
    {
        [Fact]
        public void DeserializeCategoriesFromFile()
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText(@"C:\Users\Munnanathan\Documents\Work\Projekte\Blazor\CategoryManegementTool\Shared\Files\kategorien.json"));
            Assert.Equal(2, categories.Count);
        }
    }
}
