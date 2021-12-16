import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserInfo } from '../interfaces/userAuth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = environment.apiURL + "/auth";
  private _isAuthenticated = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this._isAuthenticated.asObservable();
  public username: string = "";

  constructor(private http: HttpClient, private router:Router) { }

  setUsername(username: string){
    this.username = username;
  }

  removeUsername(){
    localStorage.removeItem("username");
    this.username = "";
  }

  getUserInfo(username: string){
    return this.http.get<UserInfo>(this.authUrl + "/user/" + username);
  }

  createUser(username: string){
      return this.http.post(environment.apiURL + "/auth/user/" + username, "")
    }

  setIsAuthenticated(flag: boolean) {
      this._isAuthenticated.next(flag);
    }

  signOut(){
    this._isAuthenticated.next(false);
    this.router.navigate(['login'])
  }

}

