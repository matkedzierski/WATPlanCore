import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Plan} from "../../models/plan";
import {FormControl} from "@angular/forms";
import {PlanService} from "../../services/plan.service";
import {PreferencesService} from "../../services/preferences.service";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  allplans: Plan[] = [];
  filteredplans: Plan[] = [];
  shownplans: Plan[] = [];
  lastSearch!: string | null;
  isLoading: boolean = false;
  search: FormControl = new FormControl('');

  constructor(private planService: PlanService,
              private preferences: PreferencesService,
              private cdRef: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.planService.getPlans().subscribe((plans) => {
      this.allplans = plans as Plan[];
      this.isLoading = false;
    });

  }

  savePlan(plan: Plan) {
    this.preferences.savePlan.next(plan);
    this.search.reset();
    this.filteredplans = [];
    this.shownplans = [];
    this.cdRef.detectChanges();
  }

  searchPlans(e: any) {
    this.filterPlans(e.target.value);
  }

  filterPlans(txt: string) {
    txt = txt.toLowerCase();
    if (txt.length < 4) {
      this.filteredplans = [];
      this.shownplans = [];
      this.lastSearch = null;
    } else {
      this.isLoading = true;
      if (txt.startsWith(this.lastSearch!)) {
        this.filteredplans = this.filteredplans.filter((p) => p.name.toLowerCase().includes(txt));
      } else {
        this.filteredplans = this.allplans.filter((p) => p.name.toLowerCase().includes(txt));
      }
      this.shownplans = this.filteredplans.slice(0, 10);
      this.lastSearch = txt;
      this.isLoading = false;
    }
  }


  checkOverflow(element: HTMLElement) {
    return element.offsetHeight < element.scrollHeight ||
      element.offsetWidth < element.scrollWidth;
  }

  getTransitionTimes(plan: Plan) {
    let secs = plan.name.length / 10;
    return {
      '-webkit-transition': secs + 's',
      '-moz-transition': secs + 's',
      'transition': secs + 's'
    }
  }
}
