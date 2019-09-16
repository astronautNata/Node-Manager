import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NodeService {
  baseUrl = 'http://localhost:5000/api/node/';

constructor(private http: HttpClient) { }

changeStatus(nodeId: any, newStatus: any) {
  return this.http.post(this.baseUrl + 'setStatus', {nodeId, newStatus}).pipe(
    map((response: any) => {
      console.log(response);
      return response;
    }));
  }

  loadNodeTelemetryData(nodeId: any) {
    return this.http.get(this.baseUrl + 'getTelemetryMetrics/' + nodeId).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  removeNodeFromNetwork(nodeId){
    return this.http.delete(this.baseUrl + 'removeNode/' + nodeId, {}).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  addNodeToNetwork(nodeObj: any) {
    return this.http.post(this.baseUrl + 'addNode', nodeObj).pipe(
      map((response: any) => {
        return response;
      })
    );
  }
}
