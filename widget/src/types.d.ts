import { Dispatch, SetStateAction } from "react";

export type GroupRow = {
    id: number;
    groupId: string;
    parentId: string;
    groupName: string;
};

export type AreaTable = {
    areaIdentifier: string;
    areaDisplayTitle: string;
};

export type ConvoTableRow = {
    id: number;
    nodeId: string;
    nodeType: string;
    isCritical: boolean;
    fallback: boolean;
    text: string;
    nodeChildrenString: string;
    isRoot: boolean;
    areaIdentifier: string;
    optionPath: string | null;
    valueOptions: string; // needs to be split by ","
    isDynamicTableNode: boolean;
    nodeComponentType: string;
};

export type SelectedOption = {
    areaDisplay: string;
    areaId: string;
};

export type Registry = {
    [key: string]: any;
};

export type WidgetPreferences = {
    id: number;
    title: string;
    subtitle: string;
    placeholder: string;
    shouldGroup: boolean;
    selectListColor: string;
    headerColor: string;
    fontFamily: string;
    header: string;
    listFontColor: string;
    headerFontColor: string;
    optionsHeaderColor: string;
    optionsHeaderFontColor: string;
    chatFontColor: string;
    chatBubbleColor: string;
};

export type ConversationUpdate = {
    ConversationId: string;
    Prompt: string;
    UserResponse: string;
    NodeId: string;
    NodeCritical: boolean;
    NodeType: string;
};

export type CompleteConverationDetails = {
    ConversationId: string;
    AreaIdentifier: string;
    Name: string;
    Email: string;
    PhoneNumber: string;
};

export type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>;
};

export type SendEmailResultResponse = {
    nextNodeId: string;
    result: boolean;
};


export type LocaleDefinition = {
    localeId: string;
    localeCountry: string;
    supportedLocales: string[];
    localeMap: LocalMap;
    localeCurrencySymbol: string;
    localePhonePattern: string;
};

export type LocaleMapItem = {
    localeId: string;
    countryName: string;
    phonePattern: string;
    currencySymbol: string;
};
export type LocaleMap = LocaleMapItem[];


export interface IProgressTheChat {
    node: ConvoTableRow;
    nodeList: Array<ConvoTableRow>;
    client: IClient;
    convoId: string;
}