import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { COULD_NOT_FIND_SERVER, GOOGLE_ACCOUNT_NOT_FOUND, INVALID_EMAIL, INVALID_GOOGLE_TOKEN, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, NOT_A_GOOGLE_ACCOUNT, VERIFICATION_EMAIL_SEND } from "@constants";
import { PalavyrLinkedList } from "frontend/dashboard/content/responseConfiguration/conversation/PalavyrDataStructure/PalavyrLinkedList";
import React, { Dispatch, ElementType, SetStateAction } from "react";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";

// / <reference types="node" />
// / <reference types="react" />
// / <reference types="react-dom" />

/*
The front end needs to send a request to an end point with a PARAM, :areaIdentifier, and this
will be sent to C# backend API which will use the accountID and areaIdentifier to retrieve from a particular endpoint

something like /endpoints/:accountId/:areaIdentifier/?authToken=23b23k5iuhi2u5b2kjb2k34uhn234ujn

Then the API will extract the accountID, the areaIdentifier and use it with the endpoint to call a DB controller
which will can nevermore (which maps the json in the db column to a class) to retrieve ONLY the data for the area
we are currently working on. This keeps the state object a little bit smaller.

So we need to first make a call when the initial page loads to retieve the area ids (and their names) in order to
render the area list on sidebar, and then also the first area in the list (a second get request).
*/

export type UUID = string;
export type AnyFunction = (...args: any[]) => any;
export type AnyVoidFunction = (...args: any[]) => void;

export type SetState<T> = Dispatch<SetStateAction<T>>;
export type TableGroup<T> = {
    [itemGroup: string]: T;
};

export type DeSerializedImageMeta = {
    presignedUrl: string;
    fileName: string;
    fileId: string;
};

export type LineStyles = "solid" | "-moz-initial" | "inherit" | "initial" | "revert" | "unset" | "dashed" | "dotted" | "double" | "groove" | "hidden" | "inset" | "none" | "outset" | "ridge" | undefined;

export type Anchor = "top" | "left" | "middle" | "center" | "bottom" | "right";
export type Selector = string;

export type Milliseconds = string;
export type ParsedAnchor = {
    x: number;
    y: number;
};

// Database
export type GroupRow = {
    id: number;
    groupId: string;
    parentId: string;
    groupName: string;
};
export type GroupTable = Array<GroupRow>;

export type AreaMeta = {
    areaIdentifier: string;
    groupId: string;
    areaName: string;
};

// Client
export type GroupNodeType = {
    text: string;
    optionPath: string;
    nodeId: string;
    parentId: string;
    nodeChildrenString: string;
    isRoot: boolean;
    id?: number;
    areaMeta: Array<AreaMeta>;
    groupId: string;
};

export type Groups = Array<GroupNodeType>;

export type ConvoTableRow = {
    nodeId: string;
    nodeType: string;
    fallback: boolean;
    text: string;
    nodeChildrenString: string;
    isCritical: boolean;
    isRoot: boolean;
    areaIdentifier: string;
    optionPath: ConvoBuilderResponse;
};

export type ConvoBuilderResponse = "Yes" | "No" | "Not Sure" | "Ok" | "Backstop" | "Yes / Not Sure" | "No / Not Sure" | "Continue" | null | any;
export type Responses = Array<ConvoBuilderResponse>;

export const ValueOptionDelimiter = "|peg|";

export type ConvoNode = {
    // these properties are written to the database
    id?: number | undefined;
    areaIdentifier: string;

    isRoot: boolean;
    nodeId: string;
    isTerminalType: boolean;
    isMultiOptionType: boolean;
    text: string;
    nodeChildrenString: string;
    isCritical: boolean;
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;

    isLoopbackAnchorType: boolean;

    nodeType: string;
    optionPath: ConvoBuilderResponse;
    valueOptions: string; // an array, but bc of the dtabase we store as a string delimited by |peg|
    shouldRenderChildren: boolean;
    shouldShowMultiOption: boolean;
    nodeComponentType: string;
    isDynamicTableNode: boolean;
    isImageNode: boolean;
    imageId: string | null;
    resolveOrder: number;
    dynamicType: string | null;
    nodeTypeCode: NodeTypeCode;
};

export type Conversation = Array<ConvoNode>;

export type Areas = Array<AreaTable>;

