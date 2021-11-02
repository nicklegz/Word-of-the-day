import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of, throwError } from 'rxjs';
import { Word } from '../interfaces/word.interface';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserWord } from '../interfaces/userword.interface';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})
export class WordService {
  word!: Word;
  observeWord!: Observable<Word>;
  nextDate!: number;
  userWord!: UserWord;
  timeInterval: number = 86400000;
  email: string | undefined;
  authenticated: boolean = false;

  constructor(public auth: AuthService, private http: HttpClient) {
  }

  public getWordOfTheDay(): Observable<Word> {
    //localStorage.clear();
    var localStoreWord = localStorage.getItem('user_word');

    if(localStoreWord != null){
      this.userWord = JSON.parse(localStoreWord)!;
      this.nextDate = this.userWord!.LastUpdated + this.timeInterval;
      this.word = this.userWord!.WordOfTheDay;
    }

    if(localStoreWord == null || Date.now() > this.nextDate){
      this.observeWord = this.http.get<Word>(baseApiUrl + '/word/word-of-the-day');
      this.observeWord.subscribe(word => {
        this.word.WordId = word.WordId;
        this.word.Text = word.Text;
        this.word.Type = word.Type;
        this.word.Definition = word.Definition;
      });

      this.userWord = {
        WordOfTheDay: this.word,
        LastUpdated: Date.now(),
      }
    
      localStorage.setItem('user_word', JSON.stringify(this.userWord));
    
      return this.observeWord;
    }

    return of(this.word);
  }

  

  // private errorHandler(error: HttpErrorResponse){
  //   if(error.error instanceof ErrorEvent){
  //     console.error('An error occured', error.error.message);

  //   }

  //   else{
  //     console.error(
  //       error.status + ' ' + error.error
  //     );
  //   }

  //   return throwError(
  //     "An error occured."
  //   );
  //   }
  }

