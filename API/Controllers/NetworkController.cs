using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private NetworkRepository _repo { get; set; }

        public NetworkController(NetworkRepository repo){
            _repo = repo;
        }

        [HttpGet("getNetwork/{networkId}")]
        public async Task<IActionResult> getNetwork(int networkId){
            try{
                var nodes = await _repo.getNetworkNodes(networkId);
                
                if(nodes == null){
                    return BadRequest("Network does not exist");
                }

                return Ok(nodes);
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }
    }
}