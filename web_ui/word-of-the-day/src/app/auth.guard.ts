import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { AuthService } from './services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  isAuthenticated!: boolean;

  constructor(private auth: AuthService, private router: Router){
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      this.auth.isAuthenticated$.subscribe(data =>{
        this.isAuthenticated = data;
      })

      if(this.isAuthenticated == false){
        this.router.navigate(['login']);
      }

      return this.isAuthenticated;
  }
}
