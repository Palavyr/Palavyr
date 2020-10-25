import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage, serverUrl, SPECIAL_HEADERS } from "./clientUtils";
import axios, { AxiosResponse, AxiosInstance } from "axios";


export class LogoutClient {
    private client: AxiosInstance
    constructor(serverURL: string = serverUrl) {

        var sessionId = getSessionIdFromLocalStorage();
        var authToken = getJwtTokenFromLocalStorage();
        // console.log("Session Id used with Logout Client: " + sessionId)

        this.client = axios.create(
            {
                headers: {
                    action: "logout",
                    sessionId: sessionId,
                    Authorization: "Bearer " + authToken, //include space after Bearer
                    ...SPECIAL_HEADERS

                }
            }
        )
        this.client.defaults.baseURL = serverURL + '/api/';
    }

    public Logout = {
        RequestLogout: async (sessionId: string): Promise<AxiosResponse> => this.client.post("authentication/logout", { SessionId: sessionId })
    }
}