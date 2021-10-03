import { Guid } from "guid-typescript";
import { Word } from "./Word";

export interface UserWords{
  Id: Guid;
  Username: string;
  PreviouslyUsedWords: Array<Word>;
  WordOfTheDay: Word;
  LastUpdated: Date;
}
