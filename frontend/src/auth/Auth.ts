import { SessionStorage } from "localStorage/sessionStorage";
import { LoginClient } from "client/LoginClient";
import { LogoutClient } from "client/LogoutClient";
import { ApiClient } from "@api-client/Client";
import { googleOAuthClientId } from "@api-client/clientUtils";
import { Credentials } from "@Palavyr-Types";

class Auth {
    private authenticated: boolean = false;
    private isActive: boolean = false;
    private loginClient = new LoginClient();

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
            const { data: authenticationResponse } = await this.loginClient.Account.registerNewAccount(email, password);
            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error trying to reach the server.");
            return null;
        }
    }

    async registerWithGoogle(oneTimeCode: string, tokenId: string, callback: () => void, errorCallback: (response) => void) {
        const { data: authenticationResponse } = await this.loginClient.Account.registerNewAccountWithGoogle(oneTimeCode, tokenId);
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
    }

    private async processAuthenticationResponse(authenticationResponse: Credentials, callback: () => any, errorCallback: (response: Credentials) => any) {
        if (authenticationResponse.authenticated) {
            this.authenticated = true;
            SessionStorage.setAuthorization(authenticationResponse.sessionId, authenticationResponse.jwtToken);
            SessionStorage.setEmailAddress(authenticationResponse.emailAddress);

            const _client = new ApiClient(); // needs to be authenticated

            const { data: accountIsActive } = await _client.Settings.Account.checkIsActive();
            this.isActive = accountIsActive;
            SessionStorage.setIsActive(accountIsActive);

            callback();
            return true;
        } else {
            this.authenticated = false;
            SessionStorage.unsetAuthorization();
            errorCallback(authenticationResponse);
            return false;
        }
    }

    async login(email: string | null, password: string | null, callback: () => any, errorCallback: (response: Credentials) => any) {
        if (email === null || password === null) return false;
        try {
            const { data: authenticationResponse } = await this.loginClient.Login.RequestLogin(email, password);
            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error attempting to reach the server.");
            return null;
        }
    }

    async loginWithGoogle(oneTimeCode: string, tokenId: string, callback: () => void, errorCallback: (response: Credentials) => void) {
        try {
            const { data: authenticationResponse } = await this.loginClient.Login.RequestLoginWithGoogleToken(oneTimeCode, tokenId);
            console.log(authenticationResponse);

            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error attempting to reach the server.");
            return null;
        }
    }

    async loginFromMemory(callback: any) {
        const token = SessionStorage.getJwtToken();
        if (token) {
            if (await this.loginClient.Status.CheckIfLoggedIn()) {
                callback();
            }
        }
    }

    async logout(callback: () => any) {
        const sessionId = SessionStorage.getSessionId();
        if (sessionId !== null && sessionId !== "") {
            const logoutClient = new LogoutClient(); // needs to be authenticated so we should instantiate this on the call
            await logoutClient.Logout.RequestLogout(sessionId);
            SessionStorage.unsetAuthorization();
            SessionStorage.unsetEmailAddress();
        }
        this.authenticated = false;
        callback();
    }

    async googleLogout(callback: () => any) {
        try {

            window.gapi.load("auth2", () => {
                window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true });
                window.gapi.auth2.getAuthInstance().then((auth2) => {
                    auth2.signOut().then(async () => {
                        auth2.disconnect().then(await this.logout(callback));
                    });
                });
            });
        } catch {
            await this.logout(callback);
        }
    }

    isAuthenticated() {
        return this.authenticated;
    }

    PerformLogout(logoutCallback: any) {
        const loginType = SessionStorage.getLoginType();

        if (loginType === SessionStorage.GoogleLoginType) {
            this.googleLogout(logoutCallback);
        } else {
            console.log("Logging Out");
            this.logout(logoutCallback);
        }
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
