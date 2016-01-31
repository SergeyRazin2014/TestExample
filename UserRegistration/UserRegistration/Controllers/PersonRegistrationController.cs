using DAL.Domain;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserRegistration.Infrastructure;
using UserRegistration.Models;

namespace UserRegistration.Controllers
{
    public class PersonRegistrationController : ApiController
    {
        private IPersonsDataProvider _personDataProvider;

        public PersonRegistrationController(IPersonsDataProvider provider)
        {
            _personDataProvider = provider;
        }

        //получить всех персонов и все роли
        public PersonAndRoles Get()
        {
            var personsAndRoles = _personDataProvider.GetPersonsAndRoles();

            return personsAndRoles;
        }

        //создать нового пользоватлея
        public Person Post(Person person)
        {
            if (person != null)
            {
                if (string.IsNullOrWhiteSpace(person.FullName) || string.IsNullOrWhiteSpace(person.Login) || string.IsNullOrWhiteSpace(person.Password))
                    throw new Exception("Ошибка валидации: не заполнены необходимые поля для создания нового пользователя");

                var addedPerson = _personDataProvider.AddNewPerson(person);
                return addedPerson;
            }

            throw new Exception("Ошибка добавления нового пользователя.");
        }

        //удалить пользователя
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                _personDataProvider.DeletePerson(Id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Item not found");
            }
        }

        //изменить пользователя
        public Person Put(Person modifiedPersonClient)
        {
            if (modifiedPersonClient != null)
            {
                var modifiedServerPerson = _personDataProvider.UpdatePerson(modifiedPersonClient);
                return modifiedServerPerson;
            }
            else
                return null;
        }
    }
}