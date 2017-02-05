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
    public class Movie
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Synopsis { get; set; }

        [DataMember]
        public string ContentRating { get; set; }

        [DataMember]
        public string Duration { get; set; }

        [DataMember]
        public string Director { get; set; }

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
