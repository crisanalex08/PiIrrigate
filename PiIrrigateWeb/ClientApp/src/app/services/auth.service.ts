import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { RegisteredUser } from '../models/registered-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  private _authSub$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _userDetails: RegisteredUser | null = null;
  private baseUrl = 'https://localhost:7133/UserManagement';
  private readonly TOKEN_KEY = 'auth_token'; // Key for storing the token
  private readonly USER_DETAILS_KEY = 'user_details'; // Key for storing user details

  public get isAuthenticated$(): Observable<boolean> {
    return this._authSub$.asObservable();
  }

  constructor(private http: HttpClient) {
    // Check if a token and user details exist in sessionStorage on service initialization
    if (this.isSessionStorageAvailable()) {
      const token = sessionStorage.getItem(this.TOKEN_KEY);
      const userDetails = sessionStorage.getItem(this.USER_DETAILS_KEY);
      if (token && userDetails) {
        this._authSub$.next(true);
        this._userDetails = JSON.parse(userDetails);
      }
    }
  }

  public ngOnDestroy(): void {
    this._authSub$.next(false);
    this._authSub$.complete();
  }

  public login(email: string, password: string): Observable<any> {
    return this.http.post<RegisteredUser>(`${this.baseUrl}/login`, { email, password }).pipe(
      tap((response: RegisteredUser) => {
        if (response && response.token) {
          this._authSub$.next(true);
          this.cacheSession(response);
        } else {
          this._authSub$.next(false);
        }
      })
    );
  }

  public register(fullName: string, email: string, password: string): Observable<any> {
    return this.http.post<RegisteredUser>(`${this.baseUrl}/register`, { fullName, email, password }).pipe(
      tap((response: RegisteredUser) => {
        if (response && response.token) {
          this._authSub$.next(true);
          this.cacheSession(response);
        } else {
          this._authSub$.next(false);
        }
      })
    );
  }

  public logout(): void {
    this._authSub$.next(false);
    this._userDetails = null;
    if (this.isSessionStorageAvailable()) {
      sessionStorage.removeItem(this.TOKEN_KEY);
      sessionStorage.removeItem(this.USER_DETAILS_KEY);
    }
  }

  public getUserDetails(): RegisteredUser | null {
    return this._userDetails;
  }

  public getToken(): string | null {
    if (this.isSessionStorageAvailable()) {
      return sessionStorage.getItem(this.TOKEN_KEY);
    }
    return null;
  }

  private cacheSession(user: RegisteredUser): void {
    if (this.isSessionStorageAvailable()) {
      sessionStorage.setItem(this.TOKEN_KEY, user.token);
      sessionStorage.setItem(this.USER_DETAILS_KEY, JSON.stringify(user));
    }
    this._userDetails = user;
  }

  private isSessionStorageAvailable(): boolean {
    try {
      return typeof sessionStorage !== 'undefined';
    } catch {
      return false;
    }
  }
}