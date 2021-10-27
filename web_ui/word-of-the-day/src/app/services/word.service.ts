import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of, throwError } from 'rxjs';
import { UserWords } from '../interfaces/userwords.interface';
import { Word } from '../interfaces/word.interface';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { switchMap , shareReplay} from 'rxjs/operators';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})
export class WordService {
  word!: Word;
  nextDate!: number;
  // user!: UserWords;
  user: any;
  timeInterval: number = 86400000;
  email: string | undefined;
  authenticated: boolean = false;

  constructor(public auth: AuthService, private http: HttpClient) {
  }

  public getWord(): Observable<Word> {
    //localStorage.clear();
    var localStoreWord = localStorage.getItem('user');

    // if(localStoreWord != null){
    //   this.user! = JSON.parse(localStoreWord)!;
    //   this.nextDate = this.user!.LastUpdated + this.timeInterval;
    //   this.word! = this.user!.WordOfTheDay;
    // }

    // if(localStoreWord == null || Date.now() > this.nextDate){
      return this.getNewWord();
    // }

    // return of(this.word!);
  }





  public getNewWord() : Observable<Word>{

    return this.http.get<Word>(baseApiUrl + '');

    // localStorage.setItem('user', JSON.stringify(currentUser!));
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

