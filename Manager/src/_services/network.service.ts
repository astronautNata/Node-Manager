import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NetworkService {
  baseUrl = 'http://localhost:5000/api/network/';

constructor(private http: HttpClient) { }

getNetwork(networkId: any) {
  return this.http.get(this.baseUrl + 'getNetwork/' + networkId).pipe(
    map((response: any) => {
      return response;
    }));
  }
}
