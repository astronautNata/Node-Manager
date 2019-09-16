using System;
using System.Collections.Generic;

namespace API.Models
{
    public class Network
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}