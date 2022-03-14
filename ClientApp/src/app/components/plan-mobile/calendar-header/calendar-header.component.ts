import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CalendarView} from "angular-calendar";


@Component({
  selector: 'app-plan-calendar-header-mobile',
  templateUrl: './calendar-header.component.html',
  styleUrls: ['./calendar-header.component.css']
})
export class CalendarHeaderMobileComponent implements OnInit {

  @Input() view: CalendarView;

  @Input() viewDate: Date;

  @Input() locale: string = 'pl';

  @Output() viewChange = new EventEmitter<CalendarView>();

  @Output() viewDateChange = new EventEmitter<Date>();

  CalendarView = CalendarView;
  now: Date = new Date();


  constructor() {

  }

  ngOnInit(): void {

  }
}
