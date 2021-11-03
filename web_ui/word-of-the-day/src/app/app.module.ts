import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { WordComponent } from './components/word/word.component';
import { AuthModule } from '@auth0/auth0-angular';
import { BottomNavComponent } from './bottom-nav/bottom-nav.component';
import { HttpClientModule } from '@angular/common/http';
import { CallbackComponent } from './components/callback/callback.component';
import { LoginComponent } from './components/login/login.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    TopNavComponent,
    WordComponent,
    BottomNavComponent,
    CallbackComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: environment.domain,
      clientId: environment.clientId}),
  ],
  providers: [
    {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
    }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
