import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent }   from './component/dashboard/dashboard.component';
import { SpielerComponent } from "./component/spieler/spieler.component";
import { MatchComponent } from "./component/match/match.component";
import { LiveComponent } from "./component/live/live.component";
import { TournierComponent } from "./component/tournier/tournier.component";

const routes: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'dashboard',  component: DashboardComponent },
    { path: 'spieler',  component: SpielerComponent },
    { path: 'matches',  component: MatchComponent },
    { path: 'live',  component: LiveComponent },
    { path: 'tourniere',  component: TournierComponent },
];
@NgModule({
    imports: [ RouterModule.forRoot(routes) ],
    exports: [ RouterModule ]
})
export class AppRoutingModule {}