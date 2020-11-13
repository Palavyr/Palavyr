import { LocalStorage } from "localStorage/localStorage";
import { LoginClient } from "client/LoginClient";
import { LogoutClient } from "client/LogoutClient";
import { ApiClient } from "@api-client/Client";
import { googleOAuthClientId } from "@api-client/clientUtils";

type Credentials = {
    jwtToken: string;
    apiKey: string;
    sessionId: string;
    emailAddress: string;
    authenticated: boolean;
    message: string;
};

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
        this.authenticated = LocalStorage.isAuthenticated() === true ? true : false;
        this.isActive = LocalStorage.isActive() === true ? true : false;
    }

    async register(email: string, password: string, callback: () => any, errorCallback: (response) => any) {
        try {
            const authenticationResponse = (await this.loginClient.Account.registerNewAccount(email, password)).data as Credentials; // TODO: Check that res is successfull before logging in
            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error trying to reach the server.")
        }

    }

    async registerWithGoogle(oneTimeCode: string, accessToken: string, tokenId: string, callback: () => void, errorCallback: (response) => void) {
        const authenticationResponse = (await this.loginClient.Account.registerNewAccountWithGoogle(oneTimeCode, accessToken, tokenId)).data as Credentials;
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
    }

    private async processAuthenticationResponse(authenticationResponse: Credentials, callback: () => any, errorCallback: (response) => any) {
        if (authenticationResponse.authenticated) {
            this.authenticated = true;
            LocalStorage.setAuthorization(authenticationResponse.sessionId, authenticationResponse.jwtToken);
            LocalStorage.setEmailAddress(authenticationResponse.emailAddress);

            const _client = new ApiClient(); // needs to be authenticated

            var accountIsActive = (await _client.Settings.Account.checkIsActive()).data as boolean;
            this.isActive = accountIsActive;
            LocalStorage.setIsActive(accountIsActive);

            callback();
            return true;
        } else {
            this.authenticated = false;
            LocalStorage.unsetAuthorization();
            errorCallback(authenticationResponse);
            return false;
        }
    }

    async login(email: string | null, password: string | null, callback: () => any, errorCallback: (response) => any) {
        if (email === null || password === null) return false;
        try {
            const authenticationResponse = (await this.loginClient.Login.RequestLogin(email, password)).data as Credentials;
            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error attempting to reach the server.")
        }
    }

    async loginWithGoogle(oneTimeCode: string, accessToken: string, tokenId: string, callback: () => void, errorCallback: (response) => void) {

        try {
            const authenticationResponse = (await this.loginClient.Login.RequestLoginWithGoogleToken(oneTimeCode, accessToken, tokenId)).data as Credentials;
            return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
        } catch {
            console.log("Error attempting to reach the server.")
        }
    }

    async loginFromMemory(callback: any) {
        const token = LocalStorage.getJwtToken();
        if (token) {
            var response = (await this.loginClient.Status.CheckIfLoggedIn()).data as boolean;
            if (response) {
                callback();
            }
        }
    }

    async logout(callback: () => any) {
        console.log("Attempting raw logout");
        const sessionId = LocalStorage.getSessionId();
        if (sessionId !== null && sessionId !== "") {
            const logoutClient = new LogoutClient(); // needs to be authenticated
            await logoutClient.Logout.RequestLogout(sessionId);
            LocalStorage.unsetAuthorization();
            LocalStorage.unsetEmailAddress();
        }
        this.authenticated = false;
        callback();
    }

    async googleLogout(callback: () => any) {
        window.gapi.load("auth2", () => {
            window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true });
            window.gapi.auth2.getAuthInstance().then((auth2) => {
                auth2.signOut().then(async () => {
                    auth2.disconnect().then(
                        await this.logout(callback)
                    )
                });
            });
        });

        // window.gapi.load("auth2", () => {
        //     window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true });
        //     window.gapi.auth2.getAuthInstance().then((auth2) => {
        //         auth2.signOut().then(() => {
        //             // auth2.disconnect().then(
        //                 error(response));
        //         // });
        //     });
        // });



    }

    isAuthenticated() {
        return this.authenticated;
    }
}

const AuthObject = new Auth();
export default AuthObject;
