using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }         
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }            
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
