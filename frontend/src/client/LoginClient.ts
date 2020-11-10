import axios, { AxiosInstance, AxiosResponse } from "axios";
import { serverUrl, SPECIAL_HEADERS } from "./clientUtils";


export class LoginClient {
    private client: AxiosInstance
    constructor(serverURL: string = serverUrl) {

        this.client = axios.create(
            {
                headers: {
                    action: "login",
                    ...SPECIAL_HEADERS
                }
            }
        )
        this.client.defaults.baseURL = serverURL + "/api/";
    }

    public Login = {
        RequestLogin: async ( email: string, password: string): Promise<AxiosResponse> => this.client.post("authentication/login",
        {
            EmailAddress: email,
            Password: password
        }),

        RequestLoginWithGoogleToken: async (oneTimeCode: string, accessToken: string, tokenId: string): Promise<AxiosResponse> => this.client.post("authentication/login", {
            OneTimeCode: oneTimeCode,
            AccessToken: accessToken,
            TokenId: tokenId
        })
    }

    public Status = {
        CheckIfLoggedIn: async (): Promise<AxiosResponse> => this.client.get(`authentication/status`)
    }

    public Account = {
        registerNewAccount: async (EmailAddress: string, Password: string): Promise<AxiosResponse> => this.client.post(`account/create`, { EmailAddress, Password }),
        registerNewAccountWithGoogle: async (oneTimeCode: string, accessToken: string, tokenId: string): Promise<AxiosResponse> => this.client.post("account/create/google", {
            OneTimeCode: oneTimeCode, AccessToken: accessToken, TokenId: tokenId
        })
    }
}
