import jwtDecode from "jwt-decode";
import Cookie from "js-cookie";
import Router from "next/router";
import { AppCookies } from "./auth_service";

export type DecodedToken = {
  readonly email: string;
  readonly exp: number;
}

export class AuthToken {
  readonly decodedToken: DecodedToken;

  constructor(readonly token?: string) {
    // we are going to default to an expired decodedToken
    this.decodedToken = { email: "", exp: 0 };

    // then try and decode the jwt using jwt-decode
    try {
      if (token) this.decodedToken = jwtDecode(token);
    } catch (e) {
    }
  }

  get authorizationString() {
    return `Bearer ${this.token}`;
  }

  get expiresAt(): Date {
    return new Date(this.decodedToken.exp * 1000);
  }

  get isExpired(): boolean {
    return new Date() > this.expiresAt;
  }

  get isValid(): boolean {
    return !this.isExpired;
  }

  static async storeTokens(idToken: string, refreshToken: string) {
    Cookie.set(AppCookies.authToken, idToken);
    Cookie.set(AppCookies.refreshToken, refreshToken);
    await Router.push("/");
  }

  static async clearTokens(){
    Cookie.set(AppCookies.authToken, '');
    Cookie.set(AppCookies.refreshToken, '');
    await Router.push("/login?loggedOut=true");
  }
}