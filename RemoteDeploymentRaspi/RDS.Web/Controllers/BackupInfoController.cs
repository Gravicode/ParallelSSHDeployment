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
    public class BackupInfoController : Controller
    {
        RDSDatabase db = null;


        public BackupInfoController()
        {
            db = ObjectContainer.Get<RDSDatabase>();
        }

        [HttpPost]
        public IActionResult Create([FromBody]BackupInfo item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            item.Id = db.GetSequence<BackupInfo>();
            db.InsertData<BackupInfo>(item);

            return Ok(new { id = item.Id });
        }


        [HttpPut("{id}")]
        public IActionResult PutData([FromRoute] int id, [FromBody] BackupInfo item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (item.Id > 0)
                db.InsertData<BackupInfo>(item);

            return Ok( new { id = item.Id });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteData([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            db.DeleteData<BackupInfo>(id);

            return Ok(new { id = id });
        }

        [HttpGet("[action]")]
        public IEnumerable<RDS.Data.BackupInfo> GetBackupInfos()
        {
            return db.GetAllData<RDS.Data.BackupInfo>();
        }


    }
}
