using DAL;
using DAL.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UserRegistration.Models;

namespace UserRegistration.Infrastructure
{
    public class PersonsDataProviderXml : IPersonsDataProvider
    {
        private SerializeXML<PersonAndRoles> _svr;
        

        public PersonsDataProviderXml(Mutex mutex)
        {
            _svr = new SerializeXML<PersonAndRoles>(mutex);
        }

        public PersonAndRoles GetPersonsAndRoles()
        {
            if (File.Exists(_svr.PathToFile))
            {
                var persAndRoeles = _svr.Desirializ();
                return persAndRoeles;
            }
            else
            {
                List<Role> roleList = new List<Role>();
                Role admin = new Role() { Id = 1, Name = "Администратор" };
                Role hydrologist = new Role() { Id = 2, Name = "Гидрологист" };
                Role operationalStaff = new Role() { Id = 3, Name = "Оперативный персонал" };

                roleList.Add(admin);
                roleList.Add(hydrologist);
                roleList.Add(operationalStaff);

                return new PersonAndRoles() { AllPersons = new List<Person>(), AllRoles = roleList };
            }
        }

        public Person AddNewPerson(Person newPerson)
        {
            var data = GetPersonsAndRoles();

            if (data.AllPersons.Count > 0)
                newPerson.Id = data.AllPersons.Max(e => e.Id) + 1;
            else
                newPerson.Id = 1;

            data.AllPersons.Add(newPerson);

            _svr.SerializtionWithExchengeFile(data);

            return newPerson;
        }

        public void DeletePerson(int Id)
        {
            var data = GetPersonsAndRoles();

            var personForDelete = data.AllPersons.FirstOrDefault(e => e.Id == Id);

            if (personForDelete == null)
                throw new Exception("пользователь не найден.");

            data.AllPersons.Remove(personForDelete);

            _svr.SerializtionWithExchengeFile(data);
        }

        public Person UpdatePerson(Person modifiedPerosn)
        {
            var data = GetPersonsAndRoles();

            int findedPersonId = GetPersonsId(data.AllPersons, modifiedPerosn);

            data.AllPersons[findedPersonId] = modifiedPerosn;

            _svr.SerializtionWithExchengeFile(data);

            return modifiedPerosn;
        }

        private int GetPersonsId(List<Person> allPerson, Person modifiedPerosn)
        {
            for (var i = 0; i < allPerson.Count; i++)
            {
                if (allPerson[i].Id == modifiedPerosn.Id)
                    return i;
            }

            throw new Exception("Пользователь не найден.");
        }
    }
}