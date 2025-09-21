using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Category
    {
        private static int count = 0;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Category()
        {
            Id = ++count;
        }
    }
}
