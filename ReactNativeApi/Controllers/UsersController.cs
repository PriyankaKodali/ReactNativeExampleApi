using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using ReactNativeApi.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using static ReactNativeApi.ViewModels.ViewModel;

namespace ReactNativeApi.Controllers
{
    public class UsersController : ApiController
    {

        [HttpPost]
        public IHttpActionResult AddUser()
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var form = HttpContext.Current.Request.Form;
                    var hobbies = JsonConvert.DeserializeObject<List<Hobbies>>(form.Get("selectedHobbies"));

                    User user = new User();

                    user.FirstName = form.Get("firstName");
                    user.LastName = form.Get("lastName");
                    user.Email = form.Get("email");
                    user.Country = Convert.ToInt32(form.Get("country"));
                    user.Gender = form.Get("gender");
                    user.DOB = Convert.ToDateTime(form.Get("dob"));

                    for (int i = 0; i < hobbies.Count(); i++)
                    {
                        UserHobbiesMapping hobby = new UserHobbiesMapping();

                        hobby.HobbyId = hobbies[i].value;
                        user.UserHobbiesMappings.Add(hobby);
                    }

                    db.Users.Add(user);
                    db.SaveChanges();

                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occcured, please try again later");
            }
        }


        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var users = db.Users.Select(x => new { name = x.FirstName + "" + x.LastName, Id = x.Id }).ToList();
                    return Content(HttpStatusCode.OK, new { users });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }

        [HttpGet]
        public IHttpActionResult GetUser(int userId)
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var user = db.Users.Find(userId);
                    var userHobbies = db.UserHobbiesMappings.Where(x => x.UserId == userId).ToList();

                    UserModel userModel = new UserModel();
                    userModel.Country = user.Country1.Name;
                    userModel.CountryId = user.Country;
                    userModel.FirstName = user.FirstName;
                    userModel.LastName = user.LastName;
                    userModel.Email = user.Email;
                    userModel.DOB = user.DOB;
                    userModel.Gender = user.Gender;

                    List<Hobbies> hobbies = new List<Hobbies>();

                    for (int i = 0; i < userHobbies.Count(); i++)
                    {
                        Hobbies hobby = new Hobbies();
                        hobby.value = Convert.ToInt32(userHobbies[i].HobbyId);
                        hobby.label = userHobbies[i].Hobby.Hobby1;

                        hobbies.Add(hobby);
                    }

                    userModel.Hobbies = hobbies;

                    return Content(HttpStatusCode.OK, new { userModel });

                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateUser(int userId)
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var user = db.Users.Find(userId);
                    var form = HttpContext.Current.Request.Form;
                    var selectedHobbies = JsonConvert.DeserializeObject<List<Hobbies>>(form.Get("selectedHobbies"));

                    user.FirstName = form.Get("firstName");
                    user.LastName = form.Get("lastName");
                    user.Email = form.Get("email");
                    user.DOB = Convert.ToDateTime(form.Get("DOB"));
                    user.Country = Convert.ToInt32(form.Get("country"));

                    var previousHobbies = db.UserHobbiesMappings.Where(x => x.UserId == userId).ToList();

                    foreach (var hobby in previousHobbies)
                    {
                        db.UserHobbiesMappings.Remove(hobby);
                    }

                    for (int i = 0; i < selectedHobbies.Count(); i++)
                    {
                        UserHobbiesMapping userHobby = new UserHobbiesMapping();
                        userHobby.HobbyId = selectedHobbies[i].value;
                        user.UserHobbiesMappings.Add(userHobby);
                    }

                    db.Entry(user).State = EntityState.Modified;
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }

        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var countries = db.Countries.Select(x => new { label = x.Name, value = x.Id }).ToList();
                    return Content(HttpStatusCode.OK, new { countries });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }

        [HttpGet]
        public IHttpActionResult GetHobbies()
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                    var hobbies = db.Hobbies.Select(x => new { label = x.Hobby1, value = x.Id }).ToList();
                    return Content(HttpStatusCode.OK, new { hobbies });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteUser(int Id)
        {
            try
            {
                using (ReactNativeSampleEntities db = new ReactNativeSampleEntities())
                {
                     var hobbies = db.UserHobbiesMappings.Where(x => x.UserId == Id).ToList();
                    foreach(var hobby in hobbies)
                    {
                        db.UserHobbiesMappings.Remove(hobby);
                    }
                    var user = db.Users.Find(Id);
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "An error occured, please try again later");
            }
        }
    }
}