using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication24.Helper;
using WebApplication24.Models;

namespace WebApplication24.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("api/Get_All_Ips_and_MAC")]
        public Task<List<string>> Get()
        {
            return GetIps.GetAllIpsandMAC();
        }

        // GET api/values/5
        // To get all IPs and MACs based on a specified subnetwork on class C
        [Route("api/Get_All_Ips_and_MAC_with_Class_C_Ip")]
        public Task<List<string>> Get(string id)
        {
            return GetIps.GetAllIpsandMAC(id);
          //  return Ip;
        }


    }
}
