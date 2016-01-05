using GDPLibrary.Entities;
using GDPWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDPWebApi.Controllers
{
    public class RecordController : ApiController
    {

        private RecordRepository _recordRepository;
        private RecordRepository RecordRepository { get { return _recordRepository ?? (_recordRepository = new RecordRepository()); } }


        // GET: api/Record
        public IEnumerable<Record> Get()
        {
            return RecordRepository.Get();
        }

        // GET: api/Record/5
        public Record Get(int id)
        {
            return RecordRepository.Get(id);
        }

        // POST: api/Record
        public IEnumerable<String> Post([FromUri]Record record)
        {
            return RecordRepository.Save(record);
        }

        // PUT: api/Record/5
        public IEnumerable<String> Put(int id, [FromUri]Record record)
        {
            return RecordRepository.Save(record);
        }

        // DELETE: api/Record/5
        public void Delete(int id)
        {
            RecordRepository.Remove(id);
        }
    }
}
