export interface Event {
  id: string;
  name: string;
  shortcut: string;
  type: string;
  number: number;
  color: string;

  startTime: Date;
  endTime: Date;

  lecturer: string;
  room: string;
  groups: string;
  info: string;

  week: number;
  dayOfWeek: number;
  blockNumber: number;
  blockSpan: number;
}
