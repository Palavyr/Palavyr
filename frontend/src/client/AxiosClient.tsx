import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { SessionStorage } from "localStorage/sessionStorage";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";

interface IAxiosClient {
    get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;
    post<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;
    put<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;
    delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T>;
}

export enum CacheIds {
    Areas = "Areas",
    CurrentPlanMeta = "CurrentPlanMeta",
    Enquiries = "Enquiries",
    Conversation = "Conversation",
    WidgetPrefs = "WidgetPrefs",
    CustomerId = "CustomerId",
    Attachments = "Attachments",
    PalavyrConfiguration = "PalavyrConfiguration",
    CompanyName = "CompanyName",
    Email = "Email",
    PhoneNumber = "PhoneNumber",
    Locale = "Locale",
    Logo = "Logo",
    ShowSeenQueries = "ShowSeenQueries",
    NeedsPassword = "NeedsPassword",
    WidgetState = "WidgetState",
    Images = "Images",
}

export async function DoRequest<T>(resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void, request: Promise<AxiosResponse<T>>, cacheId?: CacheIds) {
    if (cacheId) {
        try {
            let result: T;
            const cachedValue = SessionStorage.getCacheValue(cacheId);
            if (cachedValue) {
                result = cachedValue as T;
            } else {
                const response = (await request) as AxiosResponse<T>;
                SessionStorage.setCacheValue(cacheId, response.data);
                resolve(response.data as T);
            }
        } catch (response) {
            reject(response);
        }
    } else {
        try {
            const response = (await request) as AxiosResponse<T>;
            resolve(response.data as T);
        } catch (response) {
            reject(response);
        }
    }
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

    get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {
            if (cacheId) {
                try {
                    let result: T;
                    const cachedValue = SessionStorage.getCacheValue(cacheId);
                    if (cachedValue) {
                        resolve(cachedValue as T);
                    } else {
                        const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                        SessionStorage.setCacheValue(cacheId, response.data);
                        resolve(response.data as T);
                    }
                } catch (response) {
                    reject(response);
                }
            } else {
                try {
                    const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            }
        });
    }

    post<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {
            if (cacheId) {
                try {
                    const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                    SessionStorage.setCacheValue(cacheId, response.data);
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            } else {
                try {
                    const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            }
        });
    }

    put<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {
            if (cacheId) {
                try {
                    const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                    SessionStorage.setCacheValue(cacheId, response.data);
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            } else {
                try {
                    const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            }
        });
    }

    delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        return new Promise<T>(async (resolve: (value: T | PromiseLike<T>) => void, reject: (reason?: any) => void) => {
            if (cacheId) {
                try {
                    const response = await this.client.delete(url, config);
                    const data = response.data as T;
                    if (data) {
                        SessionStorage.setCacheValue(cacheId, data);
                    } else {
                        SessionStorage.clearCacheValue(cacheId);
                    }
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            } else {
                try {
                    const response = await this.client.delete(url, config);
                    resolve(response.data as T);
                } catch (response) {
                    reject(response);
                }
            }
        });
    }
}
