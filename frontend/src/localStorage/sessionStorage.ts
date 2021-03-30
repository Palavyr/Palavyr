import { RememberMe } from "@Palavyr-Types";

class SessionStorageAccess {
    private rememberMeEmail: string = "rememberMeEmail";
    private rememberMePassword: string = "rememberMePassword";
    private AuthString: string = "authenticated";
    private SessionString: string = "sessionId";
    private JwtTokenString: string = "Authorization";
    private EmailString: string = "emailAddress";
    private googleImage: string = "googleImage";
    private loginType: string = "loginType";
    private active: string = "rkjh45lh435lkh";

    public GoogleLoginType: string = "google";
    public DefaultLoginType: string = "default";


    private _setItem(key: string, val: boolean | string) {
        const converted = val.toString();
        sessionStorage.setItem(key, converted);
    }

    private _getItem(key: string) {
        return sessionStorage.getItem(key);
    }

    private _isSet(val: string) {
        // only pass the predefined keys from above list
        const value = this._getItem(val);
        return !this._isNullOrUndefinedOrWhiteSpace(val);
    }

    private _isNullOrUndefinedOrWhiteSpace(val: any): boolean {
        return val === null || val === undefined || val === "";
    }

    public setRememberMe(emailAddress: string, password: string) {
        this._setItem(this.rememberMeEmail, emailAddress);
        this._setItem(this.rememberMePassword, password);
    }

    public getRememberMe(): RememberMe | null {
        const emailAddress = this._getItem(this.rememberMeEmail);
        const password = this._getItem(this.rememberMePassword);

        if (this._isNullOrUndefinedOrWhiteSpace(emailAddress)) {
            this.unsetRememberMe();
            return null;
        }
        if (this._isNullOrUndefinedOrWhiteSpace(password)) {
            this.unsetRememberMe();
            return null;
        }

        if (emailAddress === null || password === null) {
            // ... type narrowing.
            return null;
        }

        return { emailAddress: emailAddress, password: password };
    }

    public checkIsRemembered(): boolean {
        return this._isSet(this.rememberMeEmail) && this._isSet(this.rememberMePassword);
    }

    public unsetRememberMe(): void {
        this._setItem(this.rememberMePassword, "");
        this._setItem(this.rememberMeEmail, "");
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
        this._setItem(this.SessionString, sessionId);
        this._setItem(this.JwtTokenString, jwtToken);
    }

    setGoogleImage(imageUrl: string) {
        this._setItem(this.googleImage, imageUrl);
    }

    setGoogleLoginType() {
        this._setItem(this.loginType, this.GoogleLoginType);
    }
    setDefaultLoginType() {
        this._setItem(this.loginType, this.DefaultLoginType);
    }

    setIsActive(val: boolean) {
        this._setItem(this.active, val);
    }

    isActive() {
        var active = this._getItem(this.active);
        return active === "true" ? true : false;
    }

    isAuthenticated() {
        var auth = this._getItem(this.AuthString);
        return auth === "true" ? true : false;
    }

    unsetAuthorization() {
        this._setItem(this.EmailString, "");
        this._setItem(this.AuthString, false);
        this._setItem(this.SessionString, "");
        this._setItem(this.JwtTokenString, "");
        this._setItem(this.loginType, "");
        this._setItem(this.googleImage, "");
        this._setItem(this.active, false);
    }

    getLoginType() {
        return this._getItem(this.loginType);
    }

    getSessionId() {
        return this._getItem(this.SessionString);
    }

    getJwtToken() {
        return this._getItem(this.JwtTokenString);
    }

    getGoogleImage() {
        return this._getItem(this.googleImage);
    }
}

const SessionStorage = new SessionStorageAccess();
export { SessionStorage };
