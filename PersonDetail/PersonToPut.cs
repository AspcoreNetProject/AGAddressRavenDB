using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AGAddressRavenDB.PersonDetail
{
    public class PersonToPut
    {
       // public string PersonID { get; set; }
        [Required]
        [StringLength(64)]
        [RegularExpression("^([A-z][A-Za-z]*\\s+[A-Za-z]*)|([A-z][A-Za-z]*)$", ErrorMessage = "No number or spl characters in Firstname")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(64)]
        [RegularExpression("^([A-z][A-Za-z]*\\s+[A-Za-z]*)|([A-z][A-Za-z]*)$", ErrorMessage = "No number or spl characters in Lastname")]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        //[RegularExpression(@"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$")]
        public string Address { get; set; }
    }
}
