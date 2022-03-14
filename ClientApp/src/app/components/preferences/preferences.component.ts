import {Component, OnInit} from '@angular/core';
import {PreferencesService} from "../../services/preferences.service";
import {CookieService} from "../../services/cookie.service";

//import {CookieService} from "../../services/cookie.service";

@Component({
  selector: 'app-preferences',
  templateUrl: './preferences.component.html',
  styleUrls: ['./preferences.component.css']
})
export class PreferencesComponent implements OnInit {

  prefs: { [name: string]: boolean } = {};

  constructor(private preferencesService: PreferencesService,
              private cookies: CookieService
  ) {
    this.preferencesService.prefsChanged.subscribe((pref) => {
      this.prefs[pref.name] = pref.value;
    });

    let pr = cookies.getPreferences();
    Object.keys(pr).forEach(pref => {
      this.preferencesService.prefsChanged.next({name: pref, value: pr[pref]});
    })
  }

  ngOnInit(): void {

  }


  mulToggle() {
    this.prefs.multiple = !this.prefs.multiple;
    this.preferencesService.prefsChanged.next({name: "multiple", value: this.prefs.multiple});
    this.cookies.setPref("multiple", this.prefs.multiple);
  }

  lecToggle() {
    this.prefs.lectures = !this.prefs.lectures;
    this.preferencesService.prefsChanged.next({name: "lectures", value: this.prefs.lectures});
    this.cookies.setPref("lectures", this.prefs.lectures);
  }

  weekToggle() {
    this.prefs.weekends = !this.prefs.weekends;
    this.preferencesService.prefsChanged.next({name: "weekends", value: this.prefs.weekends});
    this.cookies.setPref("weekends", this.prefs.weekends);
  }

  hoursToggle() {
    this.prefs.hours = !this.prefs.hours;
    this.preferencesService.prefsChanged.next({name: "hours", value: this.prefs.hours});
    this.cookies.setPref("hours", this.prefs.hours);
  }

  darkToggle() {
    this.prefs.dark = !this.prefs.dark;
    this.preferencesService.prefsChanged.next({name: "dark", value: this.prefs.dark});
    this.cookies.setPref("dark", this.prefs.dark);

    if (this.prefs.dark)
      document.body.classList.add("mat-theme-dark");
    else
      document.body.classList.remove("mat-theme-dark");
  }
}
