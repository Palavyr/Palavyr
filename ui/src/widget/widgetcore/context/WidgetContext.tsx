import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { SetState } from "@Palavyr-Types";
import React from "react";
import { IAppContext } from "widget/hook";

export interface IWidgetContext {
    context: IAppContext;
    preferences: WidgetPreferencesResource;
    convoId: string | null;
    setConvoId: SetState<string>;
    isDemo: boolean;
}

export const WidgetContext = React.createContext({} as IWidgetContext);
