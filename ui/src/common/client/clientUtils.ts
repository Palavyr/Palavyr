import { History } from "history";
import { SessionStorage } from "@localStorage/sessionStorage";
import { PalavyrRepository } from "./PalavyrRepository";

/*
    This will retrieve login credental data from localsession and send it with the requestover to the server for retrieval.
*/
export const getSessionIdFromLocalStorage = (): string => {
    const sessionId = SessionStorage.getSessionId();
    return sessionId || "noIdInStorage";
};

export const getJwtTokenFromLocalStorage = (): string => {
    const token = SessionStorage.getJwtToken();
    return token || "noTokenInStorage";
};

export const redirectToHomeWhenSessionNotEstablished = async (history: History<unknown> | string[], repository: PalavyrRepository) => {
    const jwt_token = SessionStorage.getJwtToken();
    if (!jwt_token) {
        history.push("/");
    }
    const signedIn = await repository.AuthenticationCheck.check();
    if (!signedIn) {
        history.push("/");
    }
};

export const webUrl = process.env.WEB_URL as string;
export const serverUrl = process.env.API_URL as string;
export const widgetUrl = process.env.WIDGET_URL as string;
export const stripeKey = process.env.STRIPE_KEY as string;
export const currentEnvironment = process.env.CURRENTENV as string;
export const softwareVersion = process.env.VERSION as string;
export const landingWidgetApiKey = process.env.LANDING_WIDGET_APIKEY as string;
export const googleYoutubeApikey = process.env.GOOGLE_YOUTUBE_KEY as string;
export const googleAnalyticsTrackingId = process.env.GOOGLE_ANALYTICS_KEY as string;
export const googleWidgetAnalyticsTrackingId = process.env.GOOGLE_ANALYTICS_WIDGET_KEY as string;

export enum Environments {
    Development,
    Staging,
    Production,
}

export const isDevelopmentStage = () => {
    return currentEnvironment.toUpperCase() !== "Production".toUpperCase();
};

export const SPECIAL_HEADERS = {};
