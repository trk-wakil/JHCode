using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ThirdApp.Controllers
{
    public class ValuesController : ApiController
    {

        List<string> someStrs = new List<string>();
        

        public ValuesController()
        {
            someStrs.Add("Val_a");
            someStrs.Add("Val_b");
            someStrs.Add("Val_c");
            someStrs.Add("Val_d");
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return someStrs;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return someStrs[id];
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
            someStrs.Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            someStrs[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            someStrs.RemoveAt(id);
        }
    }
}
