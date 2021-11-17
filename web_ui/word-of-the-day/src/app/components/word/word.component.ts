import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { concatMap } from 'rxjs/operators';
import { UserInfo } from 'src/app/interfaces/userAuth.interface';
import { environment } from 'src/environments/environment';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {

  word$!: Observable<Word>;

  constructor(private wordService: WordService, private auth: AuthService, private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    this.getUserInfo().subscribe(data =>{
      if(data.createUser == true){
        this.createUser();
      }
    })

    this.word$ = this.wordService.getWordOfTheDay();
  }

  createUser(){
    this.auth.user$
    .pipe(
      concatMap(user =>
        this.http.post(environment.apiURL + "/auth/user/" + user?.nickname, "")
        )
    ).subscribe();
  }

  getUserInfo(){
    return this.http.get<UserInfo>(environment.apiURL + "/auth/user");
  }
}
