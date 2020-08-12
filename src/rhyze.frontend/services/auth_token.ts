import jwtDecode from "jwt-decode";
import Cookie from "js-cookie";
import { AppCookies } from "./auth_service";

export type DecodedToken = {
    readonly email: string;
    readonly exp: number;
}

export class AuthToken {
    readonly decodedToken: DecodedToken;

    constructor(readonly token?: string, readonly refreshToken?: string) {
        // we are going to default to an expired decodedToken
        this.decodedToken = { email: "", exp: 0 };

        // then try and decode the jwt using jwt-decode
        try {
            if (token) this.decodedToken = jwtDecode(token);
        } catch (e) {
        }
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

    static storeTokens(idToken: string, refreshToken: string) {
        Cookie.set(AppCookies.authToken, idToken);
        Cookie.set(AppCookies.refreshToken, refreshToken);
    }

    static clearTokens() {
        this.storeTokens('', '');
    }
}