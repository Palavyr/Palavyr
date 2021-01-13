import { RememberMe } from "@Palavyr-Types";

class SessionStorageAccess {
    private rememberMeEmail: string = "rememberMeEmail";
    private rememberMePassword: string = "rememberMePassword";

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
        if (this._isNullOrUndefined(val) || val === "") {
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

    public setRememberMe(emailAddress: string, password: string) {
        if (!this._isSet(emailAddress) || !this._isSet(password)) {
            this.unsetRememberMe();
            return;
        }
        this._setItem(this.rememberMeEmail, emailAddress);
        this._setItem(this.rememberMePassword, password);
    }

    public getRememberMe(): RememberMe | undefined {
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

    public checkIsRemembered(): boolean {
        if (!this._isSet(this.rememberMeEmail) || !this._isSet(this.rememberMePassword)) {
            return false;
        }
        return true;
    }
    public unsetRememberMe(): void {
        this._setItem(this.rememberMePassword, "");
        this._setItem(this.rememberMeEmail, "");
    }
}

const SessionStorage = new SessionStorageAccess();
export { SessionStorage };

