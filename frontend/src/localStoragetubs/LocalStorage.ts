
class LocalStorageAccess {

    private AuthString: string = "authenticated";
    private SessionString: string = "sessionId";
    private EmailString: string = "emailAddress";

    private _setItem(key: string, val: boolean | string) {
        const converted = val.toString();
        localStorage.setItem(key, converted);
    }

    private _getItem(key: string) {
        return localStorage.getItem(key);
    }

    setEmailAddress(emailAddress: string) {
        this._setItem(this.EmailString, emailAddress);
    }

    getEmailAddress() {
        return this._getItem(this.EmailString);
    }

    unsetEmailAddress() {
        this._setItem(this.EmailString, "");
    }

    setAuthorization(sessionId: string) {
        this._setItem(this.AuthString, true);
        this._setItem(this.SessionString, sessionId)
    }

    getSessionId() {
        return this._getItem(this.SessionString);
    }

    isAuthenticated() {
        var auth = this._getItem(this.AuthString)
        return auth === "true" ? true : false;
    }

    unsetAuthorization() {
        this._setItem(this.AuthString, false);
        this._setItem(this.SessionString, "");
    }
}

const LocalStorage = new LocalStorageAccess()
export { LocalStorage }