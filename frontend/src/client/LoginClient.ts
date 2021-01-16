import { RESET_PASSWORD_LINK } from "@constants";
import { Credentials, ResetEmailResponse, ResetPasswordResponse, VerificationResponse } from "@Palavyr-Types";
import axios, { AxiosInstance, AxiosResponse } from "axios";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";

export class LoginClient {
    private client: AxiosInstance;
    private resetClient: AxiosInstance;

    constructor(serverURL: string = serverUrl) {
        this.client = axios.create({
            headers: {
                action: "login",
                ...SPECIAL_HEADERS,
            },
        });
        this.client.defaults.baseURL = serverURL + "/api/";

        this.resetClient = axios.create({
            headers: {
                action: "apiKeyAccess",
            },
        });
        this.resetClient.defaults.baseURL = serverURL + "/api/";
    }

    public Login = {
        RequestLogin: async (email: string, password: string): Promise<AxiosResponse<Credentials>> =>
            this.client.post("authentication/login", {
                EmailAddress: email,
                Password: password,
            }),

        RequestLoginWithGoogleToken: async (oneTimeCode: string, tokenId: string): Promise<AxiosResponse<Credentials>> =>
            this.client.post("authentication/login", {
                OneTimeCode: oneTimeCode,
                TokenId: tokenId,
            }),
    };

    public Status = {
        CheckIfLoggedIn: async (): Promise<AxiosResponse<boolean>> => this.client.get(`authentication/status`),
    };

    public Account = {
        registerNewAccount: async (EmailAddress: string, Password: string): Promise<AxiosResponse<Credentials>> => this.client.post(`account/create/default`, { EmailAddress, Password }),
        registerNewAccountWithGoogle: async (oneTimeCode: string, tokenId: string): Promise<AxiosResponse<Credentials>> =>
            this.client.post("account/create/google", {
                OneTimeCode: oneTimeCode,
                TokenId: tokenId,
            }),
    };

    public Reset = {
        resetPasswordRequest: async (emailAddress: string): Promise<AxiosResponse<ResetEmailResponse>> =>
            this.client.post(`authentication/password-reset-request`, {
                EmailAddress: emailAddress,
                ResetPasswordLinkTemplate: RESET_PASSWORD_LINK,
            }),
        verifyResetIdentity: async (accessToken: string): Promise<AxiosResponse<VerificationResponse>> => this.client.post(`authentication/verify-password-reset/${accessToken}`),
        resetPassword: async (password: string, secretKey: string): Promise<AxiosResponse<ResetPasswordResponse>> => this.resetClient.post(`authentication/reset-my-password?key=${secretKey}`, { Password: password }),
    };
}
