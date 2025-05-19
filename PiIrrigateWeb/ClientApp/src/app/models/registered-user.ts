export interface RegisteredUser {
  userId: string;          // Unique identifier for the user
  fullName: string;    // Full name of the user
  email: string;     // Email address of the user
  token: string;     // Authentication token for the user
}