import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { COULD_NOT_FIND_SERVER, INVALID_EMAIL, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, VERIFICATION_EMAIL_SEND } from "@constants";
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
    includeDynamicTableTotals: boolean;
};

export type StaticTableMetas = Array<StaticTableMeta>;
export type StaticTableRows = Array<StaticTableRow>;

export type StaticTableMetaTemplate = {
    id: number | null;
    description: string;
    areaIdentifier: string;
    staticTableRows: StaticTableRows;
    perPersonInputRequired: boolean;
    includeTotals: boolean;
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
    includeTotals: boolean;
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
    areaIdentifier: string;
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

export type FormStatusTypes = typeof INVALID_EMAIL | typeof NOT_A_DEFAULT_ACCOUNT | typeof INVALID_PASSWORD | typeof COULD_NOT_FIND_SERVER | typeof VERIFICATION_EMAIL_SEND | null;

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

export type NodeSetterWithHistory = (value: React.SetStateAction<IPalavyrLinkedList>) => void;

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
    addConversationHistoryToQueue(dirtyConversationRecord: PalavyrLinkedList): void;
    stepConversationBackOneStep(): void;
    stepConversationForwardOneStep(): void;
}

export interface IConversationTreeContext {
    historyTracker: IConversationHistoryTracker;
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
    selectionLabel: string;
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
    IntentId: string;
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
    pdfLink?: string;
};

export interface IProgressTheChat {
    node: WidgetNodeResource;
    nodeList: WidgetNodes;
    client: PalavyrWidgetRepository;
    convoId: string;
    designer?: boolean;
}

export type Nullable<T> = T | null;

type BaseMessage = {
    type: string;
    component: ElementType;
    sender: string;
    showAvatar: boolean;
    timestamp: Date;
    unread: boolean;
    customId?: string;
    props?: any;
    nodeType: string;
};
export interface UserMessageData extends BaseMessage {
    text: string;
}

export interface BotMessageData extends BaseMessage {
    props: any;
}

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
    pdfLink: string | null;
};

export interface ImageState {
    src: string;
    alt?: string;
    width: number;
    height: number;
}

export interface NodeOptionalProps {
    node: IPalavyrNode;
}

export interface ILinkedListBucket {
    addToBucket(node: IPalavyrNode): void;
    convertToConvoNodes(areaId: string): ConvoNode[];
    addToBucket(node: IPalavyrNode): void;
    clear(): void;
    findById(nodeId: string): IPalavyrNode | null;
}

export interface IPalavyrLinkedList {
    rootNode: IPalavyrNode;
    areaId: string;
    repository: PalavyrRepository;
    traverse(): void;
    insert(): void;
    delete(): void;
    compileToConvoNodes(): ConvoNode[];
    reconfigureTree(nodeTypeOptions: NodeTypeOptions): void;
    findNode(nodeId: string): IPalavyrNode | null;
    retrieveCleanHeadNode(): IPalavyrNode;
    updateTree: (updatedTree: IPalavyrLinkedList) => void;
    resetRootNode(): void;
    convertToPalavyrNode(repository: PalavyrRepository, rawNode: ConvoNode, updateTree: (updatedTree: IPalavyrLinkedList) => void, leftMostBranch: boolean): IPalavyrNode;
    compileToNodeFlow(): any;
}

export interface INodeReferences {
    nodes: IPalavyrNode[];
    joinedReferenceString: string;
    referenceStringArray: string[];
    references: IPalavyrNode[];
    Length: number;
    contains(nodeId: string): boolean;
    addReference(node: IPalavyrNode): void;
    addReferences(nodes: IPalavyrNode[]): void;
    Empty(): boolean;
    NotEmpty(): boolean;
    OrderByOptionPath(): void;
    Clear(): void;
    getByIndex(index: number): IPalavyrNode;
    removeReference(palavyrNode: IPalavyrNode): void;
    checkIfReferenceExistsOnCondition(condition: (nodeReference: IPalavyrNode) => boolean): boolean;
    truncateAt(index: number): void;
    applyOptionPaths(valueOptions: string[]): void;
    collectPathOptions(): string[];
    retrieveLeftmostReference(): IPalavyrNode | null;
    findIndexOf(node: IPalavyrNode): number | null;
    containsNode(node: IPalavyrNode): boolean;
    forEach(callBack: (node: IPalavyrNode, index?: number | undefined) => void): void;
    Single(): IPalavyrNode;
    Where(condition: (node: IPalavyrNode) => boolean): INodeReferences;
    containsNodeType(nodeType: string): boolean;
    AllChildrenUnset(): boolean;
    replaceAtIndex(index: number, newNode: IPalavyrNode): void;
    ShiftLeft(currentNode: IPalavyrNode): void;
    ShiftRight(currentNode: IPalavyrNode): void;
}

export interface IPalavyrNode {
    lock(): void;
    unlock(): void;
    setAsProvideInfo(): void;
    nodeIsNotSet(): boolean;
    AddNewChildReference(newChildReference: IPalavyrNode): void;
    sortChildReferences(): void;
    addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions): void;
    compileConvoNode(areaId: string): ConvoNode;
    recursiveReferenceThisAnabranchOrigin(node: IPalavyrNode): void;
    dereferenceThisAnabranchMergePoint(anabranchOriginNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions): void;
    UpdateTree(): void;
    unsetSelf(nodeTypeOptions: NodeTypeOptions): void;
    nodeIsSet(): boolean;
    nodeIsNotSet(): boolean;
    setValueOptions(newValueOptions: string[]): void;
    addValueOption(newOption: string): void;
    getValueOptions(): string[];
    addLine(parentId: string): void;
    setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    removeLine(toNode: IPalavyrNode): void;
    setNodeTypeOptions(newNodeTypeOptions: NodeTypeOptions): void;
    Equals(otherNode: IPalavyrNode): boolean;
    LoopbackContextIsSet(): boolean;
    InsertChildNodeLink(nodeTypeOptions: NodeTypeOptions): void;
    DeleteCurrentNode(nodeTypeOptions: NodeTypeOptions): void;
    SetLoopbackContext(anchorId: string): void;

    isRoot: boolean;
    nodeId: string;
    userText: string;
    isTerminal: boolean;
    shouldPresentResponse: boolean; // isCritical
    nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    isMultiOptionType: boolean;
    isDynamicTableNode: boolean;
    nodeComponentType: string;
    resolveOrder: number;
    shouldShowMultiOption: boolean;
    dynamicType: string | null;
    imageId: string | null | undefined;
    nodeTypeOptions: NodeTypeOptions;
    shouldDisableNodeTypeSelector: boolean;
    isImageNode: boolean;
    nodeTypeCode: NodeTypeCode;
    repository: PalavyrRepository;
    isLocked: boolean;

    // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    optionPath: string; // the value option that was used with the parent of this node.

    // transient
    shouldRenderChildren: boolean;
    isCurrency: boolean;

    // core
    childNodeReferences: INodeReferences;
    parentNodeReferences: INodeReferences;

    isMemberOfLeftmostBranch: boolean;
    lineMap: LineMap;

    palavyrLinkedList: IPalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;

    isPalavyrAnabranchStart: boolean;
    isPalavyrAnabranchMember: boolean;
    isPalavyrAnabranchEnd: boolean;
    isAnabranchLocked: boolean;
    anabranchContext: AnabranchContext;

    // LOOPBACK
    isLoopbackAnchorType: boolean;
    isLoopbackStart: boolean;
    isLoopbackMember: boolean;
    loopbackContext: LoopbackContext;
}
