import { RESET_PASSWORD_LINK } from "@constants";
import { Credentials, ResetEmailResponse, ResetPasswordResponse, VerificationResponse } from "@Palavyr-Types";
import { ApiErrors } from "dashboard/layouts/Errors/ApiErrors";
import { AxiosClient } from "./AxiosClient";

export class LoginRepository {
    private client: AxiosClient;
    private resetClient: AxiosClient;

    constructor() {
        this.client = new AxiosClient({} as ApiErrors, "login");
        this.resetClient = new AxiosClient({} as ApiErrors, "apiKeyAccess");
    }

    public Login = {
        RequestLogin: async (email: string, password: string) =>
            this.client.post<Credentials, {}>("authentication/login", {
                EmailAddress: email,
                Password: password,
            }),

        RequestLoginWithGoogleToken: async (oneTimeCode: string, tokenId: string) =>
            this.client.post<Credentials, {}>("authentication/login", {
                OneTimeCode: oneTimeCode,
                TokenId: tokenId,
            }),
    };

    public Status = {
        CheckIfLoggedIn: async () => this.client.get<boolean>(`authentication/status`),
    };

    public Account = {
        registerNewAccount: async (EmailAddress: string, Password: string) => this.client.post<Credentials, {}>(`account/create/default`, { EmailAddress, Password }),
        registerNewAccountWithGoogle: async (oneTimeCode: string, tokenId: string) =>
            this.client.post<Credentials, {}>("account/create/google", {
                OneTimeCode: oneTimeCode,
                TokenId: tokenId,
            }),
    };

    public Reset = {
        resetPasswordRequest: async (emailAddress: string) =>
            this.client.post<ResetEmailResponse, {}>(`authentication/password-reset-request`, {
                EmailAddress: emailAddress,
                ResetPasswordLinkTemplate: RESET_PASSWORD_LINK,
            }),
        verifyResetIdentity: async (accessToken: string) => this.client.post<VerificationResponse, {}>(`authentication/verify-password-reset/${accessToken}`),
        resetPassword: async (password: string, secretKey: string) => this.resetClient.post<ResetPasswordResponse, {}>(`authentication/reset-my-password?key=${secretKey}`, { Password: password }),
    };
}