export type AreaTable = {
    // all of the data
    areaIdentifier: string;
    areaName: string;
    areaDisplayTitle: string;
    prologue: string;
    epilogue: string;
    emailTemplate: string; // an email template
    convo: Array<ConvoNode>;
    staticTables: StaticTableMetas;
    dynamicTableType: string;
    groupId: string;
    areaSpecificEmail: string;
    emailIsVerified: boolean;
    awaitingVerification: boolean;
    subject: string;
    isEnabled: boolean;

    useAreaFallbackEmail: boolean;
    fallbackSubject: string;
    fallbackEmailTemplate: string;
};

export type StaticTableMetas = Array<StaticTableMeta>;
export type StaticTableRows = Array<StaticTableRow>;

export type StaticTableMetaTemplate = {
    id: number | null;
    description: string;
    areaIdentifier: string;
    staticTableRows: StaticTableRows;
    perPersonInputRequired: boolean;
};

export type StaticTableMeta = StaticTableMetaTemplate & {
    tableOrder: number;
};

export type StaticTableRow = {
    id: number | null;
    rowOrder: number;
    description: string;
    fee: StaticFee;
    range: boolean;
    perPerson: boolean;
    tableOrder: number;
    areaIdentifier: string;
};

export type StaticFee = {
    id: number | null;
    feeId: string;
    min: number;
    max: number;
};

export type StaticTableValidationResult = {
    result: boolean;
    message: string;
};

type HTML = string;

export type FileLink = {
    fileId: string;
    fileName: string;
    link: string;
    isUrl: boolean;
    s3Key: string;
};

export type FileLinkReference = {
    fileReference: string;
    fileId: string;
    fileName: string;
};

export type EnquiryRow = {
    id: number;
    conversationId: string;
    linkReference: FileLinkReference;
    timeStamp: string;
    accountId: string;
    areaName: string;
    emailTemplateUsed: string;
    seen: boolean;
    name: string;
    email: string;
    phoneNumber: string;
    hasResponse: boolean;
    areaIdentifier: string;
};

export type Enquiries = EnquiryRow[];

export type EnquiryActivtyResource = {
    intentName: string;
    numRecords: number;
    intentIdentifier: string;
    completed: number;
    sentEmailCount: number;
    averageIntentCompletion: number;
    intentCompletePerIntent: number[];
};

export type DynamicTableMeta = {
    id: number;
    tableTag: string;
    tableType: string;
    tableId: string;
    accountId: string;
    areaId: string;
    valuesAsPaths: boolean;
    prettyName: string;
};

export type DynamicTableMetas = Array<DynamicTableMeta>;

export type AlertType = {
    title: string;
    message: string;
    link?: string;
    linktext?: string;
};

export type EmailVerificationResponse = {
    status: "Success" | "Pending" | "Failed";
    title: string;
    message: string;
};

export type AlertDetails = {
    title: string;
    message: string;
};

// Common interfaces

export interface IHaveWidth {
    width: "xs" | "sm" | "md" | "lg" | "xl";
}

export interface IGetHelp {
    setHelpType(helpType: HelpTypes): void;
}

export type HelpTypes =
    | "editor"
    | "settings"
    | "demo"
    | "enquiries"
    | "getwidget"
    | "subscribe"
    | "conversation"
    | "estimate"
    | "email"
    | "attachments"
    | "preview"
    | "areasettings"
    | "password"
    | "email"
    | "companyname"
    | "phonenumber"
    | "logo"
    | "locale";

export type ExtraQueryParams = {
    authuser: string;
};

export type SessionState = {
    extraQueryParams: ExtraQueryParams;
};

export type GoogleAuthObject = {
    expires_at: number;
    expires_in: number;
    first_issued_at: number;
    id_token: string;
    idpId: string;
    login_hint: string;
    session_state: SessionState;
    token_type: string; // Bearer
};

export type GoogleProfileObj = {
    getEmail(): string;
    getFamilyName(): string;
    getGivenName(): string;
    getImageUrl(): string;
    getName(): string;
};

export type GoogleAuthResponse = {
    getBasicProfile(): GoogleProfileObj;
    getAuthResponse(): GoogleAuthObject;
    getId(): string;
};

export type Images = Array<string>;
export type StripeProduct = {
    id: string;
    object: string;
    active: boolean;
    created: Date;
    deleted: boolean | null;
    livemode: boolean;
    rawJObject: null;
    stripeResponse: null;
    metadata: object;
    type: string;
};

