import { CacheIds } from "@api-client/FrontendAxiosClient";
import { Font } from "@common/fonts/fontManager/types";
import { DeSerializedImageMeta, RememberMe } from "@Palavyr-Types";

class SessionStorageAccess {
    private rememberMeEmail: string = "rememberMeEmail";
    private rememberMePassword: string = "rememberMePassword";
    private AuthString: string = "authenticated";
    private SessionString: string = "sessionId";
    private JwtTokenString: string = "Authorization";
    private EmailString: string = "emailAddress";
    private loginType: string = "loginType";
    private active: string = "rkjh45lh435lkh";
    public DefaultLoginType: string = "default";

    private _setItem(key: string, val: boolean | string) {
        const converted = val.toString();
        sessionStorage.setItem(key, converted);
    }

    private _getItem(key: string) {
        return sessionStorage.getItem(key);
    }

    private ClearAll() {
        sessionStorage.clear();
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
        this._setItem(this.active, false);
    }

    unsetOtherSessionItems(){
        this._setItem(CacheIds.Areas, "");
        this._setItem(CacheIds.CurrentPlanMeta, "")
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

    setImageData(imageId: string, presignedUrl: string, fileName: string, fileId: string) {
        this._setItem(
            imageId,
            JSON.stringify({
                presignedUrl,
                fileName,
                fileId,
            })
        );
    }

    setFonts(fonts: Font[]) {
        this._setItem("palavyr-fonts", JSON.stringify(fonts));
    }

    getFonts(): Font[] | null {
        const serialized = this._getItem("palavyr-fonts");
        if (serialized) {
            return JSON.parse(serialized);
        }
        return null;
    }

    clearFonts() {
        this._setItem("palavyr-fonts", "");
    }

    getImageData(imageId: string): DeSerializedImageMeta | null {
        const serialized = this._getItem(imageId);
        if (serialized) {
            return JSON.parse(serialized) as DeSerializedImageMeta;
        }
        return null;
    }

    setCacheValue(key: string, value: any) {
        this._setItem(key, JSON.stringify(value));
    }

    getCacheValue(key: string) {
        return this.getStoredJson(key);
    }
    clearCacheValue(key: string) {
        this._setItem(key, "");
    }

    ClearAllCacheValues() {
        this.ClearAll();
    }

    private getStoredJson(key: string) {
        const value = this._getItem(key);
        if (value) {
            return JSON.parse(value);
        }
        return null;
    }
}

const SessionStorage = new SessionStorageAccess();
export { SessionStorage };
