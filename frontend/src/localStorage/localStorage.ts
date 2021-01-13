import { RememberMe } from "@Palavyr-Types";

class LocalStorageAccess {
    private AuthString: string = "authenticated";
    private SessionString: string = "sessionId";
    private JwtTokenString: string = "Authorization";
    private EmailString: string = "emailAddress";
    private googleImage: string = "googleImage";
    private loginType: string = "loginType";
    private active: string = "rkjh45lh435lkh";
    public GoogleLoginType: string = "google";
    public DefaultLoginType: string = "default";

    private rememberMeEmail: string = "rememberMeEmail";
    private rememberMePassword: string = "rememberMePassword";

    private _setItem(key: string, val: boolean | string) {
        const converted = val.toString();
        localStorage.setItem(key, converted);
    }

    private _getItem(key: string) {
        return localStorage.getItem(key);
    }

    private _isSet(val: string){
        // only pass the predefined keys from above list
        const value = this._getItem(val);
        if (this._isNullOrUndefined(val) || val === ""){
            return false;
        }
        return true;
    }

    private _isNullOrUndefined(val: any): boolean {
        if (val === null || val === undefined) {
            return true;
        }
        return false;
    }

    // Session Storage style - TODO: use session storage (already implemented!)
    setRememberMe(emailAddress: string, password: string) {
        if (!this._isSet(emailAddress) || !this._isSet(password)) {
            this.unsetRememberMe();
            return;
        }
        this._setItem(this.rememberMeEmail, emailAddress);
        this._setItem(this.rememberMePassword, password);
    }

    getRememberMe(): RememberMe | undefined {
        const emailAddress = this._getItem(this.rememberMeEmail);
        const password = this._getItem(this.rememberMePassword);

        const unsetResponse = { emailAddress: "", password: "" };

        if (this._isNullOrUndefined(emailAddress)) {
            this.unsetRememberMe();
            return unsetResponse;
        }
        if (this._isNullOrUndefined(password)) {
            this.unsetRememberMe();
            return unsetResponse;
        }
        if (emailAddress === null || password === null) {
            // ... type narrowing.
            return;
        }

        return { emailAddress: emailAddress, password: password };
    }

    checkIsRemembered(): boolean {
        if (!this._isSet(this.rememberMeEmail) || !this._isSet(this.rememberMePassword)){
            return false;
        }
        return true;
    }
    unsetRememberMe(): void {
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

const LocalStorage = new LocalStorageAccess();
export { LocalStorage };
