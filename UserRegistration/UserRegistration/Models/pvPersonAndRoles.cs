using System;
using DAL.Domain;
using System.Collections.Generic;

namespace UserRegistration.Models
{
    
    public class PersonAndRoles
    {
        public List<Person> AllPersons { get; set; }

        public List<Role> AllRoles { get; set; }
    }
}