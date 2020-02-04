﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks=false)
        {
            try
            {
                var results = await _repository.GetTalksByMonikerAsync(moniker, includeTalks);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<TalkModel>>(results));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
            
            
           
        }
    }
}
