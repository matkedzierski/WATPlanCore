import { Component, OnInit } from '@angular/core';
import { RankEntry } from 'src/app/models/rank-entry';
import { StatsService } from 'src/app/services/stats.service';

@Component({
  selector: 'app-top-plans',
  templateUrl: './top-plans.component.html',
  styleUrls: ['./top-plans.component.css']
})
export class TopPlansComponent implements OnInit {
  topPlans: RankEntry[] = [];
  displayedColumns = ['position', 'name', 'count'];
  rankingCount = 10;
  dateRange = 31;
  isLoading = false;
  unique: boolean = false;
  showSummary: boolean = false;

  constructor(private statsService: StatsService) {
  }

  ngOnInit(): void {
    this.reloadRanking();
  }

  reloadRanking() {
    this.isLoading = true;
    this.statsService.loadTop(this.rankingCount, this.dateRange, this.unique).subscribe(entries => {
      this.topPlans = entries.sort((e1, e2) => e1.position > e2.position ? 1 : -1);
      this.isLoading = false;
    });
  }

  getTotal() {
    return this.topPlans.map(p => p.count).reduce((sum, current) => sum + current);
  }
}