export type Product = StripeProduct & {
    attributes: Array<string>;
    caption: string | null;
    deactivateOn: Date | null;
    description: string;
    images: Images;
    name: string;
    packageDimensions: string | null;
    shippable: boolean | null;
    statementDescriptor: string | null;
    unitLabel: string | null;
    updated: Date;
    url: string | null;
};

export type Products = Array<Product>;
export type Prices = Array<Price>;
export type Price = StripeProduct & {
    billingScheme: string;
    currency: "usd" | "aud" | "can" | "eur";
    lookupKey: string | null;
    nickname: string | null;
    productId: string; // equals Product type id
    product: string | null;
    recurring: {
        aggregateUsage: string | null;
        interval: "month" | "year";
        intervalCount: number;
        trialPeriodDays: number | null;
        usageType: string;
        rawJObject: object;
        stripeResponse: null;
    };
    tiers: null; // TODO
    tiersMode: null; // TODO
    transformQuantity: null; // TODO
    unitAmount: number; // cents
    unitAmountDecimal: number; // in cents
};

export enum GeneralSettingsLoc {
    email,
    companyName,
    phoneNumber,
    companyLogo,
    locale,
    default_email_template,
    deleteaccount,
    password,
}

export type Action = {
    icon: React.ReactNode;
    name: string;
    onClick(): void;
};

export type Credentials = {
    jwtToken: string;
    apiKey: string;
    sessionId: string;
    emailAddress: string;
    authenticated: boolean;
    message: string;
};

export type ResponseConfigurationType = {
    prologue: string;
    epilogue: string;
    staticTablesMetas: StaticTableMetas;
    sendPdfResponse: boolean;
};

export type AccountEmailSettingsResponse = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
};

export type PhoneSettingsResponse = {
    phoneNumber: string;
    locale: string;
};

export enum NodeTypeCode {
    I,
    II,
    III,
    IV,
    V,
    VI, // anabranch
    VII, // loopback anchor
    VIII, // loopback terminal,
    IX, // image node type
    X, // multioption non editable, one path
    XI, // multioption non editable, multiple paths
}

export type NodeOption = {
    groupName: string;
    isAnabranchMergePoint: boolean;
    isAnabranchType: boolean;
    isCurrency: boolean; // TODO: For the future -- may wish to specify currency or number in the dynamic table
    isDynamicType: boolean;
    isMultiOptionEditable: boolean; // TODO- is this used? No...
    isMultiOptionType: boolean;
    isSplitMergeType: boolean;
    isTerminalType: boolean;
    nodeComponentType: string;
    pathOptions: Array<Response>;
    resolveOrder: number;
    shouldRenderChildren: boolean;
    shouldShowMultiOption: boolean;
    stringName: string | null; // TODO: this is always null - used?
    text: string;
    value: string;
    valueOptions: Array<string>;
    dynamicType: string | null;
    isImageNode: boolean;
    imageId: string | null;
    nodeTypeCode: NodeTypeCode; // passed to the node changer via the nodeOptions
    isLoopbackAnchor: boolean;
};

export type NodeTypeOptions = NodeOption[];

export type RequiredDetails = {
    type: string;
    prettyName: string;
};

export type FormStatusTypes =
    | typeof INVALID_EMAIL
    | typeof NOT_A_DEFAULT_ACCOUNT
    | typeof INVALID_PASSWORD
    | typeof INVALID_GOOGLE_TOKEN
    | typeof NOT_A_GOOGLE_ACCOUNT
    | typeof GOOGLE_ACCOUNT_NOT_FOUND
    | typeof COULD_NOT_FIND_SERVER
    | typeof VERIFICATION_EMAIL_SEND
    | null;

export type PlanType = "Free" | "Lyte" | "Premium" | "Pro";

export type PlanStatus = {
    status: PlanType;
    hasUpgraded: boolean;
};

export enum PurchaseTypes {
    Free = "Free",
    Lyte = "Lyte",
    Premium = "Premium",
    Pro = "Pro",
}

export type PlanTypeMeta = {
    allowedAttachments: number;
    allowedStaticTables: number;
    allowedDynamicTables: number;
    allowedAreas: number;

    allowedImageUpload: boolean;
    allowedEmailNotifications: boolean;
    allowedInlineEmailEditor: boolean;
    allowedSmsNotifications: boolean;

    planType: PurchaseTypes;
    isFreePlan: boolean;
};

