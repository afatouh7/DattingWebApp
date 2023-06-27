import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Messages } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { MembersService } from '../_services/members.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-meber-messages',
  templateUrl: './meber-messages.component.html',
  styleUrls: ['./meber-messages.component.css']
})
export class MeberMessagesComponent implements OnInit{
@ViewChild('messageForm') messageForm:NgForm
 @Input() messages: Messages[];
@Input() username:string;
messageContent:string;

   constructor(public messageService: MessageService){}

  ngOnInit(): void {

  }

  sendmessage(){
    this.messageService.sendMessage(this.username,this.messageContent).then(() =>{

      this.messageForm.reset();
    })

  }


}
