import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of, throwError } from 'rxjs';
import { UserWords } from '../interfaces/userwords.interface';
import { Word } from '../interfaces/word.interface';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WordService {
  word!: Word;
  nextDate!: number;
  user!: UserWords;
  timeInterval: number = 86400000;
  url = "http://localhost:5000/api/word/nicktest";

  constructor(public auth: AuthService, private http: HttpClient) {
  }

  public getWord(): Observable<Word> {

    //localStorage.clear();
    var localStoreWord = localStorage.getItem('user');

    if(localStoreWord != null){
      this.user! = JSON.parse(localStoreWord)!;
      this.nextDate = this.user!.LastUpdated + this.timeInterval;
      this.word! = this.user!.WordOfTheDay;
    }

    if(localStoreWord == null || Date.now() > this.nextDate){
      return this.getNewWord();
    }

    return of(this.word!);
  }

  public getNewWord() : Observable<Word>{

    let reqHeaders = new HttpHeaders().set('Accept', 'application/json');
    return this.http.get<Word>(this.url, {headers: reqHeaders});

    // localStorage.setItem('user', JSON.stringify(currentUser!));
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
  }

