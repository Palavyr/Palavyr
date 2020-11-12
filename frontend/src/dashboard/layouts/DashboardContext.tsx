import React, { Dispatch, SetStateAction } from "react";

interface IDashboardContext {
    numAreasAllowed?: number;
    checkAreaCount(): void;
    areaName: string;
    setViewName: Dispatch<SetStateAction<string>>;
}

interface IAuthContext {
    isActive: boolean;
    isAuthenticated: boolean;
}

export const AuthContext = React.createContext({isActive: false, isAuthenticated: false} as IAuthContext);
export const DashboardContext = React.createContext({} as IDashboardContext);