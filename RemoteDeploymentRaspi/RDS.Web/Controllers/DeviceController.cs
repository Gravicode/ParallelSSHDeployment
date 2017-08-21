using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rds.Web.Tools;
using Rds.Web.Helpers;
using RDS.Data;
using Newtonsoft.Json;

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

        [HttpPost]
        public IActionResult Create([FromBody]DeviceIdentity item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            item.Id = db.GetSequence<DeviceIdentity>();
            db.InsertData<DeviceIdentity>(item);

            return Ok(new { id = item.Id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutData([FromRoute] int id, [FromBody] DeviceIdentity item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (item.Id > 0) 
                db.InsertData<DeviceIdentity>(item);

            return Ok( new { id = item.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            db.DeleteData<DeviceIdentity>(id);

            return Ok(new { id =id });
        }

        [HttpGet("[action]")]
        public IEnumerable<RDS.Data.DeviceIdentity> GetDevices()
        {
           return  db.GetAllData<RDS.Data.DeviceIdentity>();
        }

       
    }
}
