

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TheCodeCamp.Controllers
{
    public class OperationsController : ApiController
    {
        [HttpPost]
        [Route("api/refreshSettings")]
        public IHttpActionResult RefreshAppSettings()
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
