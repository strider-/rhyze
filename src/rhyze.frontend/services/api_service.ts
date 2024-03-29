import { AuthToken } from "./auth_token";
import axios, { AxiosRequestConfig } from "axios";
import { catchApiError, ErrorResponse } from "./error";

export type Album = {
    id: string,
    name: string,
    artist: string,
    imageUrl: string,
    touchedUtc: Date
};

export type Track = {
    id: string,
    title: string,
    album: string,
    artist: string,
    albumArtist: string,
    trackNo: number,
    trackCount: number,
    discNo: number,
    discCount: number,
    year: number,
    duration: number,
    imageUrl: string,
    playCount: number
}

export const getAlbums = async (token: AuthToken, skip: number = 0, take: number = 50): Promise<ErrorResponse | Album[]> => {
    const config = getConfig(token);
    config.params = { skip, take };
    return await get<Album[]>('/albums', config);
}

export const getAlbum = async (token: AuthToken, id: string): Promise<ErrorResponse | Track[]> => {
    return await get<Track[]>(`/albums/${id}`, getConfig(token));
}

const get = <T>(path: string, config?: AxiosRequestConfig) => {
    return axios.get(path, config).then(res => res.data as T).catch(catchApiError);
};

const getConfig = (token: AuthToken): AxiosRequestConfig => {
    return {
        baseURL: process.env.NEXT_PUBLIC_RHYZE_API_URL,
        headers: { Authorization: `Bearer ${token.token}` }
    }
}