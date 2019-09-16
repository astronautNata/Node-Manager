using System;

namespace API.Dtos
{
    public class NodeServiceResponseObj
    {
        public bool success { get; set; }
        public string host { get; set; }
        public int? port { get; set; }
        public string status { get; set; }
        public DateTime? startTime { get; set; }
        public string activityTime { get; set; }
        public DateTime? statusLastChangedDate { get; set; }
        public string statusActivity { get; set; }
        public double? downloadSpeed { get; set; }
        public double? uploadSpeed { get; set; }
        public double? uploadSpeedThreshold { get; set; }
        public double? downloadSpeedThreshold { get; set; }
        public int? uploadThresholdStatus { get; set; }
        public string uploadThresholdStatusMessage { get; set; }
        public int? downloadThresholdStatus { get; set; }
        public string downloadThresholdStatusMessage { get; set; }
    }
}