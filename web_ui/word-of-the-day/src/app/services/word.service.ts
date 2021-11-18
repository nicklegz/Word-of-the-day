import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { Word } from '../interfaces/word.interface';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { concatMap, share} from 'rxjs/operators';
import { AuthService } from '@auth0/auth0-angular';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})

export class WordService {
  word$!: Observable<Word>;

  constructor(private http: HttpClient, private auth: AuthService) {}

  public getWordOfTheDay(): Observable<Word> {
    return this.auth.user$
    .pipe(
      concatMap(user =>
        this.http.get<Word>(baseApiUrl + '/word/word-of-the-day/' + user?.nickname))
        )
  }
}
