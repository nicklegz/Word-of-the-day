import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, of } from 'rxjs';
import { UserWords } from '../interfaces/UserWords';
import { Word } from '../interfaces/Word';
import { MockUserWords, Words } from '../mockdata/MockData';

@Injectable({
  providedIn: 'root'
})
export class WordService {
  userWords!: Word[];
  word!: Word;
  nextDate!: any;
  user!: UserWords;

  constructor(public auth: AuthService) { }

  public getWord(): Observable<Word> {

    var localStoreWord = localStorage.getItem('user');

    if(localStoreWord != null){
      this.user! = JSON.parse(localStoreWord)!;

      //look into this bug
      //this.nextDate = this.user!.LastUpdated.setHours(this.user!.LastUpdated.getHours() + 24);
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
    this.nextDate = currentUser!.LastUpdated.setHours(currentUser!.LastUpdated.getHours() + 24);

    if(Date.now() > this.nextDate){
      this.userWords = Words.filter(word => !currentUser!.PreviouslyUsedWords.includes(word));
      this.word = this.userWords[Math.floor(Math.random() * this.userWords.length)];

      currentUser!.PreviouslyUsedWords.push(this.word);
      currentUser!.WordOfTheDay = this.word;

      const dateNow = Date.now()
      currentUser!.LastUpdated = new Date(dateNow);
    }

    else{
      this.word! = currentUser!.WordOfTheDay;
    }

    localStorage.setItem('user', JSON.stringify(currentUser!));
  }
}
