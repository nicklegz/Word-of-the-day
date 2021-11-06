import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WordComponent } from './components/word/word.component';

const routes: Routes = [
  {path: 'home', component: WordComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