export type ProductOption = {
    card: React.ReactNode;
    purchaseType: PurchaseTypes;
    productId: string | null;
    currentplan: boolean;
};

export type ProductOptions = ProductOption[];

export enum Interval {
    free = "free",
    monthly = "month",
    yearly = "year",
}

export type PriceMap = {
    [key: string]: string | number;
};

export type ConversationUpdate = {
    id: number;
    conversationId: string;
    prompt: string;
    userResponse: string;
    nodeId: string;
    nodeCritical: number;
    nodeType: string;
    timeStamp: string;
    isEnabled: number;
    account: string;
};

export type CompletedConversation = ConversationUpdate[];

export type PreCheckError = {
    areaName: string;
    reasons: string[];
};

export type PreCheckResult = {
    isReady: boolean;
    preCheckErrors: PreCheckError[];
    apiKeyExists: boolean;
};

export type IncompleteArea = {
    areaDisplayTitle: string;
    areaName: string;
    reason: string[];
};

export type IncompleteAreas = IncompleteArea[];


export type VariableDetail = {
    name: string;
    pattern: string;
    details: string;
};

export type RememberMe = {
    emailAddress: string;
    password: string;
};

export type ResetEmailResponse = {
    message: string;
    status: boolean;
    link: string;
};

export type ResetPasswordResponse = {
    message: string;
    status: boolean;
};

export type VerificationResponse = {
    message: string;
    status: boolean;
    apiKey: string;
};

export type Settings = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
    areaName: string;
    areaTitle: string;
    subject: string;
    isEnabled: boolean;
    useAreaFallbackEmail: boolean;
};

export type AreasEnabled = {
    areaId: string;
    isEnabled: boolean;
    areaName: string;
};

export type ToggleStateChanger = Dispatch<SetStateAction<boolean | null>>;

export type LocaleResponse = {
    currentLocale: LocaleResource;
    localeMap: LocaleResource[];
};

export type LocaleResource = {
    name: string;
    displayName: string;
    currencySymbol: string;
    supportedLocales: string[];
    phoneFormat: string;
    numberDecimalSeparator: string;
    localeMap: LocaleMap;
};

export type LocaleMap = LocaleResource[];

export type ProductIds = {
    freeProductId: string;
    lyteProductId: string;
    premiumProductId: string;
    proProductId: string;
};

export type Todos = {
    name: string;
    emailAddress: string;
    logoUri: string;
    isVerified: boolean;
    awaitingVerification: boolean;
    phoneNumber: string;
};

export type TodosAsBoolean = {
    name: boolean;
    emailAddress: string;
    logoUri: boolean;
    isVerified: boolean;
    awaitingVerification: boolean;
    phoneNumber: boolean;
};

export type SplitMergeMeta = {
    isDecendentOfSplitMerge: boolean;
    decendentLevelFromSplitMerge: number;
    splitMergeRootSiblingIndex: number;
    nodeIdOfMostRecentSplitMergePrimarySibling: string;
};

export type AnabranchMeta = {
    isDecendentOfAnabranch: boolean;
    decendentLevelFromAnabranch: number;
    nodeIdOfMostRecentAnabranch: string;
    isDirectChildOfAnabranch: boolean;
    isParentOfAnabranchMergePoint: boolean;
    isAncestorOfAnabranchMergePoint: boolean;
};

export type NodeId = string;

export type NodeSetterWithHistory = (value: React.SetStateAction<PalavyrLinkedList>) => void;

export type NodeIdentity = {
    // splitmerge
    splitMergeRootSiblingIndex: number;
    decendentLevelFromSplitMerge: number;
    nodeIdOfMostRecentSplitMergePrimarySibling: NodeId;
    isDecendentOfSplitMerge: boolean;
    shouldShowMergeWithPrimarySiblingBranchOption: boolean;
    shouldShowSplitMergePrimarySiblingLabel: boolean;
    shouldCheckSplitMergeBox: boolean;

    // anabranch
    decendentLevelFromAnabranch: number;
    nodeIdOfMostRecentAnabranch: NodeId;
    otherNodeAlreadySetAsMergeBranchBool: boolean;
    isAnabranchMergePoint: boolean;
    isDecendentOfAnabranch: boolean;
    isDirectChildOfAnabranch: boolean;
    isParentOfAnabranchMergePoint: boolean;
    isAncestorOfAnabranchMergePoint: boolean;
    isOnLeftmostAnabranchBranch: boolean;
    shouldShowSetAsAnabranchMergePointOption: boolean;
    shouldShowAnabranchMergepointLabel: boolean;
    isInternalToAnabranch: boolean;
    isInternalToSplitMerge: boolean;

    // general
    canUnSetNodeType: boolean;
    shouldDisabledNodeTypeSelector: boolean;
    shouldShowResponseInPdfOption: boolean;
    shouldShowUnsetNodeTypeOption: boolean;
};

