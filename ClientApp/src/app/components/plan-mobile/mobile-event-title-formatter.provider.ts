import {Inject, Injectable, LOCALE_ID} from '@angular/core';
import {CalendarEvent, CalendarEventTitleFormatter} from 'angular-calendar';
import {formatDate} from '@angular/common';

@Injectable()
export class MobileEventTitleFormatter extends CalendarEventTitleFormatter {
  constructor(@Inject(LOCALE_ID) private locale: string) {
    super();
  }

  // you can override any of the methods defined in the parent class

  month(event: CalendarEvent): string {
    return `<b>${formatDate(event.start, 'HH:mm', this.locale)} - ${formatDate(event.end, 'HH:mm', this.locale)}</b> &nbsp;${event.title} <small><i>(${event.meta.event.Type})</i></small>`;
  }

  week(event: CalendarEvent): string {
    return `<span class="break center"><span>${event.meta.event.Shortcut}</span></span><span class="break center">(${event.meta.event.TypeShortcut})</span><span class="break center">[${event.meta.event.Number}]</span>`;
  }

  weekTooltip(event: CalendarEvent, title: string): string {
    return `${event.title} (${event.meta.event.Type}) [${event.meta.event.Number}]`;
  }

  day(event: CalendarEvent): string {
    return `<span class="break"><span>${event.title}</span></span><span class="break">(${event.meta.event.Type}) [${event.meta.event.Number}]</span><span class="break">${formatDate(event.start, 'HH:mm', this.locale)} - ${formatDate(event.end, 'HH:mm', this.locale)}</span>`;
  }

  dayTooltip(event: CalendarEvent, title: string): string {
    return `${event.title} (${event.meta.event.Type}) [${event.meta.event.Number}]`;
  }
}