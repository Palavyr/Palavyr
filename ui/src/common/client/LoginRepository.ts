import { CredentialsResource, PasswordResetRequestResource, PasswordVerificationResource, ResetPasswordResource } from "@common/types/api/ApiContracts";
import { RESET_PASSWORD_LINK } from "@constants";
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
            this.client.post<CredentialsResource, {}>(this.Routes.RequestLogin(), {
                EmailAddress: email,
                Password: password,
            }),
    };

    public Status = {
        CheckIfLoggedIn: async () => this.client.get<boolean>(this.Routes.CheckIfLoggedIn()),
    };

    public Account = {
        RegisterNewAccount: async (EmailAddress: string, Password: string) => this.client.post<CredentialsResource, {}>(this.Routes.RegisterNewAccount(), { EmailAddress, Password }),
    };

    public Reset = {
        ResetPasswordRequest: async (emailAddress: string) =>
            this.client.post<PasswordResetRequestResource, {}>(this.Routes.ResetPasswordRequest(), {
                EmailAddress: emailAddress,
                ResetPasswordLinkTemplate: RESET_PASSWORD_LINK,
            }),
        VerifyResetIdentity: async (accessToken: string) => this.client.post<PasswordVerificationResource, {}>(this.Routes.VerifyResetIdentity(accessToken)),
        ResetPassword: async (password: string, secretKey: string) => this.resetClient.post<ResetPasswordResource, {}>(this.Routes.ResetPassword(secretKey), { Password: password }),
    };
}
