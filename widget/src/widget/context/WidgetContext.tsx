import { SetState, WidgetPreferences } from "@Palavyr-Types";
import React from "react";

export interface IWidgetContext {
    preferences: WidgetPreferences;
    chatStarted: boolean;
    setChatStarted: SetState<boolean>;
    convoId: string | null;
    setConvoId: SetState<string>;
}

export const WidgetContext = React.createContext({} as IWidgetContext);
