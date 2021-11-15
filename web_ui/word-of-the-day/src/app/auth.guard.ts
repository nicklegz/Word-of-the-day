import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of, pipe } from 'rxjs';
import { map } from 'rxjs/operators';
import { isTemplateSpan } from 'typescript';
import { AuthService } from './services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  isAuthenticated!: Observable<boolean>
  constructor(private auth: AuthService){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {

    this.auth.getUserInfo()
    .pipe(map(authInfo =>{
      this.isAuthenticated = of(authInfo.isAuthenticated)
      if(authInfo.isAuthenticated == false){
        this.auth.login();
      }
    }))

    return this.isAuthenticated
  }
}
