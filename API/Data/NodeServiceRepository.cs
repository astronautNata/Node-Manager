using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Dtos;
using Newtonsoft.Json;

namespace API.Data
{
    public class NodeServiceRepository
    {
        private HttpClient _httpClient;
        public NodeServiceRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<NodeServiceResponseObj> pingNode(string host, int port)
        {
            try{
                var url = "http://" + host + ":" + port;
                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        var asyncString = await content.ReadAsStringAsync();
                        var nodeServiceResponse = JsonConvert.DeserializeObject<NodeServiceResponseObj>(asyncString.ToString());
                        if(nodeServiceResponse.success == true){
                            return nodeServiceResponse;
                        }
                        return null;
                    }
                }
            } catch(Exception ex){
                return null;
            }
        }

        public async Task<NodeServiceResponseObj> setStatus(string host, int port, string status){
            try{
                string endPoint = String.Empty;
                if(status == "online"){
                    endPoint = "turnOn";
                }
                if(status == "offline"){
                    endPoint = "turnOff";
                }
                if(endPoint != String.Empty){
                    var url = "http://" + host + ":" + port + "/" + endPoint;
                    using (HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, new Object()))
                    {
                        using (HttpContent content = response.Content)
                        {
                            var asyncString = await content.ReadAsStringAsync();
                            var nodeServiceResponse = JsonConvert.DeserializeObject<NodeServiceResponseObj>(asyncString.ToString());
                            if(nodeServiceResponse.success == true){
                                return nodeServiceResponse;
                            }
                            return null;
                        }
                    }
                } else {
                    return null;
                }
                
            } catch(Exception ex){
                return null;
            }
        }

        public async Task<NodeServiceResponseObj> getStatus(string host, int port){
            try{
                var url = "http://" + host + ":" + port + "/status";
                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        var asyncString = await content.ReadAsStringAsync();
                        var nodeServiceResponse = JsonConvert.DeserializeObject<NodeServiceResponseObj>(asyncString.ToString());
                        if(nodeServiceResponse.success == true){
                            return nodeServiceResponse;
                        }
                        return null;
                    }
                }
            } catch(Exception ex){
                return null;
            }
        }

        public async Task<NodeServiceResponseObj> getTelemetryData(string host, int port){
            try{
                var url = "http://" + host + ":" + port + "/telemetryMetrics";
                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        var asyncString = await content.ReadAsStringAsync();
                        var nodeServiceResponse = JsonConvert.DeserializeObject<NodeServiceResponseObj>(asyncString.ToString());
                        if(nodeServiceResponse.success == true){
                            return nodeServiceResponse;
                        }
                        return null;
                    }
                }
            } catch(Exception ex){
                return null;
            }
        }

        public async Task<NodeServiceResponseObj> setThreshold(string host, int port, double downloadThreshold, double uploadThreshold){
            try{
                var url = "http://" + host + ":" + port + "/threshold";
                var data = new {
                    downloadThreshold = downloadThreshold,
                    uploadThreshold = uploadThreshold
                };

                using (HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, data))
                {
                    using (HttpContent content = response.Content)
                    {
                        var asyncString = await content.ReadAsStringAsync();
                        var nodeServiceResponse = JsonConvert.DeserializeObject<NodeServiceResponseObj>(asyncString.ToString());
                        if(nodeServiceResponse.success == true){
                            return nodeServiceResponse;
                        }
                        return null;
                    }
                }
                
            } catch(Exception ex){
                return null;
            }
        }
    }
}