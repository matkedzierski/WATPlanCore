import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Event} from "../models/event";
import {EventColor} from "calendar-utils";
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class EventLoader {

  private API_BASE_URL = "//watplan.somee.com/api/events";
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*'
  });

  loadingChanged = new Subject<boolean>();
  eventsLoaded = new Subject<CalendarEvent[]>();
  eventsUnloaded = new Subject<string>();


  constructor(private api: HttpClient) {
  }

  getEvents(planID: string): Observable<Event[]> {
    return this.api.get<Event[]>(this.API_BASE_URL + '?id=' + planID, {headers: this.headers});
  }

  mapper = (event: Event, planID: string) => {
    let ev = new CalendarEvent();
    ev.color = {primary: event.Color, secondary: event.Color};
    ev.title = event.Name;
    ev.id = event.ID;
    ev.start = new Date(event.StartTime);
    ev.end = new Date(event.EndTime);
    ev.meta = {
      event: event,
      planID: planID
    };
    return ev;
  };


  loadEvents(id: string) {
    this.loadingChanged.next(true);
    this.getEvents(id).subscribe((events) => {
      let calEvs = [];
      for (let e of events) {
        calEvs.push(this.mapper(e, id));
      }
      this.eventsLoaded.next(calEvs);
      this.loadingChanged.next(false);
    });
  }

  unloadEvents(id: string) {
    this.eventsUnloaded.next(id);
  }
}

export class CalendarEvent implements CalendarEvent {
  color: EventColor;
  end: Date;
  id: string | number;
  meta: any;
  start: Date;
  title: string;
  cssClass: string = 'event';
}

