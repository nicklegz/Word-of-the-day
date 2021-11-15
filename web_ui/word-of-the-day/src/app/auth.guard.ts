import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from './services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  isAuthenticated!: Observable<boolean>
  constructor(private auth: AuthService){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> {

    this.auth.getUserInfo().subscribe(authInfo => {
      if(authInfo.isAuthenticated == false){
        this.auth.login();
      }

      this.isAuthenticated = of(authInfo.isAuthenticated)

    })

    return(this.isAuthenticated)
  }
}
