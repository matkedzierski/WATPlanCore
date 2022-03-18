import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import 'angular-calendar/scss/angular-calendar.scss';


import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';
import {LayoutModule} from '@angular/cdk/layout';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatIconModule} from '@angular/material/icon';
import {MatListModule} from '@angular/material/list';
import {MainComponent} from './components/main/main.component';
import {RouterModule} from "@angular/router";
import {DashboardComponent} from './components/dashboard/dashboard.component';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatCardModule} from '@angular/material/card';
import {MatMenuModule} from '@angular/material/menu';
import {TabsComponent} from './components/tabs/tabs.component';
import {MatTabsModule} from "@angular/material/tabs";
import {PlanComponent} from './components/plan/plan.component';
import {ManageComponent} from './components/manage/manage.component';
import localePl from '@angular/common/locales/pl';
import {CalendarModule, DateAdapter} from 'angular-calendar';
import {adapterFactory} from 'angular-calendar/date-adapters/date-fns';
import {CalendarHeaderComponent} from './components/plan/calendar-header/calendar-header.component';
import {MatButtonToggleModule} from "@angular/material/button-toggle";
import {registerLocaleData} from "@angular/common";
import {HttpClientModule} from "@angular/common/http";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {CookieService} from "./services/cookie.service";
import {PlanListOptionComponent} from './components/main/plan-list-option/plan-list-option.component';
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {PreferencesComponent} from './components/preferences/preferences.component';
import {SearchComponent} from './components/search/search.component';
import {PlanMobileComponent} from './components/plan-mobile/plan-mobile.component';
import {CalendarHeaderMobileComponent} from "./components/plan-mobile/calendar-header/calendar-header.component";
import {MatTooltipModule} from "@angular/material/tooltip";
import {StatsComponent} from "./components/dialogs/stats/stats.component";
import {MatDialogModule} from "@angular/material/dialog";
import { TopPlansComponent } from './components/dialogs/stats/top-plans/top-plans.component';
import {MatTableModule} from "@angular/material/table";
import {MatSelectModule} from "@angular/material/select";
import {MatCheckboxModule} from "@angular/material/checkbox";
import { ContactComponent } from './components/dialogs/contact/contact.component';


registerLocaleData(localePl);

@NgModule({
  declarations: [
    MainComponent,
    DashboardComponent,
    TabsComponent,
    PlanComponent,
    ManageComponent,
    CalendarHeaderComponent,
    PlanListOptionComponent,
    PreferencesComponent,
    SearchComponent,
    CalendarHeaderMobileComponent,
    PlanMobileComponent,
    StatsComponent,
    TopPlansComponent,
    ContactComponent
  ],
  exports: [MainComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    RouterModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatTabsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    MatButtonToggleModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatDialogModule,
    MatTableModule,
    MatSelectModule,
    FormsModule,
    MatCheckboxModule
  ],
  providers: [CookieService],
  bootstrap: [MainComponent]
})
export class AppModule {
}
