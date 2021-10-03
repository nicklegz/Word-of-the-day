import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopNavComponent } from './top-nav/top-nav.component';
import { WordComponent } from './word/word.component';
import { AuthModule } from '@auth0/auth0-angular';

@NgModule({
  declarations: [
    AppComponent,
    TopNavComponent,
    WordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule.forRoot({
      domain: 'nicklegz.us.auth0.com',
      clientId: 'SZjwwZGVwjWDK5giyHeeFMTVCDRIqrFI'}),
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }
