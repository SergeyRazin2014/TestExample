using System;
using System.Collections.Generic;

namespace DAL.Domain
{
    
    public class Person
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public List<Role> Roles { get; set; }
    }
}