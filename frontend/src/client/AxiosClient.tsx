import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";

interface IAxiosClient {
    get<T>(url: string, config: AxiosRequestConfig): Promise<T>;
    post<T>(url: string, payload: T, config: AxiosRequestConfig): Promise<T>;
    put<T>(url: string, payload: T, config: AxiosRequestConfig): Promise<T>;
    delete<T>(url: string): Promise<T>;
}

export class AxiosClient implements IAxiosClient {
    private client: AxiosInstance;

    constructor(action: string, sessionIdCallback?: () => string, authTokenCallback?: () => string) {
        let headers: any = { action: action, ...SPECIAL_HEADERS };

        if (sessionIdCallback) {
            const sessionId = sessionIdCallback();
            headers = { ...headers, sessionId: sessionId };
        }
        if (authTokenCallback) {
            const authToken = authTokenCallback();
            headers = { ...headers, Authorization: "Bearer " + authToken }; //include space after Bearer
        }

        this.client = axios.create({ headers: headers });
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve, reject) => {
            try {
                const response = await this.client.get(url, config);
                resolve(response.data as T);
            } catch (response) {
                reject(response);
            }
        });
    }

    post<T, S>(url: string, payload?: S, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve, reject) => {
            try {
                const response = await this.client.post(url, payload, config);
                resolve(response.data as T);
            } catch (response) {
                reject(response);
            }
        });
    }

    put<T, S>(url: string, payload?: S, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve, reject) => {
            try {
                const response = await this.client.put(url, payload, config);
                resolve(response.data as T);
            } catch (response) {
                reject(response);
            }
        });
    }

    delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve, reject) => {
            try {
                const response = await this.client.delete(url, config);
                resolve(response.data as T);
            } catch (response) {
                reject(response);
            }
        });
    }
}
