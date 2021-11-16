import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { Word } from '../interfaces/word.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { share} from 'rxjs/operators';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})

export class WordService {
  word$!: Observable<Word>;

  constructor(private http: HttpClient) {}

  public getWordOfTheDay(): Observable<Word> {
    this.word$ = this.http.get<Word>(baseApiUrl + '/word/word-of-the-day').pipe(share());
    return this.word$;
  }
}

