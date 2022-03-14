import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  OnChanges,
  OnDestroy,
  OnInit,
  ViewEncapsulation
} from '@angular/core';


import {CalendarDateFormatter, CalendarEventTitleFormatter,} from 'angular-calendar';
import {MobileEventTitleFormatter} from "./mobile-event-title-formatter.provider";
import {DOCUMENT} from "@angular/common";
import {EventLoader} from "../../services/event-loader.service";
import {PlanComponent} from "../plan/plan.component";
import {MobileDateFormatter} from "./mobile-date-formatter.provider";
import {PreferencesService} from "../../services/preferences.service";
import {CookieService} from "../../services/cookie.service";


@Component({
  selector: 'app-plan-mobile',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './plan-mobile.component.html',
  styleUrls: ['./plan-mobile.component.scss'],
  providers: [
    {
      provide: CalendarEventTitleFormatter,
      useClass: MobileEventTitleFormatter,
    },
    {
      provide: CalendarDateFormatter,
      useClass: MobileDateFormatter,
    },
  ],
  encapsulation: ViewEncapsulation.None,
})
export class PlanMobileComponent extends PlanComponent implements OnInit, OnDestroy, OnChanges {

  constructor(@Inject(DOCUMENT) protected document: Document,
              public eventLoader: EventLoader,
              protected cdRef: ChangeDetectorRef,
              protected preferences: PreferencesService,
              protected cookies: CookieService) {
    super(document, eventLoader, cdRef, preferences, cookies);
  }


}
