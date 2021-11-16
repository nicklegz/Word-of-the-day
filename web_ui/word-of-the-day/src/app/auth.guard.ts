import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of, pipe } from 'rxjs';
// import { AuthService } from './services/auth.service';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  isAuthenticated!: Observable<boolean>
  constructor(private auth: AuthService){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    this.auth.isAuthenticated$.subscribe(authInfo =>{
      if(authInfo == false){
        this.auth.loginWithRedirect()
        return false;
      }
      return true;
    })
    // this.auth.getUserInfo()
    // .subscribe(authInfo =>{
    //   if(authInfo.isAuthenticated == false){
    //     this.auth.login();
    //     return false;
    //   }

    //   return true;
    // })

    return of(true);
  }
}
