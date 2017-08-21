using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rds.Web.Tools;
using Rds.Web.Helpers;

namespace WebApplicationBasic.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        RDSDatabase db = null;

     
        public DeviceController()
        {
            db = ObjectContainer.Get<RDSDatabase>();
        }
        [HttpGet("[action]")]
        public IEnumerable<RDS.Data.DeviceIdentity> GetDevices()
        {
           return  db.GetAllData<RDS.Data.DeviceIdentity>();
        }

       
    }
}
