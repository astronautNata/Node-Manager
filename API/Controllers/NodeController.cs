using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Threading.Tasks;
using API.Dtos;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private NodeRepository _repo;
        private NodeServiceRepository _nodeService;

        public NodeController(NodeRepository repo, NodeServiceRepository nodeService){
            _repo = repo;
            _nodeService = nodeService;
        }

        [HttpPost("addNode")]
        public async Task<IActionResult> addNode(ClientRequestDto clientRequestDto){
            try{
                var nodeServiceResponse = await _nodeService.pingNode(clientRequestDto.host, clientRequestDto.port);
                if(nodeServiceResponse != null){
                    var thresholdStatus = await _nodeService.setThreshold(clientRequestDto.host, clientRequestDto.port, clientRequestDto.downloadThreshold, clientRequestDto.uploadThreshold);
                    if(thresholdStatus != null && thresholdStatus.success == true){
                        var newNode = await _repo.AddNode(clientRequestDto.host, clientRequestDto.port, clientRequestDto.userName, nodeServiceResponse.status, clientRequestDto.networkId, nodeServiceResponse.startTime, clientRequestDto.uploadThreshold, clientRequestDto.downloadThreshold);
                        if(newNode != null){
                            Ok(newNode);
                        } else {
                            BadRequest("Node already exists in this network");
                        }
                    } else {
                        BadRequest("Failed to update threshold values");
                    }
                }
                return Ok(nodeServiceResponse);
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("removeNode/{nodeId}")]
        public async Task<IActionResult> removeNode(int nodeId){
            try{
                var actionStatus = await _repo.RemoveNode(nodeId);
                if(actionStatus == true){
                    return Ok();
                } else {
                    return BadRequest("Node does not exist.");
                }
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("setStatus")]
        public async Task<IActionResult> setStatus(NodeRequestDto nodeRequestDto){
            try{
                var node = await _repo.GetNodeById(nodeRequestDto.nodeId);
                if(node != null){
                    var actionStatus = await _nodeService.setStatus(node.Host, node.Port, nodeRequestDto.newStatus);
                    if(actionStatus != null && actionStatus.success == true){
                        var dbStatus = _repo.SetNodeStatus(nodeRequestDto.nodeId, nodeRequestDto.newStatus, actionStatus.statusLastChangedDate);
                        if(dbStatus.Result == true){
                            //this is in case node was unavailable and after start up it has no thresholds in it's memory
                            if(actionStatus.downloadSpeedThreshold == null){
                                actionStatus.downloadSpeedThreshold = node.DownloadSpeedThreshold;
                            }
                            if(actionStatus.uploadSpeedThreshold == null){
                                actionStatus.uploadSpeedThreshold = node.UploadSpeedThreshold;
                            }
                            return Ok(actionStatus);
                        } else {
                            return BadRequest("Failed to change status in Db.");
                        }
                    } else {
                        var dbStatus = _repo.SetNodeStatus(nodeRequestDto.nodeId, "unavailable", DateTime.Now);
                        return BadRequest("Failed to change status on node.");
                    }
                } else {
                    return BadRequest("Node does not exist.");
                }
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getStatus/{nodeId}")]
        public async Task<IActionResult> getStatus(int nodeId){
            try{
                var node = await _repo.GetNodeById(nodeId);
                if(node != null){
                    var actionStatus = await _nodeService.getStatus(node.Host, node.Port);
                    if(actionStatus != null && actionStatus.success == true){
                        return Ok(actionStatus);
                    } else {
                        return BadRequest("Failed to get status from node");
                    }  
                } else {
                    return BadRequest("Node does not exist.");
                }
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("getTelemetryMetrics/{nodeId}")]
        public async Task<IActionResult> telemetryMetrics(int nodeId){
            try{
                var node = await _repo.GetNodeById(nodeId);
                if(node != null){
                    var actionStatus = await _nodeService.getTelemetryData(node.Host, node.Port);
                    if(actionStatus != null && actionStatus.success == true){
                        var dbStatus = await _repo.setNodeTelemetryMatrics(nodeId, actionStatus.downloadSpeed, actionStatus.uploadSpeed);
                        if(dbStatus != null){
                            //this is in case node was unavailable and after start up it has no thresholds in it's memory
                            if(actionStatus.downloadSpeedThreshold == null){
                                actionStatus.downloadSpeedThreshold = node.DownloadSpeedThreshold;
                            }
                            if(actionStatus.uploadSpeedThreshold == null){
                                actionStatus.uploadSpeedThreshold = node.UploadSpeedThreshold;
                            }
                            return Ok(actionStatus);
                        } else {
                            return BadRequest("Failed to update Node telemetry data in db.");
                        }
                        
                    } else {
                        var dbStatus = _repo.SetNodeStatus(nodeId, "unavailable", DateTime.Now);
                        return BadRequest("Failed to get telemtry data from node.");
                    }  
                } else {
                    return BadRequest("Node does not exist.");
                }
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("setThreshold")]
        public async Task<IActionResult> telemetryMetrics(ThresholdDto thresholdDto){
            try{
                var node = await _repo.GetNodeById(thresholdDto.nodeId);
                if(node != null){
                    var actionStatus = await _nodeService.setThreshold(node.Host, node.Port, thresholdDto.downloadThreshold, thresholdDto.uploadThreshold);
                    if(actionStatus != null && actionStatus.success == true){
                        var dbStatus = await _repo.setThreshold(thresholdDto.nodeId, thresholdDto.downloadThreshold, thresholdDto.uploadThreshold);
                        if(dbStatus != null){
                            return Ok(actionStatus);
                        } else {
                            return BadRequest("Failed to update Node threshold data in db.");
                        }
                    } else {
                        var dbStatus = _repo.SetNodeStatus(thresholdDto.nodeId, "unavailable", DateTime.Now);
                        return BadRequest("Failed to set node's threshold data.");
                    }  
                } else {
                    return BadRequest("Node does not exist.");
                }
            } catch(Exception ex){
                return StatusCode(500, "Internal server error");
            }
        }
    }
}