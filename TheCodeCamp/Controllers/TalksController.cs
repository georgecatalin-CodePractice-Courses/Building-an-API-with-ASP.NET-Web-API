using AutoMapper;
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

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeTalks = false)
        {
            try
            {

                var result = await _repository.GetTalkByMonikerAsync(moniker, id, includeTalks);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<TalkModel>(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
               
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel model)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);

                if (ModelState.IsValid)
                {
                    var talk = _mapper.Map<Talk>(model);
                    talk.Camp = camp;

                    //Map the speaker if necessary

                    if (model.Speaker!=null)
                    {
                        var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                        if (speaker!=null)
                        {
                            talk.Speaker = speaker;
                        }
                    }

                    _repository.AddTalk(talk);

                    if (await _repository.SaveChangesAsync())
                    {
                        return CreatedAtRoute("GetTalk", new { moniker = moniker, id = model.TalkId }, _mapper.Map<TalkModel>(talk));
                    }
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Put(string moniker, int talkId, TalkModel model)
        {
            try
            {
                var talk = _mapper.Map<Talk>(model);
                if (talk==null)
                {
                    return NotFound();
                }

                _mapper.Map(model, talk);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<TalkModel>(talk));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{talkId}")]
        public async Task<IHttpActionResult> Delete(string moniker, int talkId)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, talkId, true);
                if (talk==null)
                {
                    return NotFound();
                }

                _repository.DeleteTalk(talk);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch ( Exception ex)
            {
                return InternalServerError(ex);
            }

            
        }
    }
}
