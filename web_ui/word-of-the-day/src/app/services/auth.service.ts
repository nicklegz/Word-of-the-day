import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserInfo } from '../interfaces/userAuth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = environment.apiURL + "/auth";
  public isAuthenticated = false;

  constructor(private http: HttpClient) { }

  getUserInfo(username: string){
    return this.http.get<UserInfo>(this.authUrl + "/user/" + username);
  }

  createUser(username: string){
      return this.http.post(environment.apiURL + "/auth/user/" + username, "")
    }

  setIsAuthenticated(flag: boolean) {
      this.isAuthenticated = flag;
    }

  signOut(){
    this.setIsAuthenticated(false);
  }
  }

