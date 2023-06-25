import { User } from "./user";

export class UserParams{
  gender:string;
  minAge:number;
  maxAge:number;
  pageNumber:number;
  pageSize:number;
  orderBy='lastActive'
  constructor(user:User){
    this.gender = user.gender === 'female' ?'male': 'female';
  }
}


