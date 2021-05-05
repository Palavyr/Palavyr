import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";
import { AxiosClient } from "./AxiosClient";

export class LogoutClient {
    private client: AxiosClient;
    constructor() {
        this.client = new AxiosClient("logout", getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);
    }

    public Logout = {
        RequestLogout: async (sessionId: string) => this.client.post<void, {}>("authentication/logout", { SessionId: sessionId }),
    };
}
