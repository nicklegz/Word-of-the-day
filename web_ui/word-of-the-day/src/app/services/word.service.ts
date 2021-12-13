import { Injectable } from '@angular/core';
import { Word } from '../interfaces/word.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

const baseApiUrl = environment.apiURL;

@Injectable({
  providedIn: 'root'
})

export class WordService {
  private username!: string;

  constructor(
    private http: HttpClient, 
    private auth: AuthService) {
      this.auth.username.subscribe(username => {
        this.username = username;
      })
    }

  public getWordOfTheDay(){
    return this.http.get<Word>(baseApiUrl + '/word/word-of-the-day/' + this.username)
  }

   public getPreviouslyUsedWords() {
    return this.http.get<Array<Word>>(baseApiUrl + '/word/previously-used-words/' + this.username)
  }

  public getSavedWords(){
    
  }
}
