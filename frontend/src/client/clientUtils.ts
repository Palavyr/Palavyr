import { LocalStorage } from "localStorage/localStorage";

/*
This will retrieve login credental data from localstorage and send it with the requestover to the server for retrieval.
*/
export const getSessionIdFromLocalStorage = (): string => {
    var sessionId = LocalStorage.getSessionId()
    return sessionId || "noIdInStorage";
}

export const serverUrl = process.env.API_URL as string;
export const webUrl = process.env.WEB_URL as string;
export const widgetUrl = process.env.WIDGET_URL as string;

console.log(widgetUrl);

if (serverUrl === undefined) {
    console.log("SERVER URL UNDEFINED")
}
if (webUrl === undefined) {
    console.log("WEB URL UNDEFINED")
}

if (widgetUrl === undefined){

    console.log("WIDGET URL UNDEFINED")
}

export const SPECIAL_HEADERS = {}