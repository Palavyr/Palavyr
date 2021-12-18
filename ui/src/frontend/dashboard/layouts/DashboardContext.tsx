import { IAuthContext, IConversationTreeContext, IDashboardContext } from "@Palavyr-Types";
import React from "react";

export const AuthContext = React.createContext<IAuthContext>({ isActive: false, isAuthenticated: false });
export const DashboardContext = React.createContext({
    unseenNotifications: 0,
    successOpen: false,
    warningOpen: false,
    errorOpen: false,
    successText: "Success",
    warningText: "Warning",
    errorText: "Error",
} as IDashboardContext);

export const ConversationTreeContext = React.createContext({} as IConversationTreeContext);
