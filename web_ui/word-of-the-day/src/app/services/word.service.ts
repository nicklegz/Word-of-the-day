import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of, throwError } from 'rxjs';
import { Word } from '../interfaces/word.interface';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserWord } from '../interfaces/userword.interface';
import { share} from 'rxjs/operators';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})

export class WordService {
  word!: Word;
  word$!: Observable<Word>;
  nextDate!: number;
  userWord!: UserWord;
  timeInterval: number = 86400000;

  constructor(public auth: AuthService, private http: HttpClient) {}

  public getWordOfTheDay(): Observable<Word> {
    //localStorage.clear();
    var localStoreWord = localStorage.getItem('user_word');

    if(localStoreWord != null){
      this.userWord = JSON.parse(localStoreWord)!;
      this.nextDate = this.userWord!.LastUpdated + this.timeInterval;
      this.word = this.userWord!.WordOfTheDay;
    }

    if(localStoreWord == null || Date.now() > this.nextDate){
      this.word$ = this.http.get<Word>(baseApiUrl + '/word/word-of-the-day').pipe(share());
      this.word$.subscribe(word => {
        this.word.WordId = word.WordId;
        this.word.Text = word.Text;
        this.word.Type = word.Type;
        this.word.Definition = word.Definition;

        // this.setUserWord(word);

      },this.errorHandler);

      return this.word$;
    }

    return of(this.word);
  }

  private errorHandler(error: HttpErrorResponse){
    if(error.error instanceof ErrorEvent){
      console.error('An error occured', error.error.message);

    }

    else{
      console.error(
        error.status + ' ' + error.error
      );
    }

    return throwError(
      "An error occured."
    );
    }
    
    setUserWord(word: Word){
      const userWord = {
        WordOfTheDay: word,
        LastUpdated: Date.now()
      }

      localStorage.setItem("user_word", JSON.stringify(userWord));
    }
}

