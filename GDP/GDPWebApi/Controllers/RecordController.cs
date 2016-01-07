using GDPLibrary.Entities;
using GDPWebApi.Models;
using GDPWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDPWebApi.Controllers
{
    [RoutePrefix("api/Record")]
    public class RecordController : ApiController
    {

        private RecordRepository _recordRepository;
        private RecordRepository RecordRepository { get { return _recordRepository ?? (_recordRepository = new RecordRepository()); } }


        // GET: api/Record
        public IEnumerable<Record> Get()
        {
            return RecordRepository.Get();
        }

        // POST: api/Record
        [HttpPost]
        public HttpResponseMessage Post([FromUri]RecordModel record)
        {
            var errors = new List<String>();
            if (record == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new List<String>() { "Record is missing." });
            errors = RecordRepository.Save(record.Parse());
            return Request.CreateResponse(errors.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK, errors.Any() ? errors : new List<String>() { "Record registered with success." });
        }

        // DELETE: api/Record/5
        [HttpDelete, Route("{id}")]
        public void Delete(int id)
        {
            RecordRepository.Remove(id);
        }
    }
}
