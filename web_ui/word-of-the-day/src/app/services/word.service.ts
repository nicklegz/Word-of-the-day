import { Injectable } from '@angular/core';
import { Word } from '../interfaces/word.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';
import { BehaviorSubject } from 'rxjs';

const baseWordApiUrl = `${environment.apiURL}/word`;

@Injectable({
  providedIn: 'root'
})

export class WordService {
  private _listOfWords = new BehaviorSubject<Word[]>([]);
  public listOfWords$ = this._listOfWords.asObservable();

  private _listWordsViewName = new  BehaviorSubject<string>("");
  public listWordsViewName$ = this._listWordsViewName.asObservable();

  private _isLikedWordOfTheDay = new BehaviorSubject<boolean>(false);
  public isLikedWordOfTheDay$ = this._isLikedWordOfTheDay.asObservable();
  public isLikedWordOfTheDay = this._isLikedWordOfTheDay;

  constructor(
    private http: HttpClient, 
    private auth: AuthService) {}

  public getWordOfTheDay(){
    return this.http.get<Word>(`${baseWordApiUrl}/word-of-the-day/${this.auth.username}`);
  }

   public getPreviouslyUsedWords() {
    return this.http.get<Word[]>(`${baseWordApiUrl}/previously-used-words/${this.auth.username}`);
  }

  public getLikedWords(){
    return this.http.get<Word[]>(`${baseWordApiUrl}/liked-words/${this.auth.username}`);
  }

  public getIsLikedWordOfTheDay(wordId: number){
    return this.http.get<boolean>(`${baseWordApiUrl}/liked-words/${this.auth.username}/${wordId}`);
  }

  public addLikedWord(wordId: number){
    return this.http.post(`${baseWordApiUrl}/liked-words/${this.auth.username}`, wordId);
  }

  public deleteLikedWord(wordId: number){
    return this.http.delete(`${baseWordApiUrl}/liked-words/${this.auth.username}/${wordId}`);
  }

  //manipulate the BehaviourSubjects
  public setListOfWords(words: Word[]){
    this._listOfWords.next(words);
  }

  public setListWordsViewName(viewname: string){
    this._listWordsViewName.next(viewname);
  }

  public setIsLikedWordOfTheDay(isLiked: boolean){
    this._isLikedWordOfTheDay.next(isLiked);
  }
}
