import { HttpClient, HttpParams } from "@angular/common/http";
import { PaginatedResult } from "../_models/pagination";
import { map } from "rxjs";

export function getPaginatedResult<T>(url, params,http:HttpClient) {
  const paginatedResult:PaginatedResult<T> = new PaginatedResult<T>();

  return http.get<T>(this.baseUrl + 'USers', { observe: 'response', params }).pipe(
    map(response => {
      this.paginatedResult.result = response.body;
      if (response.headers.get('Pagination') !== null) {
        this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return this.paginatedResult;
    })

  );
}

export function getPaginationHeaders(pageNumber: number, pageSize:number){
let params= new HttpParams();
if (pageNumber !== undefined) {
  params=params.append('pageNumber',pageNumber.toString());

}
if (pageSize !== undefined) {
  params= params.append('pageSize', pageSize.toString());

}
 return params;

}
