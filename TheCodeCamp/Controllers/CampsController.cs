using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;

namespace TheCodeCamp.Controllers
{
    public class CampsController : ApiController
    {
        private readonly ICampRepository _repository;

        public CampsController(ICampRepository repository)
        {
            this._repository = repository;
        }
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result =await  _repository.GetAllCampsAsync();

                //return BadRequest("Whoops...I have made it all wrong.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                //TODO Add Error Logging
                return InternalServerError(ex);
            }

        }
    }
}
