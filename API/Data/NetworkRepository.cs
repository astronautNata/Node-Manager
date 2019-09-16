using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class NetworkRepository
    {
        public DataContext _context { get; set; }
        public NetworkRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Node>> getNetworkNodes(int networkId){
            Network network = await _context.Networks.FirstOrDefaultAsync(n => n.Id == networkId);

            if(network != null){
                List<Node> nodes = await _context.Network_MN_Nodes.Where(n => n.Network.Id == network.Id)
                .Select(n => n.Node)
                .ToListAsync();
                return nodes;
            }

            return null;
        }
    }
}