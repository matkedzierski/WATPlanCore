import {Injectable} from '@angular/core';
import {Plan} from "../models/plan";

@Injectable()
export class CookieService {

  constructor() {
  }

  public getCookie(name: string) {
    let ca: Array<string> = document.cookie.split(';');
    let caLen: number = ca.length;
    let cookieName = `${name}=`;
    let c: string;

    for (let i: number = 0; i < caLen; i += 1) {
      c = ca[i].replace(/^\s+/g, '');
      if (c.indexOf(cookieName) == 0) {
        return c.substring(cookieName.length, c.length);
      }
    }
    return '';
  }

  public deleteCookie(name: string) {
    this.setCookie(name, "", -1);
  }

  public setCookie(name: string, value: string, expireDays: number, path: string = "") {
    let d: Date = new Date();
    d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
    let expires: string = "expires=" + d.toUTCString();
    document.cookie = name + "=" + value + "; " + expires + (path.length > 0 ? "; path=" + path : "");
  }

  storeSavedPlans(savedplans: Plan[]) {
    let val = JSON.stringify(savedplans);
    this.setCookie("saved_plans", val, 365);
  }

  getSavedPlans() {
    let val = this.getCookie("saved_plans");
    return val ? JSON.parse(val) as Plan[] : [];
  }

  savePlan(plan: Plan) {
    this.storeSavedPlans(this.getSavedPlans().concat(plan));
  }

  deleteSaved(plan: Plan) {
    let saved = this.getSavedPlans();
    let ind = saved.findIndex(p => p.ID === plan.ID);
    saved.splice(ind, 1);
    this.storeSavedPlans(saved);
  }


  storeShownPlans(currentPlans: Plan[]) {
    this.setCookie("shown_plans", JSON.stringify(currentPlans), 365);
  }

  getShownPlans() {
    let val = this.getCookie("shown_plans");
    return val ? JSON.parse(val) as Plan[] : [];
  }

  saveShown(plan: Plan) {
    this.storeShownPlans(this.getShownPlans().concat(plan));
  }

  deleteShown(plan: Plan) {
    if (!plan) return;
    let shown = this.getShownPlans();
    let ind = shown.findIndex(p => p.ID === plan.ID);
    if (ind == -1) return;
    shown.splice(ind, 1);
    this.storeShownPlans(shown);
  }


  storePreferences(prefs: any) {
    this.setCookie("preferences", JSON.stringify(prefs), 365);
  }

  getPreferences() {
    let val = this.getCookie("preferences");
    return val ? JSON.parse(val) : {};
  }

  setPref(name: string, value: boolean) {
    let pr = this.getPreferences();
    pr[name] = value;
    this.storePreferences(pr);
  }

  getPref(name: string) {
    return this.getPreferences()[name];
  }
}
