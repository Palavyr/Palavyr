import { WidgetClient } from "client/Client";
import { ElementType } from "react";

export type SecretKey = string | null;

export type AreaTable = {
    areaIdentifier: string;
    areaDisplayTitle: string;
};

export type WidgetNodeResource = {
    areaIdentifier: string;
    nodeId: string;
    text: string;
    nodeType: string;
    nodeChildrenString: string;
    isRoot: boolean;
    isCritical: boolean;
    optionPath: string | null;
    valueOptions: string; // needs to be split by ","
    nodeComponentType: string;
    isDynamicTableNode: boolean;
    dynamicType: string | null;
    resolveOrder: number | null;
}

export type WidgetNodes = WidgetNodeResource[];

export type SelectedOption = {
    areaDisplay: string;
    areaId: string;
};

export type Registry = {
    [key: string]: any;
};

export type NewConversation = {
    conversationId: string;
    widgetPreferences: WidgetPreferences;
    conversationNodes: WidgetNodes;
};

export type WidgetPreferences = {
    // title: string;
    // subtitle: string;
    landingHeader: string;
    chatHeader: string;
    placeholder: string;
    selectListColor: string;
    headerColor: string;
    fontFamily: string;
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
    UserResponse: string | null;
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
    localeMap: LocaleMap;
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
    node: WidgetNodeResource;
    nodeList: WidgetNodes;
    client: WidgetClient;
    convoId: string;
}

export type Nullable<T> = T | null;

export type AnyFunction = (...args: any[]) => any;

type BaseMessage = {
    type: string;
    component: ElementType;
    sender: string;
    showAvatar: boolean;
    timestamp: Date;
    unread: boolean;
    customId?: string;
    props?: any;
};

export type DynamicResponses = DynamicResponse[];
export type DynamicResponse = {
    [UniqueTableKey: string]: Response[];
};
export type Response = {
    [nodeId: string]: string; // node.nodeId: response value;
};

export type KeyValue = {
    [key: string]: string; // MUST BE STRING for server
};

export type KeyValues = Array<KeyValue>;

export type ContextProperties = {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
};

export interface IMessage extends BaseMessage {
    text: string;
}

export interface Link extends BaseMessage {
    title: string;
    link: string;
    target: string;
}

export interface LinkParams {
    link: string;
    title: string;
    target?: string;
}

export interface CustomCompMessage extends BaseMessage {
    props: any;
}

export interface BehaviorState {
    showChat: boolean;
    disabledInput: boolean;
    messageLoader: boolean;
    userDetailsVisible: boolean;
}

export interface ContextState {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
}

export interface MessagesState {
    messages: (IMessage | Link | CustomCompMessage)[];
    badgeCount: number;
}

export interface ImageState {
    src: string;
    alt?: string;
    width: number;
    height: number;
}

export interface FullscreenPreviewState extends ImageState {
    visible?: boolean;
}

export interface GlobalState {
    messagesReducer: MessagesState;
    behaviorReducer: BehaviorState;
    previewReducer: FullscreenPreviewState;
    contextReducer: ContextProperties;
}

export const OPEN_USER_DETAILS = "BEHAVIOR/OPEN_USER_DETAILS";
export const CLOSE_USER_DETAILS = "BEHAVIOR/CLOSE_USER_DETAILS";
export const TOGGLE_USER_DETAILS = "BEHAVIOR/TOGGLE_USER_DETAILS";
export const TOGGLE_CHAT = "BEHAVIOR/TOGGLE_CHAT";
export const TOGGLE_INPUT_DISABLED = "BEHAVIOR/TOGGLE_INPUT_DISABLED";
export const DISABLE_INPUT = "BEHAVIOR/INPUT_DISABLED";
export const ENABLE_INPUT = "BEHAVIOR/INPUT_ENABLED";
export const TOGGLE_MESSAGE_LOADER = "BEHAVIOR/TOGGLE_MSG_LOADER";
export const SET_BADGE_COUNT = "BEHAVIOR/SET_BADGE_COUNT";
export const ADD_NEW_USER_MESSAGE = "MESSAGES/ADD_NEW_USER_MESSAGE";
export const ADD_NEW_RESPONSE_MESSAGE = "MESSAGES/ADD_NEW_RESPONSE_MESSAGE";
export const ADD_NEW_LINK_SNIPPET = "MESSAGES/ADD_NEW_LINK_SNIPPET";
export const ADD_COMPONENT_MESSAGE = "MESSAGES/ADD_COMPONENT_MESSAGE";
export const DROP_MESSAGES = "MESSAGES/DROP_MESSAGES";
export const HIDE_AVATAR = "MESSAGES/HIDE_AVATAR";
export const DELETE_MESSAGES = "MESSAGES/DELETE_MESSAGES";
export const MARK_ALL_READ = "MESSAGES/MARK_ALL_READ";
export const OPEN_FULLSCREEN_PREVIEW = "FULLSCREEN/OPEN_PREVIEW";
export const CLOSE_FULLSCREEN_PREVIEW = "FULLSCREEN/CLOSE_PREVIEW";
export const SET_CONTEXT_PROPERTIES = "CONTEXT_PROPERTIES/SET";
export const GET_CONTEXT_PROPERTIES = "CONTEXT_PROPERTIES/GET";

