import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { AppRoutingModule }     from './app-routing.module';
import { DashboardComponent } from './component/dashboard/dashboard.component';
import { SpielerComponent } from './component/spieler/spieler.component';
import { MatchComponent } from './component/match/match.component';
import { LiveComponent } from './component/live/live.component';
import { TournierComponent } from './component/tournier/tournier.component';
import {SpielerService} from "./service/spieler.service";
import {MatchService} from "./service/match.service";
import {TournierService} from "./service/tournier.service";
import {StatistikService} from "./service/statistik.service";


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    SpielerComponent,
    MatchComponent,
    LiveComponent,
    TournierComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [
      SpielerService,
      MatchService,
      TournierService,
      StatistikService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
