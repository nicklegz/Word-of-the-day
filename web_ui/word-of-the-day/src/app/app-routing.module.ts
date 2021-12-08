import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { LoginComponent } from './components/login/login.component';
import { WordComponent } from './components/word/word.component';

const routes: Routes = [
  // {path: 'home', component: WordComponent, canActivate:[AuthGuard]},
  {path: 'home', component: WordComponent},
  {path: 'login', component: LoginComponent},
  // {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: '**', redirectTo: '/home'}
]; 

@NgModule({
  imports: [RouterModule.forRoot(routes , { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
