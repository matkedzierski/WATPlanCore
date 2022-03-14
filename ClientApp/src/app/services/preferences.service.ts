import {Injectable} from '@angular/core';
import {Subject} from "rxjs";
import {Plan} from "../models/plan";

@Injectable({
  providedIn: 'root'
})
export class PreferencesService {

  public prefsChanged: Subject<{ name: string, value: boolean }> = new Subject<any>();
  public savePlan: Subject<Plan> = new Subject<Plan>();

  constructor() {
  }
}