export const SET_NUM_INDIVIDUALS_CONTEXT = "CONTEXT_PROPERTIES/SET_NUM_INDIVIDUALS";
export const SET_NAME_CONTEXT = "CONTEXT_PROPERTIES/SET_NAME";
export const SET_PHONE_CONTEXT = "CONTEXT_PROPERTIES/SET_PHONE";
export const SET_EMAILADDRESS_CONTEXT = "CONTEXT_PROPERTIES/SET_EMAILADDRESS";
export const SET_REGION_CONTEXT = "CONTEXT_PROPERTIES/SET_REGION";
export const SET_KEYVALUE_CONTEXT = "CONTEXT_PROPERTIES/SET_KEYVALUE";
export const SET_DYNAMICRESPONSE_CONTEXT = "CONTEXT_PROPERTIES/SET_DYNAMICRESPONSE";
export const SET_DYNAMICRESPONSES_CONTEXT = "CONTEXT_PROPERTIES/SET_DYNAMICRESPONSES";

export interface OpenUserDetails {
    type: typeof OPEN_USER_DETAILS;
}
export interface CloseUserDetails {
    type: typeof CLOSE_USER_DETAILS;
}

export interface ToggleUserDetails {
    type: typeof TOGGLE_USER_DETAILS;
}

export interface ToggleChat {
    type: typeof TOGGLE_CHAT;
}

export interface ToggleInputDisabled {
    type: typeof TOGGLE_INPUT_DISABLED;
}

export interface InputDisabled {
    type: typeof DISABLE_INPUT;
}

export interface InputEnabled {
    type: typeof ENABLE_INPUT;
}

export interface AddUserMessage {
    type: typeof ADD_NEW_USER_MESSAGE;
    text: string;
    id?: string;
}

export interface AddResponseMessage {
    type: typeof ADD_NEW_RESPONSE_MESSAGE;
    text: string;
    id?: string;
}

export interface ToggleMsgLoader {
    type: typeof TOGGLE_MESSAGE_LOADER;
}

export interface AddLinkSnippet {
    type: typeof ADD_NEW_LINK_SNIPPET;
    link: LinkParams;
    id?: string;
}

export interface RenderCustomComponent {
    type: typeof ADD_COMPONENT_MESSAGE;
    component: ElementType;
    props: any;
    showAvatar: boolean;
    id?: string;
}

export interface DropMessages {
    type: typeof DROP_MESSAGES;
}

export interface HideAvatar {
    type: typeof HIDE_AVATAR;
    index: number;
}

export interface DeleteMessages {
    type: typeof DELETE_MESSAGES;
    count: number;
    id?: string;
}

export interface SetBadgeCount {
    type: typeof SET_BADGE_COUNT;
    count: number;
}

export interface MarkAllMessagesRead {
    type: typeof MARK_ALL_READ;
}

export type BehaviorActions = OpenUserDetails | CloseUserDetails | ToggleUserDetails | ToggleChat | ToggleInputDisabled | ToggleMsgLoader | ToggleInputDisabled | InputEnabled | InputDisabled;

export type MessagesActions = AddUserMessage | AddResponseMessage | AddLinkSnippet | RenderCustomComponent | DropMessages | HideAvatar | DeleteMessages | MarkAllMessagesRead | SetBadgeCount;

export interface openFullscreenPreview {
    type: typeof OPEN_FULLSCREEN_PREVIEW;
    payload: FullscreenPreviewState;
}

export interface closeFullscreenPreview {
    type: typeof CLOSE_FULLSCREEN_PREVIEW;
}

export type FullscreenPreviewActions = openFullscreenPreview | closeFullscreenPreview;

export interface setContextProperties {
    type: typeof SET_CONTEXT_PROPERTIES;
    contextProperties: ContextProperties;
}

export interface getContextProperties {
    type: typeof GET_CONTEXT_PROPERTIES;
}

export interface setNameContext {
    type: typeof SET_NAME_CONTEXT;
    name: string;
}

export interface setPhoneContext {
    type: typeof SET_PHONE_CONTEXT;
    phoneNumber: string;
}

export interface setEmailAddressContext {
    type: typeof SET_EMAILADDRESS_CONTEXT;
    emailAddress: string;
}

export interface setRegionContext {
    type: typeof SET_REGION_CONTEXT;
    region: string;
}

export interface addKeyValueContext {
    type: typeof SET_KEYVALUE_CONTEXT;
    keyValue: KeyValue;
}

export interface setDynamiceResponses {
    type: typeof SET_DYNAMICRESPONSES_CONTEXT;
    dynamicResponseObject: DynamicResponses;
}

export interface addNumIndividuals {
    type: typeof SET_NUM_INDIVIDUALS_CONTEXT;
    numIndividuals: number;
}

export type ContextPropertyActions =
    | setContextProperties
    | getContextProperties
    | setNameContext
    | setPhoneContext
    | setEmailAddressContext
    | setRegionContext
    | addKeyValueContext
    | setDynamiceResponses;

export type AllActions = ContextPropertyActions | FullscreenPreviewActions | BehaviorActions | MessagesActions;
