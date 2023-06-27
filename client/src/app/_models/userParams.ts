import { User } from "./user";

export class UserParams{
  gender:string;
  minAge:10;
  maxAge:150;
  pageNumber:number;
  pageSize:number;
  orderBy='lastActive'
  constructor(user:User){
    this.gender = user.gender === 'male' ?'female': 'male';
  }
}


