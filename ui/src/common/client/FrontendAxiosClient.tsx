import { ErrorResponse, SetState } from "@Palavyr-Types";
import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { ApiErrors } from "frontend/dashboard/layouts/Errors/ApiErrors";
import { SessionStorage } from "@localStorage/sessionStorage";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";
import { Loaders } from "./Loaders";

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
    S3Key = "S3Key",
    SupportedUnitIds = "SupportedUnitIds",
}

export class AxiosClient implements IAxiosClient {
    public client: AxiosInstance;
    private apiErrors?: ApiErrors;
    private loaders?: Loaders;
    private action: string = "tubmcgubs";
    private sessionIdCallback?: () => string;
    private authTokenCallback?: () => string;
    public sessionId: string;

    constructor(apiErrors?: ApiErrors, loaders?: Loaders, action?: string, sessionIdCallback?: () => string, authTokenCallback?: () => string) {
        this.apiErrors = apiErrors;
        this.loaders = loaders;
        this.sessionIdCallback = sessionIdCallback;
        this.authTokenCallback = authTokenCallback;
        if (action !== undefined) {
            this.action = action;
        }
    }

    private setAuthorizationContext() {
        if (this.apiErrors !== undefined) {
            this.apiErrors.ClearErrorPanel();
        }
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

    async get<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                const cachedValue = SessionStorage.getCacheValue(cacheId);
                if (cachedValue) {
                    return cachedValue as T;
                } else {
                    this.loaders?.startLoadingSpinner();
                    const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                    SessionStorage.setCacheValue(cacheId, response.data);
                    this.loaders?.stopLoadingSpinner();
                    return response.data as T;
                }
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        } else {
            try {
                this.loaders?.startLoadingSpinner();
                const response = (await this.client.get(url, config)) as AxiosResponse<T>;
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        }
    }

    async post<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                this.loaders?.startLoadingSpinner();

                const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                SessionStorage.setCacheValue(cacheId, response.data);
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        } else {
            try {
                this.loaders?.startLoadingSpinner();

                const response = (await this.client.post(url, payload, config)) as AxiosResponse<T>;
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        }
    }

    async put<T, S>(url: string, payload?: S, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                this.loaders?.startLoadingSpinner();

                const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                SessionStorage.setCacheValue(cacheId, response.data);
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        } else {
            try {
                this.loaders?.startLoadingSpinner();

                const response = (await this.client.put(url, payload, config)) as AxiosResponse<T>;
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        }
    }

    async delete<T>(url: string, cacheId?: CacheIds, config?: AxiosRequestConfig): Promise<T> {
        this.setAuthorizationContext();
        if (cacheId) {
            try {
                this.loaders?.startLoadingSpinner();

                const response = await this.client.delete(url, config);

                const data = response.data as T;
                if (data) {
                    SessionStorage.setCacheValue(cacheId, data);
                } else {
                    SessionStorage.clearCacheValue(cacheId);
                }
                this.loaders?.stopLoadingSpinner();
                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        } else {
            try {
                this.loaders?.startLoadingSpinner();

                const response = await this.client.delete(url, config);
                this.loaders?.stopLoadingSpinner();

                return response.data as T;
            } catch (error) {
                this.ProcessErrorResponse(error);
                return Promise.resolve((null as unknown) as T);
            }
        }
    }

    private ProcessErrorResponse(rawError: { response: { data: string } }) {
        const error = this.parseError(rawError);

        try {
            this.loaders?.stopLoadingSpinner();
        } catch {}

        try {
            this.apiErrors?.SetErrorSnack(error.message);
            this.apiErrors?.SetErrorPanel(error);
        } catch {}

        throw new Error(error.message);
    }

    private parseError(rawError: { response: { data: string } }): ErrorResponse {
        try {
            const errorObject = JSON.parse(rawError.response.data);
            return { message: errorObject.Message, additionalMessages: errorObject.AdditionalMessages, statusCode: errorObject.StatusCode };
        } catch {
            const serverErrorMessage = { message: "Failed to retrieve data from the server." } as ErrorResponse;
            this.apiErrors?.SetErrorPanel(serverErrorMessage);
            throw new Error(serverErrorMessage.message);
        }
    }
}
