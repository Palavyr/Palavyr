import { WidgetPreferences } from "@Palavyr-Types";
import React from "react";

export interface IWidgetContext {
    preferences: WidgetPreferences;
}

export const WidgetContext = React.createContext({} as IWidgetContext);
