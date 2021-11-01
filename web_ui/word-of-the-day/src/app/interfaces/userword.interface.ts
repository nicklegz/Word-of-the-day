import { Guid } from "guid-typescript";
import { Word } from "./word.interface";

export interface UserWord{
  Id: Guid;
  Username: string;
  WordOfTheDay: Word;
  LastUpdated: number;
}
