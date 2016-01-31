using System;
using DAL.Domain;

namespace UserRegistration.Infrastructure
{
    public interface IPersonsDataProvider
    {
        Person AddNewPerson(Person newPerson);
        void DeletePerson(int Id);
        Models.PersonAndRoles GetPersonsAndRoles();
        Person UpdatePerson(Person modifiedPerosn);
    }
}
