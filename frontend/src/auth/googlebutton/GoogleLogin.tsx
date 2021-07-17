import { googleConsentScreenUrl, googleOAuthClientId, webUrl } from "@api-client/clientUtils";
import React from "react";
import { makeStyles } from "@material-ui/core";
import { AnyFunction } from "@Palavyr-Types";
import { useEffect } from "react";
import { useHistory } from "react-router-dom";
import { DASHBOARD_HOME } from "@constants";

const useStyles = makeStyles((theme) => ({
    loginButton: {
        // fontSize: "18pt"
    },
}));

interface IGoogleLogin {
    onSuccess: AnyFunction;
    onFailure: AnyFunction;
}

interface Gapi {
    load: any;
    auth2: any;
    signin2: any;
}

type CurrentWindow = Window &
    typeof globalThis & {
        gapi: Gapi;
    };

export const GoogleLogin = ({ onSuccess, onFailure }: IGoogleLogin) => {
    const classes = useStyles();

    const initializeGoogleSignin = () => {
        window.gapi.load("auth2", () => {
            window.gapi.auth2.init({ client_id: googleOAuthClientId, fetch_basic_profile: true, ux_mode: "redirect", redirect_uri: webUrl }).then(() => {
                const params = {
                    onsuccess: onSuccess,
                    onfailure: onFailure,
                    width: 240,
                    height: 50,
                    longtitle: true,
                    theme: "dark",
                };
                window.gapi.signin2.render("googleLoginButton", params);
            });
        });
    };

    const insertGapiScript = () => {
        const script = document.createElement("script");
        script.src = "https://apis.google.com/js/platform.js";
        script.onload = () => {
            initializeGoogleSignin();
        };
        document.body.appendChild(script);
    };

    useEffect(() => {
        insertGapiScript();
    }, []);
    const history = useHistory();
    return (
        <div style={{ display: "flex", justifyContent: "center", width: "100%" }}>
            <div id="googleLoginButton" className={classes.loginButton}></div>
        </div>
    );
};
