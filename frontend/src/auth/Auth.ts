import { LocalStorage } from "localstorage/LocalStorage";
import { LoginClient } from "client/LoginClient";
import { LogoutClient } from "client/LogoutClient";


type Credentials = {
    apiKey: string;
    sessionId: string;
    authenticated: boolean;
    message: string;
}

class Auth {

    private authenticated: boolean = false;
    private sessionId: string;

    constructor() {
        this.authenticated = LocalStorage.isAuthenticated() === true ? true : false;
    }

    async register(email: string, password: string, callback: () => any, errorCallback: (response) => any) {
        const loginClient = new LoginClient();
        const res = await loginClient.Account.registerNewAccount(email, password);
        const result = await this.login(email, password, callback, errorCallback)
        if (result) LocalStorage.setEmailAddress(email);
        return result;
    }

    async memoryLogin() {
        return false; // TODO implement memory login so we don't have to keep typing our creds.
    }


    async login(email: string | null, password: string | null, callback: () => any, errorCallback: (response) => any) {
        const loginClient = new LoginClient();

        if (email === null || password === null) return false;
        const res = await loginClient.Login.RequestLogin(email, email, password)
        const authenticationResponse = res.data as Credentials;
        if (authenticationResponse.authenticated) {
            this.authenticated = true;
            LocalStorage.setAuthorization(authenticationResponse.sessionId);
            LocalStorage.setEmailAddress(email);
            callback()
            return true;
        } else {
            errorCallback(authenticationResponse)
            return false;
        }
    }

    async logout(callback: () => any) {
        const logoutClient = new LogoutClient();

        const sessionId = LocalStorage.getSessionId();
        console.log("Session ID at logout: " + sessionId)
        if (sessionId !== null) {
            await logoutClient.Logout.RequestLogout();
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
