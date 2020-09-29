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

// Enable for debugging if necessary 
// console.log("Process.Env Variables gathered for the current env...")
// console.log("WIDGET_URL: " + widgetUrl);
// console.log("SERVER_URL: " + serverUrl);
// console.log("WEB_URL: " + webUrl);

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