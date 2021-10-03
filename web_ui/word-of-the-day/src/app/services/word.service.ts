import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of } from 'rxjs';
import { UserWords } from '../interfaces/userwords.interface';
import { Word } from '../interfaces/word.interface';
import { MockUserWords, Words } from '../mockdata/MockData';

@Injectable({
  providedIn: 'root'
})
export class WordService {
  word!: Word;
  nextDate!: number;
  user!: UserWords;
  timeInterval: number = 86400000;
  availableWords!: Word[];

  constructor(public auth: AuthService) { }

  public getWord(): Observable<Word> {

    //localStorage.clear();
    var localStoreWord = localStorage.getItem('user');

    if(localStoreWord != null){
      this.user! = JSON.parse(localStoreWord)!;
      this.nextDate = this.user!.LastUpdated + this.timeInterval;
      this.word! = this.user!.WordOfTheDay;
    }

    if(localStoreWord == null || Date.now() > this.nextDate){
      this.getNewWord();
    }

    return of(this.word!);
  }

  private getNewWord() {
    const curUsername = "nicktest"
    const currentUser = MockUserWords.find(x => x.Username === curUsername);
    this.nextDate = currentUser!.LastUpdated + this.timeInterval;

    if(Date.now() > this.nextDate){

      this.getAvailableWords(currentUser!);
      this.word = this.availableWords[Math.floor(Math.random() * this.availableWords.length)];
      currentUser!.PreviouslyUsedWords.push(this.word);
      currentUser!.WordOfTheDay = this.word;
      currentUser!.LastUpdated = Date.now();
    }

    else{
      this.word! = currentUser!.WordOfTheDay;
    }

    localStorage.setItem('user', JSON.stringify(currentUser!));
  }

  private getAvailableWords(currentUser: UserWords){
    return this.availableWords = Words.filter(words =>{
      return currentUser?.PreviouslyUsedWords.some(userWords => {
        return words.Id != userWords.Id && words.WordText != userWords.WordText && words.Definition != userWords.Definition
      })
    });
  }
}
