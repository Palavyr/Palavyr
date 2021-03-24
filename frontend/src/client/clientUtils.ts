import { LocalStorage } from "localStorage/localStorage";

/*
This will retrieve login credental data from localstorage and send it with the requestover to the server for retrieval.
*/
export const getSessionIdFromLocalStorage = (): string => {
    var sessionId = LocalStorage.getSessionId()
    return sessionId || "noIdInStorage";
}

export const getJwtTokenFromLocalStorage = (): string => {
    var token = LocalStorage.getJwtToken();
    return token || "noTokenInStorage";
}

export const serverUrl = process.env.API_URL as string;
export const webUrl = process.env.WEB_URL as string;
export const widgetUrl = process.env.WIDGET_URL as string;
export const widgetApiKey = process.env.WIDGET_APIKEY as string;
export const googleOAuthClientId = process.env.GOOGLE_OAUTH as string;
export const stripeKey = process.env.STRIPE_KEY as string;
export const currentEnvironment = process.env.CURRENTENV as string;

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