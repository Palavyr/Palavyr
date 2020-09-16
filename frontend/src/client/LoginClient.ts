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
        RequestLogin: async (username: string, email: string, password: string): Promise<AxiosResponse> => this.client.post("authentication/login",
        {
            UserName: username,
            EmailAddress: email,
            Password: password
        })
    }

    public Session = {
        RequestSession: async (sessionId: string): Promise<AxiosResponse> => this.client.post(`authentication/session/${sessionId}`)
    }

    public Account = {
        registerNewAccount: async (EmailAddress: string, Password: string): Promise<AxiosResponse> => this.client.post(`account/create`, { EmailAddress, Password })
    }
}
