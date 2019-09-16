using System;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class NodeRepository
    {
        public DataContext _context { get; set; }
        public NodeRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Node> GetNodeById(int nodeId){
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId);
            return existingNode;
        }

        public async Task<Node> AddNode(string host, int port, string userName, string status, int networkId, DateTime? startTime, double uploadThreshold, double downloadThreshold)
        {
            //first check if node on this address exists
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Host == host && n.Port == port);

            if (existingNode == null)
            {
                var newNode = new Node
                {
                    Host = host,
                    Port = port,
                    Status = status,
                    NodeStartTime = startTime,
                    UploadSpeedThreshold = uploadThreshold,
                    DownloadSpeedThreshold = downloadThreshold,
                    CreatedBy = userName,
                    CreatedDate = new DateTime()
                };
                _context.Nodes.Add(newNode);
                _context.SaveChanges();

                //add this node to a network
                var network = await _context.Networks.FirstOrDefaultAsync(n => n.Id == networkId);
                if(network != null){
                    var nodeInNetwork = new Network_MN_Nodes{
                        Network = network,
                        Node = newNode
                    };
                    _context.Network_MN_Nodes.Add(nodeInNetwork);
                    _context.SaveChanges();
                }

                return newNode;
            }
            else
            {
                return null;
            }
        }

         public async Task<bool> RemoveNode(int nodeId)
        {
            //first check if node on this address exists
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId);

            if (existingNode != null)
            {
                //remove this node from a network(s)
                var nodeInNetwork = await _context.Network_MN_Nodes.Where(n => n.Node.Id == nodeId).ToListAsync();
                foreach(var node in nodeInNetwork){
                    _context.Network_MN_Nodes.Remove(node);
                }

                //remove this node
                _context.Nodes.Remove(existingNode);
                
                _context.SaveChanges();

                return true;
            }
            
            return false;
            
        }

        public async Task<bool> SetNodeStatus(int nodeId, string newStatus, DateTime? statusLastChangedDate){
            
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId);

            if(existingNode != null)
            {
                existingNode.Status = newStatus;
                existingNode.StatusLastChangedDate = statusLastChangedDate;
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<string> GetNodeStatus(int nodeId){
            
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId);

            if(existingNode != null)
            {
                return existingNode.Status;
            }

            return "";
        }
        
        public async Task<Node> setNodeTelemetryMatrics(int nodeId, double? downloadSpeed, double? uploadSpeed){
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId); 

            if(existingNode != null){
                existingNode.DownloadSpeed = downloadSpeed;
                existingNode.UploadSpeed = uploadSpeed;
                existingNode.UpdatedDate = new DateTime();

                _context.SaveChanges();
            }

            return existingNode;
        }


        public async Task<Node> setThreshold(int nodeId, double? downloadThreshold, double? uploadThreshold){
            var existingNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId); 

            if(existingNode != null){
                existingNode.DownloadSpeedThreshold = downloadThreshold;
                existingNode.UploadSpeedThreshold = uploadThreshold;
                existingNode.UpdatedDate = new DateTime();

                _context.SaveChanges();
            }

            return existingNode;
        }
    }
}