import { Guid } from "guid-typescript";
import { Word } from "./word.interface";

export interface UserWords{
  Id: Guid;
  Username: string;
  PreviouslyUsedWords: Array<Word>;
  WordOfTheDay: Word;
  LastUpdated: number;
}
