import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { UserRole } from 'src/app/_models/userRole';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  @Input() updateSelecteROles= new EventEmitter();
  user:UserRole;
  //roles: any[];
  roles: { name: string, checked: boolean }[] = [];

constructor(public bsModalRef:BsModalRef){}
  ngOnInit(): void {

  }
  updateRoles(){
    this.updateSelecteROles.emit(this.roles);
    this.bsModalRef.hide();
  }
  // updateRoles() {
  //   if (Array.isArray(this.roles)) { // Check if roles is an array
  //     this.updateSelecteROles.emit(this.roles);
  //     this.bsModalRef.hide();
  //   }
  // }

}
