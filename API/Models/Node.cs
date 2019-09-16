using System;

namespace API.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? NodeStartTime { get; set; }
        public DateTime? StatusLastChangedDate { get; set; }
        public double? DownloadSpeed { get; set; }
        public double? UploadSpeed { get; set; }
        public double? DownloadSpeedThreshold { get; set; }
        public double? UploadSpeedThreshold { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}