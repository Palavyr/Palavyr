import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { ApiErrors } from "dashboard/layouts/Errors/ApiErrors";
import { SessionStorage } from "localStorage/sessionStorage";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";

interface IAxiosClient {
    get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined>;
    post<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined>;
    put<T>(url: string, payload: T, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined>;
    delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined>;
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
    S3Key = "S3Key",
}

export class AxiosClient implements IAxiosClient {
    private client: AxiosInstance;
    private apiErrors: ApiErrors;
    private action: string = "tubmcgubs";
    private sessionIdCallback?: () => string;
    private authTokenCallback?: () => string;
    public sessionId: string;

    constructor(apiErrors: ApiErrors, action?: string, sessionIdCallback?: () => string, authTokenCallback?: () => string) {
        this.apiErrors = apiErrors;
        this.sessionIdCallback = sessionIdCallback;
        this.authTokenCallback = authTokenCallback;
        if (action !== undefined) {
            this.action = action;
        }
    }

    private setAuthorizationContext() {
        let headers: any = { action: this.action, ...SPECIAL_HEADERS };
        if (this.sessionIdCallback !== undefined) {
            const sessionId = this.sessionIdCallback();
            this.sessionId = sessionId;
            headers = { ...headers, sessionId: sessionId };
        }
        if (this.authTokenCallback !== undefined) {
            const authToken = this.authTokenCallback();
            headers = { ...headers, Authorization: "Bearer " + authToken }; //include space after Bearer
        }
        this.client = axios.create({ headers: headers });
        this.client.defaults.baseURL = serverUrl + "/api/";
    }

    async get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                const cachedValue = SessionStorage.getCacheValue(cacheId);
                if (cachedValue) {
                    return cachedValue as T;
                } else {
                    const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                    SessionStorage.setCacheValue(cacheId, response.data);
                    return response.data as T;
                }
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        } else {
            try {
                const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        }
    }

    async post<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T | undefined> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                SessionStorage.setCacheValue(cacheId, response.data);
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        } else {
            try {
                const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        }
    }

    async put<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                SessionStorage.setCacheValue(cacheId, response.data);
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        } else {
            try {
                const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        }
    }

    async delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                const response = await this.client.delete(url, config);
                const data = response.data as T;
                if (data) {
                    SessionStorage.setCacheValue(cacheId, data);
                } else {
                    SessionStorage.clearCacheValue(cacheId);
                }
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        } else {
            try {
                const response = await this.client.delete(url, config);
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
            }
        }
    }

    private ProcessErrorResponse(rawError: { response: { data: string } }) {
        const error = this.parseError(rawError);

        this.apiErrors.SetErrorSnack(error.messages[0]);
        this.apiErrors.SetErrorPanel(error.messages);

        throw new Error(error.messages.join(", "));
    }

    private parseError(rawError: { response: { data: string } }): ErrorResponse {
        const errorObject = JSON.parse(rawError.response.data);
        return { messages: errorObject.Messages, statusCode: errorObject.StatusCode };
    }
}

export type ErrorResponse = {
    messages: string[];
    statusCode: number;
};
