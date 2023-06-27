import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { UserRole } from 'src/app/_models/userRole';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-managment',
  templateUrl: './user-managment.component.html',
  styleUrls: ['./user-managment.component.css']
})
export class UserManagmentComponent  implements OnInit {
 users:Partial<UserRole[]>;
 bsmodalRef:BsModalRef;
constructor(private adminservice:AdminService, private modalservice:BsModalService){}

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles(){
    this.adminservice.getUsersWithRoles().subscribe(users=>{
      this.users=users;
    })
  }

  openRolesModal(user:UserRole){

    const config={
      class:'modal-dialog-centerd',
      initialState:{
       user,
       roles:this.getRolesArray(user)
      }
    }

    this.bsmodalRef=this.modalservice.show(RolesModalComponent, config)
    this.bsmodalRef.content.updateSelecteROles.subscribe(value=>{
      if(value){
      const rolesTOUpdate={
        roles:[...value.filter(el=>el.checked===true).map(el=>el.name)]
      };
      if(rolesTOUpdate){
        this.adminservice.updateUserRoles(user.userName, rolesTOUpdate.roles).subscribe(()=>{
          user.roles= [...rolesTOUpdate.roles]
        })

        }
        }
    })
  }
  private getRolesArray(user){
    const roles=[];
    const userRoles= user.roles;
    const availableRoles: any[]=[
      {name:'Admin', value:'Admin'},
      {name:'Moderator', value:'Moderator'},
      {name:'member', value:'member'},

    ];
     availableRoles.forEach(role=>{
      let isMatch=false;
      for(const userRole of userRoles){
        if(role.name === userRole){
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if(!isMatch){
        role.checked=false;
        roles.push(role);

      }
     })
     return roles;

  }

}