// Dynamic Table types
export type SelectOneFlatData = {
    id: number;
    accountId: string;
    areaId: string;
    tableId: string;
    option: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowOrder: number;
};

export type PercentOfThresholdData = {
    id: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    itemId: string;
    itemName: string;
    itemOrder: number;
    rowId: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    modifier: number;
    posNeg: boolean;
    rowOrder: number;
    triggerFallback: boolean;
};

export type BasicThresholdData = {
    id: number;
    rowId: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    itemName: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowOrder: number;
    minThreshold: number;
    maxThreshold: number;
    triggerFallback: boolean;
};

export type TwoNestedCategoryData = {
    id: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowId: string;
    rowOrder: number;
    itemId: string;
    itemOrder: number;
    itemName: string;
    innerItemName: string;
};

export type CategoryNestedThresholdData = {
    id: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowId: string;
    rowOrder: number;
    itemId: string;
    itemOrder: number;
    itemName: string;
    threshold: number;
    triggerFallback: boolean;
};

export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | BasicThresholdData[] | TwoNestedCategoryData[] | CategoryNestedThresholdData[] | any; // | SelectOneThresholdData etc

export type DynamicTableData = {
    tableRows: TableData;
    isInUse: boolean;
};

export interface IDynamicTableBody {
    tableData: TableData;
    modifier: any;
}

export type DynamicTableProps = {
    tableData: Array<TableData>;
    setTableData: SetState<TableData>;
    areaIdentifier: string;
    tableId: string;
    tableTag: string;
    tableMeta: DynamicTableMeta;
    setTableMeta: any;
    deleteAction(): Promise<any>;
    showDebug: boolean;
};

export type DynamicTableComponentMap = {
    [key: string]: (props: DynamicTableProps) => JSX.Element;
};

export type TableNameMap = {
    [tableName: string]: string;
};

export type TreeErrors = {
    missingNodes: string[];
    outOfOrder: string[];
    anyErrors: boolean;
};

export type AreaNameDetail = {
    areaName: string;
    areaIdentifier: string;
};

export type AreaNameDetails = AreaNameDetail[];

export type SnackbarPositions = "tr" | "t" | "tl" | "bl" | "b" | "br";

export type EmptyComponentType = React.ComponentType<{}>;

export type LineLink = {
    from: string;
    to: string;
};
export type LineMap = LineLink[];

export type LoopbackContext = {
    loopbackOriginId: string;
};

export type AnabranchContext = {
    anabranchOriginId: string; // the node Id of the anabranch root node
    leftmostAnabranch: boolean;
};

export interface IDashboardContext {
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
    panelErrors: ErrorResponse | null;
    setPanelErrors: SetState<ErrorResponse | null>;
    repository: PalavyrRepository;
    areaNameDetails: AreaNameDetails;
    reRenderDashboard(): void;
    handleDrawerOpen(): void;
    handleDrawerClose(): void;
    menuOpen: boolean;
}

export type ErrorResponse = {
    message: string;
    additionalMessages: string[];
    statusCode: number;
};

export interface IAuthContext {
    isActive: boolean;
    isAuthenticated: boolean;
}

export interface IConversationHistoryTracker {
    addConversationHistoryToQueue(dirtyConversationRecord: PalavyrLinkedList, conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]): void;
    stepConversationBackOneStep(conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]): void;
    stepConversationForwardOneStep(conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]): void;
}

export interface IConversationTreeContext {
    setNodes: NodeSetterWithHistory;
    historyTracker: IConversationHistoryTracker;
    conversationHistory: PalavyrLinkedList[];
    conversationHistoryPosition: number;
    nodeTypeOptions: NodeTypeOptions;
    showDebugData: boolean;
    useNewEditor: boolean;
}

