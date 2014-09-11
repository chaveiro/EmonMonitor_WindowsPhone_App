using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmonMonitor.DataAccess.Contracts
{

    [DataContract]
    public class FeedValue
    {
        [DataMember]
        public object[][] Prop { get; set; }
    }

}

