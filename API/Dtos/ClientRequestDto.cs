namespace API.Dtos
{
    public class ClientRequestDto
    {
        public string host { get; set; }
        public int port { get; set; }
        public string userName { get; set; }
        public int networkId { get; set; }
        public int groupId { get; set; }
        public double uploadThreshold { get; set; }
        public double downloadThreshold { get; set; }
    }
}