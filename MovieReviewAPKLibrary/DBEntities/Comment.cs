using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewAPILibrary
{
    [Serializable]
    [DataContract]
    public class Comments
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int MovieId { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public string UpdatedBy { get; set; }

        [DataMember]
        public DateTime UpdatedOn { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

    }
}
