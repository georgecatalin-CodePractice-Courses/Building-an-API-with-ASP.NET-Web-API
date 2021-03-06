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
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks=false)
        {
            try
            {
                var result =await  _repository.GetAllCampsAsync(includeTalks);

                //Mapping
                var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                //TODO Add Error Logging
                return InternalServerError(ex);
            }

        }

        [Route("{moniker}", Name="GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks=false)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker, includeTalks);

                if (result==null)
                {
                    return NotFound();
                }


                var modelResult = _mapper.Map<CampModel>(result);

                return Ok(modelResult);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        //version of searching in the API by means of Query Strings

        [Route("SearchByEventDate")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate,bool includeTalks = false)
        {
            var result =await  _repository.GetAllCampsByEventDate(eventDate, includeTalks);

            return Ok(_mapper.Map<IEnumerable<CampModel>>(result));
        }

        //version of searching in the API by means of routing

        [Route("SearchByEventDate/{eventDate:DateTime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDateRoute(DateTime eventDate, bool includeTalks = false)
        {
            var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);

            return Ok(_mapper.Map<IEnumerable<CampModel>>(result));
        }

        [Route()]
        [HttpPost]
        public async Task<IHttpActionResult> Post(CampModel model)
        {
            try
            {
                if (await _repository.GetCampAsync(model.Moniker)!=null)
                {
                    ModelState.AddModelError("Moniker", "This moniker already exists!");

                }


                if (ModelState.IsValid)
                {
                    var camp = _mapper.Map<Camp>(model);

                    _repository.AddCamp(camp);

                    //two versions of setting the location, both can be used

                    //1

                    //if (await _repository.SaveChangesAsync())
                    //{
                    //    var newModel = _mapper.Map<CampModel>(camp);
                    //    var location = Url.Link("GetCamp", new { moniker = newModel.Moniker });
                    //    return Created(location, newModel);
                    //}

                    //2

                    if (await _repository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CampModel>(camp);

                        return CreatedAtRoute("GetCamp", new { moniker = newModel.Moniker }, newModel);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel campModel)
        {
            try
            {
                var camp =await _repository.GetCampAsync(moniker);

                if (camp==null)
                {
                    return NotFound();
                }

                _mapper.Map(campModel, camp);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CampModel>(camp));
                }
                else
                {
                    return InternalServerError();
                }
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp =await _repository.GetCampAsync(moniker);

                if (camp==null)
                {
                    return NotFound();
                }

                _repository.DeleteCamp(camp);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
