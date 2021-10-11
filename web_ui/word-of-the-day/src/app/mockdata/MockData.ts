import { Guid } from "guid-typescript";
import { UserWords } from "../interfaces/userwords.interface";
import { Word } from "../interfaces/word.interface";

export const MockUserWords: UserWords[] = [
  {Id: Guid.parse("ad4293bb-a4bb-4337-a8f6-80123092d777"), WordOfTheDay: {WordId: 4, WordText: "Hungry", Definition:"Having a desire, craving, or need for food; feeling hunger."},
  PreviouslyUsedWords: [{WordId: 1, WordText: "Happy", Definition:"Characterized by or indicative of pleasure, contentment, or joy."}], Username: "nicktest",
  LastUpdated: Date.parse("2020-07-19T20:23:01.804Z")}
]

export const Words : Word[] =[
  {WordId: 1, WordText: "Happy", Definition:"Characterized by or indicative of pleasure, contentment, or joy."},
  {WordId: 2, WordText: "Sad", Definition:"Expressive of or characterized by sorrow."},
  {WordId: 3, WordText: "Funny", Definition:"Providing fun; causing amusement or laughter; amusing; comical."},
  {WordId: 4, WordText: "Hungry", Definition:"Having a desire, craving, or need for food; feeling hunger."}
]
