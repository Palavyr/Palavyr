import { SetState, WidgetPreferences } from "@Palavyr-Types";
import React from "react";
import { IAppContext } from "widget/hook";

export interface IWidgetContext {
    context: IAppContext;
    preferences: WidgetPreferences;
    chatStarted: boolean;
    setChatStarted: SetState<boolean>;
    convoId: string | null;
    setConvoId: SetState<string>;
}

export const WidgetContext = React.createContext({} as IWidgetContext);
