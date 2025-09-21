using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        private static int count = 0;
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public ESRoleUser Role { get; set; } = ESRoleUser.Admin;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public User()
        {
            this.Id = ++count;
        }
    }
}
