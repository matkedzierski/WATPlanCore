import { Injectable } from '@angular/core';
import {RankEntry} from "../models/rank-entry";
import {HttpClient} from "@angular/common/http";

const API_BASE_URL = "api/stats";

@Injectable({
  providedIn: 'root'
})
export class StatsService {
  constructor(private http: HttpClient) { }

  loadTop(count: number, days: number, unique: boolean){
    return this.http.get<RankEntry[]>(`${API_BASE_URL}/top/${count}/${days}/${unique}`);
  }
}
