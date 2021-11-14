import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { ErrorComponent } from './components/error/error.component';
import { WordComponent } from './components/word/word.component';

const routes: Routes = [
  {path: 'home', component: WordComponent, canActivate:[AuthGuard]},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'error', component: ErrorComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
