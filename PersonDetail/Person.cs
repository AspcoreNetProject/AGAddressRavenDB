using System.Runtime.Serialization;

namespace AGAddressRavenDB.PersonDetail
{
    public class Person
    {

       //public string PersonId { get; set; }
        public string FirstName { get;  set; }
        public string  LastName { get;  set; }
        public string Address { get;  set; }


        public Person()
        { }

        public Person(string firstname, string lastname, string address )
        {
           
            FirstName = firstname.ToLower();
            LastName = lastname.ToLower();
            Address = address;
            //PersonId = pId ?? string.Empty;

        }
    }
}
