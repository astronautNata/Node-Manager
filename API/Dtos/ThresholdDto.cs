namespace API.Dtos
{
    public class ThresholdDto
    {
        public int nodeId { get; set; }
        public double downloadThreshold { get; set; }
        public double uploadThreshold { get; set; }
    }
}