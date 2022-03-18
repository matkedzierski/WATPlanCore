import {Component, NgZone, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, NgForm, Validators} from "@angular/forms";
import {MatDialogRef} from "@angular/material/dialog";
import {CdkTextareaAutosize} from "@angular/cdk/text-field";
import {TicketCategory} from 'src/app/models/enum/ticket-category.enum';
import {MatSnackBar} from "@angular/material/snack-bar";
import { Ticket } from 'src/app/models/ticket';
import {ContactService} from "../../../services/contact.service";

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  @ViewChild('autosize') autosize!: CdkTextareaAutosize;
  model: Ticket = {planName: "", content: "", category: TicketCategory.MISSING_PLAN, email: "", sender: ""};
  categories = CATEGORIES;
  planRelatedCategories = [TicketCategory.MISSING_PLAN, TicketCategory.INCORRECT_DATA];

  constructor(private ref: MatDialogRef<ContactComponent>,
              private _ngZone: NgZone,
              private snack: MatSnackBar,
              private contactService: ContactService,
              private _fb: FormBuilder) { }

  ngOnInit(): void {
  }

  submit(form: NgForm) {
    if(form.valid){
      this.contactService.sendTicket(this.model).subscribe(
        ()=>{},
          err =>{
            this.snack.open("Błąd podczas przesyłania zgłoszenia", "x", {duration: 1000});
      })
      this.ref.close();
    } else {
      console.log(form.controls);
    }
  }
}


export const CATEGORIES = [
  {
    category: TicketCategory.MISSING_PLAN,
    label: "Brakujący plan"
  },{
    category: TicketCategory.INCORRECT_DATA,
    label: "Niepoprawne lub nieaktualne dane w planie"
  },{
    category: TicketCategory.SITE_ERROR,
    label: "Błąd w działaniu strony"
  },{
    category: TicketCategory.TECH_SUPPORT,
    label: "Wsparcie techniczne"
  },{
    category: TicketCategory.SUGGESTION,
    label: "Sugestia"
  },{
    category: TicketCategory.OPINION,
    label: "Opinia"
  },{
    category: TicketCategory.COOPERATION,
    label: "Współpraca"
  }, {
    category: TicketCategory.OTHER,
    label: "Inne"
  }
];
