import { Component, OnInit } from '@angular/core';
import { User } from '../../../model/user';
import { UserService } from '../../../service/user.service';
import { CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-crud',
  templateUrl: './user-crud.component.html',
  imports: [CommonModule, FormsModule, NgIf],
  standalone: true
})
export class UserCrudComponent implements OnInit {
  users: User[] = [];

  // Default empty user form
  userForm: User = {
    userID: 0,
    email: '',
    mobile: '',
    city: '',
    state: '',
    address: ''
  };

  // State flags
  isEditing = false;
  errorMessage = '';

  // Pagination, sorting, and searching
  totalCount = 0;
  searchTerm = '';
  sortBy = 'userID';
  sortDirection = 'asc';
  pageNumber = 1;
  pageSize = 5;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    // Load users when component is initialized
    this.getAllUsers();
  }
allowOnlyNumbers(event: KeyboardEvent): void {
  const charCode = event.which ? event.which : event.keyCode;
  if (charCode < 48 || charCode > 57) {
    event.preventDefault();
  }
}

  // ✅ Get users from backend with all parameters
  getAllUsers(): void {
    this.userService.getUsers(
      this.searchTerm,
      this.sortBy,
      this.sortDirection,
      this.pageNumber,
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.users = response.users;
        this.totalCount = response.totalCount;
      },
      error: () => {
        this.users = [];
        this.totalCount = 0;
      }
    });
  }

  // ✅ Save or update user
  saveUser(): void {
    if (this.isEditing) {
      this.userService.updateUser(this.userForm).subscribe({
        next: () => {
          this.getAllUsers();
          this.resetForm();
          this.errorMessage = '';
        },
        error: (err) => {
          this.errorMessage = err.error;
        }
      });
    } else {
      this.userService.addUser(this.userForm).subscribe({
        next: () => {
          this.getAllUsers();
          this.resetForm();
          this.errorMessage = '';
        },
        error: (err) => {
          this.errorMessage = err.error;
        }
      });
    }
  }

  // ✅ Edit user
  editUser(user: User): void {
    this.userForm = { ...user };
    this.isEditing = true;
  }

  // ✅ Delete user
  deleteUser(id: number): void {
    this.userService.deleteUser(id).subscribe(() => {
      this.getAllUsers();
    });
  }

  // ✅ Reset form state
  resetForm(): void {
    this.userForm = {
      userID: 0,
      email: '',
      mobile: '',
      city: '',
      state: '',
      address: ''
    };
    this.isEditing = false;
  }

  // ✅ Search input change handler
  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.pageNumber = 1;
    this.getAllUsers();
  }

  // ✅ Sort column handler
  onSort(column: string): void {
    if (this.sortBy === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = column;
      this.sortDirection = 'asc';
    }
    this.getAllUsers();
  }

  // ✅ Handle page change
  onPageChange(page: number): void {
    this.pageNumber = page;
    this.getAllUsers();
  }

  // ✅ Utility: total number of pages
  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // ✅ Utility: generate array of page numbers
  getPageNumbers(): number[] {
    return Array.from({ length: this.getTotalPages() }, (_, i) => i + 1);
  }
}
