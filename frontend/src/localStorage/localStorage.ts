
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

    setGoogleImage(imageUrl: string) {
        this._setItem(this.googleImage, imageUrl);
    }

    setGoogleLoginType(){
        this._setItem(this.loginType, this.GoogleLoginType);
    }
    setDefaultLoginType() {
        this._setItem(this.loginType, this.DefaultLoginType);
    }

    setIsActive(val: boolean) {
        this._setItem(this.active, val)
    }

    isActive() {
        var active = this._getItem(this.active);
        return active === "true" ? true : false;
    }

    isAuthenticated() {
        var auth = this._getItem(this.AuthString)
        return auth === "true" ? true : false;
    }

    unsetAuthorization() {
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

const LocalStorage = new LocalStorageAccess()
export { LocalStorage }