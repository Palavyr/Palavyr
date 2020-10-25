
class LocalStorageAccess {

    private AuthString: string = "authenticated";
    private SessionString: string = "sessionId";
    private JwtTokenString: string = "Authorization";
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

    setAuthorization(sessionId: string, jwtToken: string) {
        this._setItem(this.AuthString, true);
        this._setItem(this.SessionString, sessionId)
        this._setItem(this.JwtTokenString, jwtToken)
    }
    isAuthenticated() {
        var auth = this._getItem(this.AuthString)
        return auth === "true" ? true : false;
    }

    unsetAuthorization() {
        this._setItem(this.AuthString, false);
        this._setItem(this.SessionString, "");
        this._setItem(this.JwtTokenString, "");
    }

    getSessionId() {
        return this._getItem(this.SessionString);
    }

    getJwtToken() {
        return this._getItem(this.JwtTokenString);
    }
}

const LocalStorage = new LocalStorageAccess()
export { LocalStorage }