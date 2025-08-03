import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7260/api/UserMaster';

  constructor(private http: HttpClient) { }

  // getUsers(): Observable<User[]> {
  //   return this.http.get<User[]>(this.apiUrl);
  // }
//   getUsers(search: string = '', page: number = 1, pageSize: number = 5, sortBy: string = 'userID', sortOrder: string = 'asc'): Observable<any> {
//   const params = new HttpParams()
//     .set('search', search)
//     .set('page', page)
//     .set('pageSize', pageSize)
//     .set('sortBy', sortBy)
//     .set('sortOrder', sortOrder);

//   return this.http.get(`${this.apiUrl}/UserMaster`, { params });
// }
getUsers(searchTerm: string = '', sortBy: string = 'id', sortDirection: string = 'asc', pageNumber: number = 1, pageSize: number = 10): Observable<any> {
  const params = {
    searchTerm,
    sortBy,
    sortDirection,
    pageNumber,
    pageSize
  };

  return this.http.get(`${this.apiUrl}`, { params });
}



  addUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${user.userID}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}