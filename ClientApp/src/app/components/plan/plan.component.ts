import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  ViewEncapsulation
} from '@angular/core';


import {CalendarDateFormatter, CalendarEventTitleFormatter, CalendarView,} from 'angular-calendar';
import {isBefore, isSameDay, isSameMonth} from 'date-fns';
import {CustomEventTitleFormatter} from "./custom-event-title-formatter.provider";
import {DOCUMENT} from "@angular/common";
import {CalendarEvent, EventLoader} from "../../services/event-loader.service";
import {Subscription} from "rxjs";
import {DateFormatter} from "./date-formatter.provider";
import {PreferencesService} from "../../services/preferences.service";
import {CookieService} from "../../services/cookie.service";


@Component({
  selector: 'app-plan',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.scss'],
  providers: [
    {
      provide: CalendarEventTitleFormatter,
      useClass: CustomEventTitleFormatter,
    },
    {
      provide: CalendarDateFormatter,
      useClass: DateFormatter,
    },
  ],
  encapsulation: ViewEncapsulation.None,
})
export class PlanComponent implements OnInit, OnDestroy, OnChanges {
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  shownEvents: CalendarEvent[] = [];
  isLoading: boolean = true;
  lastDay: any;
  loadingSubscription: Subscription;
  eventsUnloadedSubscription: Subscription;
  eventsLoadedSubscription: Subscription;
  prefs: { [name: string]: boolean } = {};

  constructor(@Inject(DOCUMENT) protected document: Document,
              public eventLoader: EventLoader,
              protected cdRef: ChangeDetectorRef,
              protected preferences: PreferencesService,
              protected cookies: CookieService) {
    preferences.prefsChanged.subscribe(m => this.prefsChanged(m));
    this.prefs = this.cookies.getPreferences();
  }

  ngOnInit(): void {
    //this.document.body.classList.add('dark-theme');
    this.loadingSubscription = this.eventLoader.loadingChanged.subscribe((l) => {
      this.isLoading = l;
      this.cdRef.detectChanges();
    });

    this.eventsUnloadedSubscription = this.eventLoader.eventsUnloaded.subscribe((id) => {
      this.events = this.events.filter(e => e.meta.planID != id);
      this.shownEvents = this.prefs.lectures ? this.events.filter(p => p.meta.event.Type.toLowerCase() !== "wykład") : this.events;
      this.cdRef.detectChanges()
    });


    this.eventsLoadedSubscription = this.eventLoader.eventsLoaded.subscribe(events => {
      this.events = this.events.concat(events).sort((a, b) => isBefore(a.start, b.start) ? -1 : 1);
      this.shownEvents = this.prefs.lectures ? this.events.filter(p => p.meta.event.Type.toLowerCase() !== "wykład") : this.events;
      this.cdRef.detectChanges();
    });
  }

  ngOnDestroy(): void {
    //this.document.body.classList.remove('dark-theme');
    this.eventsUnloadedSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.eventsLoadedSubscription.unsubscribe();
  }

  activeDayIsOpen: boolean = true;

  dayClicked(day?: any): void {
    if (!day) day = this.lastDay;
    this.lastDay = day;
    let date = new Date(day.date);
    let events = day.events;
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
        this.viewDate = date;
      }
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
  }


  viewDateChanged($event: Date) {
    this.viewDate = $event;
    this.activeDayIsOpen = false;
  }


  prefsChanged(pref: any) {
    this.prefs[pref.name] = pref.value;
    this.shownEvents = this.prefs.lectures ? this.events.filter(p => p.meta.event.Type.toLowerCase() !== "wykład") : this.events;
    this.cdRef.detectChanges();
  }
}

