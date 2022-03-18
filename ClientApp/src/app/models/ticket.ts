import {TicketCategory} from "./enum/ticket-category.enum";

export class Ticket {
  planName?: string;
  content!: string;
  category!: TicketCategory;
  email!: string
  sender!: string
}
