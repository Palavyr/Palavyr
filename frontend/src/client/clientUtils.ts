import { SessionStorage } from "localStorage/sessionStorage";

/*
This will retrieve login credental data from localsession and send it with the requestover to the server for retrieval.
*/
export const getSessionIdFromLocalStorage = (): string => {
    var sessionId = SessionStorage.getSessionId()
    return sessionId || "noIdInStorage";
}

export const getJwtTokenFromLocalStorage = (): string => {
    var token = SessionStorage.getJwtToken();
    if (!token){
        throw new Error("No token in local storage...")
    }
    return token || "noTokenInStorage";
}

export const serverUrl = process.env.API_URL as string;
export const webUrl = process.env.WEB_URL as string;
export const widgetUrl = process.env.WIDGET_URL as string;
export const widgetApiKey = process.env.WIDGET_APIKEY as string;
export const googleOAuthClientId = process.env.GOOGLE_OAUTH as string;
export const stripeKey = process.env.STRIPE_KEY as string;
export const currentEnvironment = process.env.CURRENTENV as string;
export const softwareVersion = process.env.VERSION as string;

export enum Environments {
    Development,
    Staging,
    Production
}

export const isDevelopmentStage = () => {
    return currentEnvironment !== typeof(Environments.Production);
}

if (softwareVersion === undefined) {
    console.log("SOFTWARE VERSION IS UNDEFINED");
}

if (serverUrl === undefined) {
    console.log("SERVER URL UNDEFINED")
}

if (webUrl === undefined) {
    console.log("WEB URL UNDEFINED")
}

if (widgetUrl === undefined) {
    console.log("WIDGET URL UNDEFINED")
}
if (widgetApiKey === undefined) {
    console.log("WIDGET API KEY UNDEFINED")
}

if (googleOAuthClientId === undefined) {
    console.log("GOOGLE OAUTH CLIENT ID UNDEFINED")
}

if (stripeKey === undefined) {
    console.log("STRIPE KEY UNDEFINED")
}

export const SPECIAL_HEADERS = {}