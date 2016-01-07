using GDPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GDPWebApi.Models
{
    [DataContract]
    public class RecordModel
    {
        [DataMember]
        public int IdRecord { get; set; }
        [DataMember]
        public int IdCategory { get; set; }
        [DataMember]
        public int IdUser { get; set; }
        [DataMember]
        public String Entry { get; set; }

        public Record Parse()
        {
            return new Record()
            {
                IdRecord = this.IdRecord,
                IdCategory = this.IdCategory,
                IdUser = this.IdUser,
                Entry = Convert.FromBase64String(this.Entry ?? "")
            };
        }
    }
}