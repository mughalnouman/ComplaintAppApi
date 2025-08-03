// src/app/user/user.ts

export interface User {
  userID: number;        // Unique identifier for the user (primary key)
  email: string;     // Email address of the user (required)
  mobile: string;     // Contact phone number (required)
  city: string;
  state?: string;
  address: string;  // City name (optional, as it's nullable in the database)
}
