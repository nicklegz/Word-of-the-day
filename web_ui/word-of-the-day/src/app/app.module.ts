import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { WordComponent } from './components/word/word.component';
import { BottomNavComponent } from './components/bottom-nav/bottom-nav.component';
import { HttpClientModule } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
// import { AuthInterceptor } from './services/auth.interceptor';
import { ErrorComponent } from './components/error/error.component';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from 'src/environments/environment';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { LoginComponent } from './components/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { SignupComponent } from './components/signup/signup.component';
import { AccountComponent } from './components/account/account.component';
import { WordsComponent } from './components/words/words.component';
import { AuthComponent } from './components/auth/auth.component';

@NgModule({
  declarations: [
    AppComponent,
    TopNavComponent,
    WordComponent,
    BottomNavComponent,
    ErrorComponent,
    SpinnerComponent,
    LoginComponent,
    SignupComponent,
    AccountComponent,
    WordsComponent,
    AuthComponent
  ],
  exports:[
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: environment.domain,
      clientId: environment.clientId
    }),
  ],
  providers: [
    // {
    // provide: HTTP_INTERCEPTORS,
    // useClass: AuthInterceptor,
    // multi: true,
    // }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
