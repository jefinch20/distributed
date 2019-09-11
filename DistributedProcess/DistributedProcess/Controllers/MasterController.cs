using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedProcess.Managers;
using Microsoft.AspNetCore.Mvc;

namespace DistributedProcess.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        IMasterManager _masterManager;

        public MasterController(IMasterManager masterManager)
        {
            _masterManager = masterManager;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return _masterManager.GetMasterStatus();
        }
    }
}
