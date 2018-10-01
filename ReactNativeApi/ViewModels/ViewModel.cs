using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactNativeApi.ViewModels
{
    public class ViewModel
    {
        public class UserModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public int CountryId { get; set; }
            public string Country { get; set; }
            public DateTime DOB { get; set; }
            public List<Hobbies> Hobbies { get; set; }
        }
        public class Hobbies
        {
            public int value { get; set; }
            public string label { get; set; }
        }
    }
}