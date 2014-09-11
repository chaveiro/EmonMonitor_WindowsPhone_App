using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmonMonitor.DataAccess.Contracts
{

    [DataContract]
    public class FeedItem
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "datatype")]
        public string Datatype { get; set; }
        
        [DataMember(Name = "tag")]
        public string Tag { get; set; }

        [DataMember(Name = "time")]
        public long Time { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
        
        [DataMember(Name = "public")]
        public bool Ispublic { get; set; }
    }

}
