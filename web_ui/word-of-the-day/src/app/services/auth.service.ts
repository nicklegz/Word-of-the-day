import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserInfo } from '../interfaces/userAuth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = environment.apiURL + "/auth";

  constructor(private http: HttpClient,  private router: Router) { }

  login(){
    window.location.href = this.authUrl + "/login";
  }

  getUserInfo(){
    return this.http.get<UserInfo>(this.authUrl + "/user");
  }

  
}
