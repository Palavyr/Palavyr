import { googleAnalyticsTrackingId, googleWidgetAnalyticsTrackingId } from "@api-client/clientUtils";

export enum GTagMethod {
    Event = "event",
    Config = "config",
}

export enum GTagEvents {
    PageView = "page_view",
    WidgetSelection = "widget_selection",
}

export enum GTagLabels {}

export const AppPageView = (url: string) => {
    window.gtag(GTagMethod.Event, GTagEvents.PageView, {
        page_title: document.title,
        page_location: url,
        page_path: url,
        send_to: googleAnalyticsTrackingId,
    });
};

export const WidgetPageView = (url: string) => {
    window.gtag(GTagMethod.Event, GTagEvents.PageView, {
        page_title: document.title,
        page_location: url,
        page_path: url,
        send_to: googleWidgetAnalyticsTrackingId,
    });
};

export const widgetSelection = (apiKey: string, selection: string, intentId: string) => {
    window.gtag(GTagMethod.Event, GTagEvents.WidgetSelection, {
        api_key: apiKey,
        selection: selection,
        intent_id: intentId,
        send_to: googleWidgetAnalyticsTrackingId,
    });
};

// export const base = (label: string, eventLabel: string, category: string, params: any = {}, callback: (...args: any) => any = () => null) => {
//     window.gtag("event", label, {
//         event_category: category,
//         event_callback: callback,
//         event_label: eventLabel,
//         ...params,
//     });
// };
