import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AccountComponent } from './components/account/account.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { WordComponent } from './components/word/word.component';
import { WordsComponent } from './components/words/words.component';

const routes: Routes = [
  {path: 'home', component: WordComponent, canActivate:[AuthGuard]},
  {path: 'account', component: AccountComponent, canActivate:[AuthGuard]},
  {path: 'words/:listname', component: WordsComponent, canActivate:[AuthGuard]},
  {path: 'login', component: LoginComponent},
  {path: 'sign-up', component: SignupComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: '**', redirectTo: '/home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes , { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
