import { ApiRoutes } from "./ApiRoutes";
import { getJwtTokenFromLocalStorage, getSessionIdFromLocalStorage } from "./clientUtils";
import { AxiosClient } from "./FrontendAxiosClient";

export class LogoutRepository extends ApiRoutes {
    private client: AxiosClient;
    constructor() {
        super();
        this.client = new AxiosClient(undefined, undefined, "logout", getSessionIdFromLocalStorage, getJwtTokenFromLocalStorage);
    }

    public Logout = {
        RequestLogout: async (sessionId: string) => this.client.post<void, {}>(this.Routes.RequestLogout(), { SessionId: sessionId }),
    };
}
