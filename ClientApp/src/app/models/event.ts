export interface Event {
  ID: string;
  Name: string;
  Shortcut: string;
  Type: string;
  Number: number;
  Color: string;

  StartTime: Date;
  EndTime: Date;

  Lecturer: string;
  Room: string;
  Groups: string;
  Info: string;

  Week: number;
  DayOfWeek: number;
  BlockNumber: number;
  BlockSpan: number;
}
