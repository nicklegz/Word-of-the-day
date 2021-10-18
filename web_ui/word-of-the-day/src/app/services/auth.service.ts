import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as auth0 from 'auth0-js';
import { environment } from 'src/environments/environment';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class CustomAuthService {

  userProfile: any;
  requestedScopes: string = 'openid profile';

  auth0 = new auth0.WebAuth({
    clientID: environment.authClientId,
    domain: environment.authDomain,
    responseType: 'token id_token',
    // audience: environment.apiURL,
    redirectUri: environment.redirectUri,
    scope: this.requestedScopes
  });

  constructor(public router: Router) {  }

  public login() {
    this.auth0.authorize();
  }

  public handleAuthentication(): void {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.idToken) {
        window.location.hash = '';
        this.setSession(authResult);
        this.router.navigate(['callback']);
      } else if (err) {
        this.router.navigate(['login']);
        console.log(err);
        alert('Error: <%= "${err.error}" %>. Check the console for further details.');
      }
    });
  }

  private setSession(authResult: auth0.Auth0DecodedHash): void {
    // Set the time that the Access Token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn! * 1000) + new Date().getTime());

    // If there is a value on the scope param from the authResult,
    // use it to set scopes in the session for the user. Otherwise
    // use the scopes as requested. If no scopes were requested,
    // set it to nothing
    const scopes = authResult.scope || this.requestedScopes || '';

    localStorage.setItem('access_token', authResult.accessToken!);
    localStorage.setItem('id_token', authResult.idToken!);
    localStorage.setItem('expires_at', expiresAt);
    localStorage.setItem('scopes', JSON.stringify(scopes));

  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    localStorage.removeItem('scopes');
    // Go back to the home route
    this.router.navigate(['/home']);
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // Access Token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem('expires_at')!);
    return new Date().getTime() < expiresAt;
  }

  public userHasScopes(scopes: Array<string>): boolean {
    const grantedScopes = JSON.parse(localStorage.getItem('scopes')!).split(' ');
    return scopes.every(scope => grantedScopes.includes(scope));
  }

  public getProfile(): any {
    const idToken = localStorage.getItem('id_token');
    if (!idToken) {
      throw new Error('Id Token must exist to fetch profile');
    }

    return this.decodeJwtToken(idToken);

  }

  private decodeJwtToken(token: string): any {
    try{
        return jwt_decode(token);
    }
    catch(Error){
        return null;
    }
  }
}
