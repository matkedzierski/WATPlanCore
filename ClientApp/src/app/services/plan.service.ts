import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../environments/environment";

const API_BASE_URL = "/api/plans";

@Injectable({
  providedIn: 'root'
})
export class PlanService {

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*'
  });

  constructor(private api: HttpClient) {
  }


  getPlans() {
    return this.api.get(`${environment.apiUrl}${API_BASE_URL}`, {headers: this.headers});
  }

}
