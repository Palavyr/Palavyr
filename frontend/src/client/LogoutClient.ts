import { getSessionIdFromLocalStorage, serverUrl, SPECIAL_HEADERS } from "./clientUtils";
import axios, { AxiosResponse, AxiosInstance } from "axios";


export class LogoutClient {
    private client: AxiosInstance
    constructor(serverURL: string = serverUrl) {

        var sessionId = getSessionIdFromLocalStorage();
        // console.log("Session Id used with Logout Client: " + sessionId)

        this.client = axios.create(
            {
                headers: {
                    action: "logout",
                    sessionId: sessionId,
                    ...SPECIAL_HEADERS

                }
            }
        )
        this.client.defaults.baseURL = serverURL + '/api/';
    }

    public Logout = {
        RequestLogout: async (): Promise<AxiosResponse> => this.client.post("authentication/logout")
    }
}