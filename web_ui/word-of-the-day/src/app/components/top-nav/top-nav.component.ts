import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.css']
})
export class TopNavComponent implements OnInit{

  isAuthenticated$!: Observable<boolean>;

  constructor(
    private auth: AuthService,
     private router: Router) { 
  }

  ngOnInit(){
    this.isAuthenticated$ = this.auth.isAuthenticated$;
  }

  signOut(){
    localStorage.removeItem("username");
    this.auth.signOut();
  }

  goToAccount(){
    this.router.navigate(['account'])
  }
}
