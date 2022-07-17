import { RESET_PASSWORD_LINK } from "@constants";
import { Credentials, ResetEmailResponse, ResetPasswordResponse, VerificationResponse } from "@Palavyr-Types";
import { ApiRoutes } from "./ApiRoutes";
import { AxiosClient } from "./FrontendAxiosClient";
export class LoginRepository extends ApiRoutes {
    private client: AxiosClient;
    private resetClient: AxiosClient;

    constructor() {
        super();
        this.client = new AxiosClient(undefined, undefined, "login");
        this.resetClient = new AxiosClient(undefined, undefined, "apiKeyAccess");
    }

    public Login = {
        RequestLogin: async (email: string, password: string) =>
            this.client.post<Credentials, {}>(this.Routes.RequestLogin(), {
                EmailAddress: email,
                Password: password,
            }),
    };

    public Status = {
        CheckIfLoggedIn: async () => this.client.get<boolean>(this.Routes.CheckIfLoggedIn()),
    };

    public Account = {
        RegisterNewAccount: async (EmailAddress: string, Password: string) => this.client.post<Credentials, {}>(this.Routes.RegisterNewAccount(), { EmailAddress, Password }),
    };

    public Reset = {
        ResetPasswordRequest: async (emailAddress: string) =>
            this.client.post<ResetEmailResponse, {}>(this.Routes.ResetPasswordRequest(), {
                EmailAddress: emailAddress,
                ResetPasswordLinkTemplate: RESET_PASSWORD_LINK,
            }),
        VerifyResetIdentity: async (accessToken: string) => this.client.post<VerificationResponse, {}>(this.Routes.VerifyResetIdentity(accessToken)),
        ResetPassword: async (password: string, secretKey: string) => this.resetClient.post<ResetPasswordResponse, {}>(this.Routes.ResetPassword(secretKey), { Password: password }),
    };
}
