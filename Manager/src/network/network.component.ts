import { Component, OnInit } from '@angular/core';
import { NetworkService } from '../_services/network.service';
import { NodeService } from '../_services/node.service';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.css']
})
export class NetworkComponent implements OnInit {
  networkNodes: any;
  networkId: any = 1;
  addNodeFailed: any = false;

  constructor(private networkService: NetworkService, private nodeService: NodeService) {}

  ngOnInit() {
    this.loadNetwork();
  }

  loadNetwork(){
    this.networkService.getNetwork(this.networkId).subscribe(
      response => {
        this.networkNodes = response;
        console.log(response);
      },
      error => {
        console.log('Failed to load network', error);
      }
    );
  }

  onNodeStatusChange(node: any, newStatus: any) {
    this.nodeService.changeStatus(node.id, newStatus).subscribe(response => {
      node = Object.assign(node, response);
    }, error => {
      console.log('Failed to change node status', error);
      node.status = 'unavailable';
    });
  }

  onNodeRefreshClick(node: any){
    this.nodeService.loadNodeTelemetryData(node.id).subscribe(response => {
      node = Object.assign(node, response);
    }, error => {
      console.log('Failed to load node telemetry data', error);
      node.status = 'unavailable';
    });
  }

  onNodeRemoveClick(node: any) {
    this.nodeService.removeNodeFromNetwork(node.id).subscribe(response => {
      var thisNodeIdx = -1;
      this.networkNodes.forEach((n: any, index: any) => {
        if (n.id === node.id) {
          thisNodeIdx = index;
        }
      });
      if(thisNodeIdx !== -1){
        this.networkNodes.splice(thisNodeIdx, 1);
      }
    }, error => {
      console.log('Failed to remove node', error);
      node.status = 'unavailable';
    });
  }

  onAddNodeFailed(){
    this.addNodeFailed = true;
  }
}
