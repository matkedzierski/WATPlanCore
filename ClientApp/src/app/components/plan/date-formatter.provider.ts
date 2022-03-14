import {CalendarDateFormatter, DateFormatterParams} from 'angular-calendar';
import {formatDate} from '@angular/common';
import {Injectable} from '@angular/core';
import {endOfWeek, startOfWeek} from "date-fns";

@Injectable()
export class DateFormatter extends CalendarDateFormatter {
  // you can override any of the methods defined in the parent class
  months = [
    "Styczeń",
    "Luty",
    "Marzec",
    "Kwiecień",
    "Maj",
    "Czerwiec",
    "Lipiec",
    "Sierpień",
    "Wrzesień",
    "Październik",
    "Listopad",
    "Grudzień"
  ];

  monthViewTitle({date, locale}: DateFormatterParams): string {
    return this.months[date.getMonth()] + " " + formatDate(date, "YYYY", locale);
  }

  weekViewTitle({date, locale, weekStartsOn, excludeDays, daysInWeek,}: DateFormatterParams): string {
    if (!weekStartsOn) weekStartsOn = 1;
    let start = startOfWeek(date, {weekStartsOn: weekStartsOn as 0 | 1 | 2 | 3 | 4 | 5 | 6});
    let stMonth = formatDate(start, "MMMM", locale);
    let stDate = start.getDate();

    let end = endOfWeek(date, {weekStartsOn: weekStartsOn as 0 | 1 | 2 | 3 | 4 | 5 | 6});
    let endMonth = formatDate(end, "MMMM", locale);
    let endDate = end.getDate();

    if (stMonth === endMonth) {
      return stDate + " - " + endDate + " " + stMonth;
    } else {
      return stDate + " " + stMonth + " - " + endDate + " " + endMonth;
    }
  }

  dayViewTitle({date, locale}: DateFormatterParams): string {
    return formatDate(date, "dd MMMM yyyy (EEEE)", locale);
  }

  weekViewColumnHeader({date, locale}: DateFormatterParams): string {
    return formatDate(date, "EEEE", locale);
  }

  weekViewColumnSubHeader({date, locale,}: DateFormatterParams): string {
    return formatDate(date, "dd.MM", locale);
  }

  monthViewColumnHeader({date, locale}: DateFormatterParams): string {
    return formatDate(date, "EEEE", locale);
  }

  public dayViewHour({date, locale}: DateFormatterParams): string {
    return formatDate(date, 'HH:mm', locale);
  }

  public weekViewHour({date, locale}: DateFormatterParams): string {
    return this.dayViewHour({date, locale});
  }
}
