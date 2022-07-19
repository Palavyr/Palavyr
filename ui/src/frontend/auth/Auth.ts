import { SessionStorage } from "@localStorage/sessionStorage";
import { LoginRepository } from "@common/client/LoginRepository";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { CredentialsResource } from "@Palavyr-Types";
import { LogoutRepository } from "@common/client/LogoutRepository";

class Auth {
    private authenticated: boolean = false;
    private isActive: boolean = false;
    private loginClient = new LoginRepository();

    get accountIsActive() {
        return this.isActive;
    }
    get accountIsAuthenticated() {
        return this.authenticated;
    }

    constructor() {
        this.authenticated = SessionStorage.isAuthenticated() === true ? true : false;
        this.isActive = SessionStorage.isActive() === true ? true : false;
    }

    async register(email: string, password: string, callback: () => any, errorCallback: (response) => any) {
        try {
            const authenticationResponse = await this.loginClient.Account.RegisterNewAccount(email, password);
            return await this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error trying to reach the server.");
            return null;
        }
    }

    private async processAuthenticationResponse(authenticationResponse: CredentialsResource, successRedirectToDashboard: () => any, errorCallback: (response: CredentialsResource) => any): Promise<boolean> {
        if (authenticationResponse.authenticated) {
            this.authenticated = true;
            SessionStorage.setAuthorization(authenticationResponse.sessionId, authenticationResponse.jwtToken);
            SessionStorage.setEmailAddress(authenticationResponse.emailAddress);

            const _client = new PalavyrRepository(); // needs to be authenticated

            const accountIsActive = await _client.Settings.Account.CheckIsActive();
            this.isActive = accountIsActive;
            SessionStorage.setIsActive(accountIsActive);

            await successRedirectToDashboard();
            return Promise.resolve(true);
        } else {
            this.authenticated = false;
            SessionStorage.unsetAuthorization();
            await errorCallback(authenticationResponse);
            return Promise.resolve(false);
        }
    }

    async login(email: string | null, password: string | null, callback: () => any, errorCallback: (response: CredentialsResource) => any) {
        if (email === null || password === null) return false;
        try {
            const authenticationResponse = await this.loginClient.Login.RequestLogin(email, password);
            return await this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error attempting to reach the server.");
            return null;
        }
    }

    async loginFromMemory(callback: any) {
        const token = SessionStorage.getJwtToken();
        if (token) {
            if (await this.loginClient.Status.CheckIfLoggedIn()) {
                await callback();
            }
        }
    }

    async logout(callback: () => any) {
        const sessionId = SessionStorage.getSessionId();
        if (sessionId !== null && sessionId !== "") {
            const logoutRepository = new LogoutRepository(); // needs to be authenticated so we should instantiate this on the call
            await logoutRepository.Logout.RequestLogout(sessionId);
            SessionStorage.unsetAuthorization();
            SessionStorage.unsetEmailAddress();
            SessionStorage.unsetOtherSessionItems();
        }
        this.authenticated = false;
        await callback();
    }

    isAuthenticated() {
        return this.authenticated;
    }

    async PerformLogout(logoutCallback: any) {
        await this.logout(logoutCallback);
    }

    ClearAuthentication() {
        SessionStorage.unsetAuthorization();
    }

    SetIsActive() {
        SessionStorage.setIsActive(true);
    }
}

const AuthObject = new Auth();
export default AuthObject;
