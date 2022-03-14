import {AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {Observable} from 'rxjs';
import {map, shareReplay} from 'rxjs/operators';
import {Plan} from "../../models/plan";
//import {CookieService} from "../../services/cookie.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {PlanComponent} from "../plan/plan.component";
import {PreferencesService} from "../../services/preferences.service";
import {EventLoader} from "../../services/event-loader.service";
import {CookieService} from "../../services/cookie.service";

@Component({
  selector: 'app-root',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {

  savedplans: Plan[] = [];
  currentPlans: Plan[] = [];
  lock: boolean = false;

  @ViewChild(PlanComponent) planComponent: PlanComponent;
  prefs: { [name: string]: boolean } = {};

  // mobile phones
  isHandset: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver,
              private cdRef: ChangeDetectorRef,
              private cookies: CookieService,
              private snackBar: MatSnackBar,
              private prefService: PreferencesService,
              private eventLoader: EventLoader) {

    prefService.prefsChanged.subscribe(m => this.prefsChanged(m));
    prefService.savePlan.subscribe(plan => this.savePlan(plan));
    eventLoader.loadingChanged.subscribe(l => {
      this.lock = l;
    });

    //recover from cookies
    this.prefs = this.cookies.getPreferences();


    //set dark mode
    if (this.prefs.dark)
      document.body.classList.add("mat-theme-dark");
    else
      document.body.classList.remove("mat-theme-dark");

    if (!this.prefs.consent) {
      this.snackBar.open("Strona do poprawnego działania wykorzystuje ciasteczka! Korzystając z WATPlanu wyrażasz zgodę na cookies.", "OK").onAction().subscribe(() => {
        this.cookies.setPref("consent", true);
      });
    }

    this.savedplans = this.cookies.getSavedPlans();

    let shown = this.cookies.getShownPlans();
    if (shown && shown.length > 0)
      this.currentPlans = this.prefs.multiple ? shown : [shown[0]];

    for (let plan of this.currentPlans) {
      this.eventLoader.loadEvents(plan.ID);
    }
    this.eventLoader.loadingChanged.next(false);
  }

  selectPlan(plan: Plan) {
    if (this.lock) return;
    let ind = this.currentPlans.findIndex(p => p.ID === plan.ID);
    if (ind > -1) {
      this.eventLoader.unloadEvents(plan.ID);
      this.currentPlans.splice(ind, 1);
      this.currentPlans = Array.from(this.currentPlans);
      this.cookies.deleteShown(plan);
    } else {
      if (this.prefs.multiple) {
        this.currentPlans = this.currentPlans.concat([plan]);
      } else {
        let prev = this.currentPlans[0];
        this.eventLoader.unloadEvents(prev?.ID);
        this.cookies.deleteShown(prev);
        this.currentPlans = [plan];
      }
      this.cookies.saveShown(plan);
      this.cdRef.detectChanges();
      this.eventLoader.loadEvents(plan.ID);
    }
  }

  ngOnInit(): void {

  }

  savePlan(plan: Plan) {
    if (this.savedplans.findIndex(p => p.ID === plan.ID) != -1) {
      this.snackBar.open("Ten plan jest już zapisany!", "OK", {duration: 2500});
    } else {
      this.savedplans.push(plan);
      this.cookies.savePlan(plan);
      console.log('main saved');
      if (this.savedplans.length == 1)
        this.selectPlan(plan);
      this.snackBar.open("Plan " + plan.Name + " został zapisany!", "OK", {duration: 2500});
    }
  }

  deletePlan(plan: Plan) {
    this.eventLoader.unloadEvents(plan.ID);
    let ind = this.savedplans.findIndex(p => p == plan);
    this.savedplans.splice(ind, 1);
    this.cookies.deleteSaved(plan);
    ind = this.currentPlans.findIndex(p => p == plan);
    if (ind >= 0) {
      this.currentPlans.splice(ind, 1);
      this.cookies.deleteShown(plan);
    }

    this.snackBar.open("Plan " + plan.Name + " został usunięty!", "OK", {duration: 2500});
  }

  prefsChanged(pref: any) {
    this.prefs[pref.name] = pref.value;
    if (!this.prefs.multiple && this.currentPlans.length > 0) {
      for (let i = 1; i < this.currentPlans.length; i++) {
        this.eventLoader.unloadEvents(this.currentPlans[i].ID);
      }
      this.currentPlans = [this.currentPlans[0]];
    }
  }

  isActive(plan: Plan) {
    return this.currentPlans.findIndex(p => p.ID === plan.ID) != -1;
  }

  ngAfterViewInit(): void {

  }
}
