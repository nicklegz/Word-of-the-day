import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { from, Observable, of } from 'rxjs';
import { concatMap, finalize, map, share } from 'rxjs/operators';
import { UserInfo } from 'src/app/interfaces/userAuth.interface';
import { LoadingService } from 'src/app/services/loading.service';
import { environment } from 'src/environments/environment';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {
  word!: any;
  response: any;
  word$!: Observable<Word>;
  loading$!: Observable<boolean>;
  res: any;

  constructor(
    private wordService: WordService, 
    private auth: AuthService,
    private http: HttpClient,
    private loader: LoadingService) 
    {}

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;

    this.getUserInfo().subscribe(data =>{
      if(data.createUser == true){
        this.createUser();
      }
    })

    this.word$ = this.wordService.getWordOfTheDay();
  }

  createUser(){
    this.auth.user$.pipe(
      concatMap(user =>
        this.http.post(environment.apiURL + "/auth/user/" + user?.nickname, "")
        )
    ).subscribe()
  }

  getUserInfo(){
    return this.auth.user$
    .pipe(
      concatMap(user =>
        this.http.get<UserInfo>(environment.apiURL + "/auth/user/" + user?.nickname)
        )
    )
  }
}
