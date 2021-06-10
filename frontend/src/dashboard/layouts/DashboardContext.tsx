import { Conversation, NodeSetterWithHistory, NodeTypeOptions, PlanType, PlanTypeMeta, SetState, SnackbarPositions } from "@Palavyr-Types";
import { PalavyrLinkedList } from "dashboard/content/responseConfiguration/conversation/convoDataStructure/PalavyrLinkedList";
import { ConversationHistoryTracker } from "dashboard/content/responseConfiguration/conversation/nodes/ConversationHistoryTracker";
import React from "react";

interface IDashboardContext {
    accountTypeNeedsPassword: boolean;
    checkAreaCount(): void;
    areaName: string;
    setViewName: SetState<string>;
    currencySymbol: string;
    setIsLoading: SetState<boolean>;
    successText: string;
    successOpen: boolean;
    setSuccessOpen: SetState<boolean>;
    setSuccessText: SetState<string>;
    warningText: string;
    warningOpen: boolean;
    setWarningOpen: SetState<boolean>;
    setWarningText: SetState<string>;
    errorText: string;
    errorOpen: boolean;
    setErrorOpen: SetState<boolean>;
    setErrorText: SetState<string>;
    setSnackPosition: SetState<SnackbarPositions>;
    snackPosition: SnackbarPositions;
    unseenNotifications: number;
    setUnseenNotifications: SetState<number>;
    planTypeMeta: PlanTypeMeta | undefined;
}

interface IAuthContext {
    isActive: boolean;
    isAuthenticated: boolean;
}

interface IConversationTreeContext {
    nodeList: Conversation;
    setNodes: NodeSetterWithHistory;
    historyTracker: ConversationHistoryTracker;
    conversationHistory: Conversation[];
    conversationHistoryPosition: number;
    nodeTypeOptions: NodeTypeOptions;
    showDebugData: boolean;
    palavyrLinkedList?: PalavyrLinkedList;
}

export const AuthContext = React.createContext({ isActive: false, isAuthenticated: false } as IAuthContext);
export const DashboardContext = React.createContext({ unseenNotifications: 0, successOpen: false, warningOpen: false, errorOpen: false, successText: "Success", warningText: "Warning", errorText: "Error" } as IDashboardContext);
export const ConversationTreeContext = React.createContext({} as IConversationTreeContext);
