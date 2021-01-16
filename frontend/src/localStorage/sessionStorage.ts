import { RememberMe } from "@Palavyr-Types";

class SessionStorageAccess {
    private rememberMeEmail: string = "rememberMeEmail";
    private rememberMePassword: string = "rememberMePassword";

    private _setItem(key: string, val: boolean | string) {
        const converted = val.toString();
        sessionStorage.setItem(key, converted);
    }

    private _setLocalStorageItem(key: string, val: boolean | string) {
        const converted = val.toString();
        localStorage.setItem(key, converted);
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
        return (val === null || val === undefined || val === "")
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
}

const SessionStorage = new SessionStorageAccess();
export { SessionStorage };
