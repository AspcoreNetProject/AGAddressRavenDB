using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace AGAddressRavenDB.PersonDetail
{
    public class PersonToGet
    {
        //public string PersonID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
       
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
