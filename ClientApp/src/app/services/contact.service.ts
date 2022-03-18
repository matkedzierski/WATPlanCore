import { Injectable } from '@angular/core';
import {Ticket} from "../models/ticket";
import {HttpClient} from "@angular/common/http";
import {RankEntry} from "../models/rank-entry";

const API_BASE_URL = "api/contact";

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private http: HttpClient) { }

  sendTicket(ticket: Ticket){
    return this.http.post<Ticket>(`${API_BASE_URL}/ticket`, ticket);
  }
}
