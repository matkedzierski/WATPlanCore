import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class PlanService {

  private API_BASE_URL = "/api/plans";
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*'
  });

  constructor(private api: HttpClient) {
  }


  getPlans() {
    return this.api.get(this.API_BASE_URL, {headers: this.headers});
  }

}
