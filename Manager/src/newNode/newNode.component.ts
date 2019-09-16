import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { NodeService } from '../_services/node.service';

@Component({
  selector: 'app-newNode',
  templateUrl: './newNode.component.html',
  styleUrls: ['./newNode.component.css']
})
export class NewNodeComponent implements OnInit {
  @Output() onAddNode: EventEmitter<any> = new EventEmitter();
  @Output() onAddNodeFailed: EventEmitter<any> = new EventEmitter();
  
  constructor(private nodeService: NodeService) { }
    host: any;
    port: any;
    uploadThreshold: any;
    downloadThreshold: any;

  ngOnInit() {
  }

  onAddNewNode() {
    const nodeObj = {
      groupId: 1,
      networkId: 1,
      host: this.host,
      port: this.port,
      uploadThreshold: this.uploadThreshold,
      downloadThreshold: this.downloadThreshold
    }

    this.nodeService.addNodeToNetwork(nodeObj).subscribe(response => {
      if(response == null){
        this.onAddNodeFailed.emit();
        console.log("Node does not exist.");
      } else {
        this.onAddNode.emit();
      }
    }, error => {
      this.onAddNodeFailed.emit();
      console.log('Failed to add node', error);
    });
  }
}
