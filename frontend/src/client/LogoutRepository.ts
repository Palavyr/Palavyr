import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";
import { AxiosClient } from "./AxiosClient";
import { ApiErrors } from "dashboard/layouts/Errors/ApiErrors";

export class LogoutRepository {
    private client: AxiosClient;
    constructor() {
        this.client = new AxiosClient({} as ApiErrors, "logout", getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);
    }

    public Logout = {
        RequestLogout: async (sessionId: string) => this.client.post<void, {}>("authentication/logout", { SessionId: sessionId }),
    };
}
