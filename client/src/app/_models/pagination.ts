


// export interface Pagination{
//   currentPage:number;
//   itemsPerPage:number;
//   totalItems:number;
//   totalPages:number;

// }

export class PaginatedResult<T>{
  result: T;
  pagination: Pagination;

  constructor() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 5,
      totalItems: 10,
      totalPages: 15
    }
  }
}
export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// export class PaginatedResult<T> {
//     result: T;
//     pagination: Pagination;
// }
