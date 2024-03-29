import axios, { AxiosRequestConfig } from 'axios';
import { LoginInputs } from '../pages/login';
import { catchAuthError } from "./error";
import { AuthToken } from './auth_token';

export const AppCookies = {
    authToken: "rhyze.authToken",
    refreshToken: "rhyze.refreshToken"
};

type ErrorMessage = string;

export const login = async (inputs: LoginInputs): Promise<ErrorMessage | void> => {
    const data = new URLSearchParams(inputs);
    data.append("returnSecureToken", "true");
    const config = authConfig("https://www.googleapis.com");

    const res: any = await post(config, "/identitytoolkit/v3/relyingparty/verifyPassword", data).catch(catchAuthError);

    if (res.code === 400) {
        return "Invalid username and/or password.";
    } else if (res.error) {
        return res.error;
    }
    if (res.data && res.data.idToken && res.data.refreshToken) {
        AuthToken.storeTokens(res.data.idToken, res.data.refreshToken);
        return
    }
    return "Something unexpected happened!";
}

export const refreshToken = async (auth: AuthToken): Promise<ErrorMessage | AuthToken> => {
    const config = authConfig("https://securetoken.googleapis.com/");
    const data = new URLSearchParams(
        { grant_type: "refresh_token", refresh_token: auth.refreshToken }
    );

    const res: any = await post(config, "/v1/token", data).catch(catchAuthError);

    if (res.error) {
        return res.error;
    }

    if (res.data && res.data.id_token && res.data.refresh_token) {
        AuthToken.storeTokens(res.data.id_token, res.data.refresh_token);
        return new AuthToken(res.data.id_token, res.data.refresh_token);
    }

    return "Something unexpected happened!";
}

export const logout = () => AuthToken.clearTokens();

const post = (config: AxiosRequestConfig, url: string, data: URLSearchParams) => {
    return axios.post(url, data, config).catch(catchAuthError);
};

const authConfig = (baseURL: string): AxiosRequestConfig => {
    return {
        baseURL,
        params: { key: process.env.NEXT_PUBLIC_FIREBASE_AUTHKEY }
    }
}