export type YoutubeVideoResourcePlayer = {
    embedHtml: string;
    embedHeight: number;
    embedWidth: number;
};

export type YoutubeVideoResourceSnippet = {
    publishedAt: string;
    channelId: string;
    title: string;
    description: string;
    channelTitle: string;
    tags: [string];
    categoryId: string;
    liveBroadcastContent: string;
    defaultLanguage: string;
    localized: {
        title: string;
        description: string;
    };
    defaultAudioLanguage: string;
};

export type YoutubePlaylistItemContentDetails = {
    videoId: string;
    videoPublishedAt: string;
};

export type PlaylistItemsResource = {
    kind: string;
    etag: string;
    snippet: YoutubeVideoResourceSnippet;
    contentDetails: YoutubePlaylistItemContentDetails;
};

export type YoutubePlaylistItemsResponse = {
    kind: string;
    etag: string;
    items: PlaylistItemsResource[];
};

export type VideoMap = {
    videoId: string;
    title: string;
    description: string;
};

export type BlogPostRecord = {
    title: string;
    id: number;
    date: number;
    src: string; // an image src uri
    snippet: string; // short description
    content: React.ReactNode;
};
export type BlogPosts = BlogPostRecord[];

export type BlogPostRouteMeta = BlogPostRecord & {
    url: string;
    params: string;
};

/////////////////////////////////////////////////////////////
////////////// WIDGET DIRECT COPY FOR NOW ////////////////////
// export type SelectedOption = {
//     areaDisplay: string;
//     areaId: string;
// };

// export interface IMessage extends BaseMessage {
//     text: string;
// }

// export interface Link extends BaseMessage {
//     title: string;
//     link: string;
//     target: string;
// }

// export interface LinkParams {
//     link: string;
//     title: string;
//     target?: string;
// }

// export interface CustomCompMessage extends BaseMessage {
//     props: any;
// }

// export type Registry = {
//     [key: string]: any;
// };

// export type WidgetNodeResource = {
//     areaIdentifier: string;
//     nodeId: string;
//     text: string;
//     nodeType: string;
//     nodeChildrenString: string;
//     isRoot: boolean;
//     isCritical: boolean;
//     optionPath: string | null;
//     valueOptions: string; // needs to be split by ","
//     nodeComponentType: string;
//     isDynamicTableNode: boolean;
//     dynamicType: string | null;
//     resolveOrder: number | null;
// };

////////////////// Widget

export type SecretKey = string | null;

export type WidgetAreaTable = {
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
};

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
    buttonColor: string;
    buttonFontColor: string;
};


export type WidgetConversationUpdate = {
    ConversationId: string;
    Prompt: string;
    UserResponse: string | null;
    NodeId: string;
    NodeCritical: boolean;
    NodeType: string;
};

export type ConversationRecordUpdate = {
    ConversationId: string;
    AreaIdentifier: string;
    Name: string;
    Email: string;
    PhoneNumber: string;
    Fallback: boolean;
    Locale: string;
    IsComplete: boolean;
};

export type WidgetPreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>;
};

export type SendEmailResultResponse = {
    nextNodeId: string;
    result: boolean;
};

export interface IProgressTheChat {
    node: WidgetNodeResource;
    nodeList: WidgetNodes;
    client: PalavyrWidgetRepository;
    convoId: string;
}

export type Nullable<T> = T | null;

// export type AnyFunction = (...args: any[]) => any;

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

export type SpecificResponse = {
    [nodeId: string]: string; // node.nodeId: response value;
};
export type DynamicResponse = {
    [UniqueTableKey: string]: SpecificResponse[];
};
export type DynamicResponses = DynamicResponse[];

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
    widgetPreferences: WidgetPreferences | null;
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
    widgetPreferences: WidgetPreferences | null;
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

export const SET_WIDGETPREFERENCES_CONTEXT = "CONTEXT_WIDGETPREFS/SET_WIDGETPREFERENCES";
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

export type ContextPropertyActions = setContextProperties | getContextProperties | setNameContext | setPhoneContext | setEmailAddressContext | setRegionContext | addKeyValueContext | setDynamiceResponses;

export type AllActions = ContextPropertyActions | FullscreenPreviewActions | BehaviorActions | MessagesActions;