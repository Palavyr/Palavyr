import { COULD_NOT_FIND_SERVER, GOOGLE_ACCOUNT_NOT_FOUND, INVALID_EMAIL, INVALID_GOOGLE_TOKEN, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, NOT_A_GOOGLE_ACCOUNT, VERIFICATION_EMAIL_SEND } from "@constants";
import { PalavyrLinkedList } from "dashboard/content/responseConfiguration/conversation/convoDataStructure/PalavyrLinkedList";
import { Dispatch, SetStateAction } from "react";
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
    optionPath: Response;
};

export type Response = "Yes" | "No" | "Not Sure" | "Ok" | "Backstop" | "Yes / Not Sure" | "No / Not Sure" | "Continue" | null | any;
export type Responses = Array<Response>;

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
    isSplitMergeType: boolean;
    IsSplitMergeMergePoint: boolean;
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;

    nodeType: string;
    fallback: boolean;
    optionPath: Response;
    valueOptions: string; // an array, but bc of the dtabase we store as a string delimited by |peg|
    shouldRenderChildren: boolean;
    shouldShowMultiOption: boolean;
    nodeComponentType: string;
    isDynamicTableNode: boolean;
    isImageNode: boolean;
    imageId: string | null;
    resolveOrder: number;
    dynamicType: string | null;
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
};

export type Enquiries = EnquiryRow[];

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

export enum AreaSettingsLoc {
    email,
    response,
    attachments,
    conversation,
    settings,
    preview,
}

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
};

// export type NodeTypeOptions = {[index: string]: NodeOptions};
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

export type WidgetPreferences = {
    selectListColor: string;
    headerColor: string;
    fontFamily: string;
    landingHeader: string;
    chatHeader: string;
    placeholder: string;
    listFontColor: string;
    headerFontColor: string;
    optionsHeaderColor: string;
    optionsHeaderFontColor: string;
    chatFontColor: string;
    chatBubbleColor: string;
    buttonColor: string;
    buttonFontColor: string;
};

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
    category: string;
    subCategory: string;
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
    category: string;
    threshold: number;
    triggerFallback: boolean;
};

export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | BasicThresholdData[] | TwoNestedCategoryData[] | CategoryNestedThresholdData[] | any; // | SelectOneThresholdData etc

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

export type SplitmergeContext = {
    splitmergeOriginId: string; // the node Id of the split merge root node
};

export type AnabranchContext = {
    anabranchOriginId: string; // the node Id of the anabranch root node
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
}

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
}
