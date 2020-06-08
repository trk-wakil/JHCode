using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThirdApp.Models;

namespace ThirdApp.Controllers
{
    public class ThingsController : ApiController
    {
        List<Person> people = new List<Person>();

        public ThingsController()
        {
            people.Add(new Person { id = 1, firstName = "Tarek", lastName = "Wakil" });
            people.Add(new Person { id = 2, firstName = "Jim", lastName = "Doe" });
            people.Add(new Person { id = 3, firstName = "Joe", lastName = "Vin" });
        }



        [Route("api/Things/GetFirstNames/{userId:int}")]
        [HttpGet]
        public List<string> GetFirstNames(int userId)
        {
            var result = new List<string>();
            foreach(var p in people)
            {
                result.Add(p.firstName);
            }
            return result;
        }


        // GET: api/Things
        public List<Person> Get()
        {
            return people;
        }

        // GET: api/Things/5
        public Person Get(int id)
        {
            return people.Where(x => x.id == id).FirstOrDefault();
        }

        // POST: api/Things
        public void Post(Person value)
        {
            people.Add(value);
        }

        // PUT: api/Things/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Things/5
        public void Delete(int id)
        {
        }
    }
}
