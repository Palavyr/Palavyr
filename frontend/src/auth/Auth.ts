import { LocalStorage } from "localStorage/localStorage";
import { LoginClient } from "client/LoginClient";
import { LogoutClient } from "client/LogoutClient";
import { callbackify } from "util";


type Credentials = {
    jwtToken: string;
    apiKey: string;
    sessionId: string;
    emailAddress: string;
    authenticated: boolean;
    message: string;
}

class Auth {

    private authenticated: boolean = false;
    private loginClient = new LoginClient();
    private logoutClient = new LogoutClient();

    constructor() {
        this.authenticated = LocalStorage.isAuthenticated() === true ? true : false;
    }

    async register(email: string, password: string, callback: () => any, errorCallback: (response) => any) {
        const authenticationResponse = (await this.loginClient.Account.registerNewAccount(email, password)).data as Credentials; // TODO: Check that res is successfull before logging in
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
    }

    async registerWithGoogle(oneTimeCode: string, accessToken: string, tokenId: string, callback: () => void, errorCallback: (response) => void) {
        const authenticationResponse = (await this.loginClient.Account.registerNewAccountWithGoogle(oneTimeCode, accessToken, tokenId)).data as Credentials;
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);    }

    private processAuthenticationResponse(authenticationResponse: Credentials, callback: () => any, errorCallback: (response) => any): boolean {
        if (authenticationResponse.authenticated) {
            this.authenticated = true;
            LocalStorage.setAuthorization(authenticationResponse.sessionId, authenticationResponse.jwtToken);
            LocalStorage.setEmailAddress(authenticationResponse.emailAddress);
            callback()
            return true;
        } else {
            errorCallback(authenticationResponse)
            return false;
        }
    }

    async login(email: string | null, password: string | null, callback: () => any, errorCallback: (response) => any) {

        if (email === null || password === null) return false;
        const authenticationResponse = (await this.loginClient.Login.RequestLogin(email, password)).data as Credentials;
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
    }

    async loginWithGoogle(oneTimeCode: string, accessToken: string, tokenId: string, callback: () => void, errorCallback: (response) => void) {
        const authenticationResponse = (await this.loginClient.Login.RequestLoginWithGoogleToken(oneTimeCode, accessToken, tokenId)).data as Credentials;
        return this.processAuthenticationResponse(authenticationResponse, callback, errorCallback);
    }


    async loginFromMemory (callback: any) {
        const token = LocalStorage.getJwtToken();
        if (token) {
            var response = (await this.loginClient.Status.CheckIfLoggedIn()).data as boolean;
            if (response) {
                callback();
            }
        }
    }

    async logout(callback: () => any) {

        const sessionId = LocalStorage.getSessionId();
        console.log("Session ID at logout: " + sessionId)
        if (sessionId !== null && sessionId !== "") {
            await this.logoutClient.Logout.RequestLogout(sessionId);
            LocalStorage.unsetAuthorization();
            LocalStorage.unsetEmailAddress();
        }
        this.authenticated = false;
        callback();
    }

    isAuthenticated() {
        return this.authenticated;
    }
}

export default new Auth();
