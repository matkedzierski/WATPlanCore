import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Plan} from "../../../models/plan";

@Component({
  selector: 'app-plan-list-option',
  templateUrl: './plan-list-option.component.html',
  styleUrls: ['./plan-list-option.component.css']
})
export class PlanListOptionComponent implements OnInit {

  @Input()
  plan: Plan;

  @Output()
  delete: EventEmitter<void> = new EventEmitter<void>();

  constructor() {
  }

  ngOnInit(): void {
  }

  deletePlan($event: MouseEvent) {
    this.delete.emit();
    $event.stopPropagation();
  }
}
