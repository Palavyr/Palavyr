import { Conversation, ConvoNode, PlanType } from "@Palavyr-Types";
import React, { Dispatch, SetStateAction } from "react";

interface IDashboardContext {
    numAreasAllowed: number;
    checkAreaCount(): void;
    areaName: string;
    setViewName: Dispatch<SetStateAction<string>>;
    subscription: PlanType | undefined;
    currencySymbol: string;
    setIsLoading: Dispatch<SetStateAction<boolean>>;
}

interface IAuthContext {
    isActive: boolean;
    isAuthenticated: boolean;
}

interface IConversationTreeContext {
    nodeList: Conversation;
    setNodes: (value: React.SetStateAction<Conversation>) => void;
    // conversationHistory: Conversation[];
    // setConversationHistory(newConversation: Conversation | ConvoNode[]): void;
}


// interface IConversationHistoryContext {

export const AuthContext = React.createContext({isActive: false, isAuthenticated: false} as IAuthContext);
export const DashboardContext = React.createContext({} as IDashboardContext);
export const ConversationTreeContext = React.createContext({} as IConversationTreeContext);
