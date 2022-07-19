import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { COULD_NOT_FIND_SERVER, INVALID_EMAIL, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, VERIFICATION_EMAIL_SEND } from "@constants";
import { PalavyrLinkedList } from "frontend/dashboard/content/responseConfiguration/conversation/PalavyrDataStructure/PalavyrLinkedList";
import React, { Dispatch, ElementType, SetStateAction } from "react";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { ConversationDesignerNodeResource, ConversationDesignerNodeResources, FileAssetResource, IntentResource, PricingStrategyTableMetaResource, StaticTableMetaResources, TableData, WidgetNodeResource, WidgetNodeResources, WidgetPreferencesResource } from "./api/EntityResources";
import { NodeTypeOptionResource, NodeTypeOptionResources, QuantUnitDefinition } from "./api/ApiContracts";
import { NodeTypeCode, NodeTypeCodeEnum, PurchaseTypes, UnitGroups, UnitPrettyNames } from "./api/Enums";
// / <reference types="node" />
// / <reference types="react" />
// / <reference types="react-dom" />

/*
The front end needs to send a request to an end point with a PARAM, :intentId, and this
will be sent to C# backend API which will use the accountID and intentId to retrieve from a particular endpoint

something like /endpoints/:accountId/:intentId/?authToken=23b23k5iuhi2u5b2kjb2k34uhn234ujn

Then the API will extract the accountID, the intentId and use it with the endpoint to call a DB controller
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
    intentId: string;
    groupId: string;
    areaName: string;
};

export const ValueOptionDelimiter = "|peg|";

export type StaticTableValidationResult = {
    result: boolean;
    message: string;
};

export type SelectionMap = {
    [conversationId: string]: boolean;
};

export type MarkAsSeenUpdate = {
    sonversationId: string;
    seen: boolean;
};

export type EnquiryActivtyResource = {
    intentName: string;
    numRecords: number;
    intentIdentifier: string;
    completed: number;
    sentEmailCount: number;
    averageIntentCompletion: number;
    intentCompletePerIntent: number[];
};

export type AlertType = {
    title: string;
    message: string;
    link?: string;
    linktext?: string;
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



export type Action = {
    icon: React.ReactNode;
    name: string;
    onClick(): void;
};

export type ResponseConfigurationType = {
    prologue: string;
    epilogue: string;
    staticTablesMetas: StaticTableMetaResources;
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

export type PlanTypeMeta = {
    allowedAttachments: number;
    allowedStaticTables: number;
    allowedPricingStrategys: number;
    allowedAreas: number;

    allowedFileUpload: boolean;
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

export type PriceMap = {
    [key: string]: string | number;
};

export type IncompleteIntent = {
    areaDisplayTitle: string;
    areaName: string;
    reason: string[];
};

export type IncompleteIntents = IncompleteIntent[];

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

export type IntentsEnabled = {
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
    logoUri: FileAssetResource;
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

export type PricingStrategyData = {
    tableRows: TableData;
    isInUse: boolean;
};

export interface IPricingStrategyBody {
    tableData: TableData;
    modifier: any;
    unitGroup?: UnitGroups;
    unitPrettyName?: UnitPrettyNames;
}

export type PricingStrategy = {
    tableMeta: PricingStrategyTableMetaResource;
    tableRows: TableData;
};

export type PricingStrategyValidationResult = {
    isValid: boolean;
    tableRows: TableData;
};
export interface Modifier {
    validateTable: (tableRows: TableData) => PricingStrategyValidationResult;
}

export type PricingStrategyProps = {
    availablePricingStrategyOptions: PricingStrategyTableTypeResource[];
    tableNameMap: TableNameMap;
    unitTypes: QuantUnitDefinition[];
    inUse: boolean;
    intentId: string;
    tableId: string;
    deleteAction(): Promise<void>;
    showDebug: boolean;
    setTables: SetState<PricingStrategy[]>;
    table: PricingStrategy;
    tables: PricingStrategy[];
    tableIndex: number;
};

export type PricingStrategyComponentMap = {
    [key: string]: (props: PricingStrategyProps) => JSX.Element;
};

export type PricingStrategyTableTypeResource = {
    prettyName: string;
    tableType: string;
};
export type TableNameMap = PricingStrategyTableTypeResource[];

export type TreeErrors = {
    missingNodes: string[];
    outOfOrder: string[];
    anyErrors: boolean;
};

export type IntentNameDetail = {
    intentName: string;
    intentId: string;
};

export type IntentNameDetails = IntentNameDetail[];

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
    intentId: string;
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
    areaNameDetails: IntentNameDetails;
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
    nodeTypeOptions: NodeTypeOptionResource;
    showDebugData: boolean;
    useNewEditor: boolean;
}

////////////////// Widget

export type SecretKey = string | null;

export type WidgetAreaTable = {
    intentId: string;
    areaDisplayTitle: string;
};

export type SelectedOption = {
    intentDisplay: string;
    intentId: string;
};

export type Registry = {
    [key: string]: any;
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
    incompleteAreas: IntentResource[];
};

export interface IProgressTheChat {
    node: WidgetNodeResource;
    nodeList: WidgetNodeResources;
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

export type KeyValues = KeyValue[];

export type ContextProperties = {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferencesResource | null;
    responseFileAsset: FileAssetResource;
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
    convertToConvoNodes(areaId: string): ConversationDesignerNodeResources;
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
    compileToConvoNodes(): ConversationDesignerNodeResources;
    reconfigureTree(nodeTypeOptions: NodeTypeOptionResources): void;
    findNode(nodeId: string): IPalavyrNode | null;
    retrieveCleanHeadNode(): IPalavyrNode;
    updateTree: (updatedTree: IPalavyrLinkedList) => void;
    resetRootNode(): void;
    convertToPalavyrNode(repository: PalavyrRepository, rawNode: ConversationDesignerNodeResource, updateTree: (updatedTree: IPalavyrLinkedList) => void, leftMostBranch: boolean): IPalavyrNode;
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
    addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources): void;
    compileConvoNode(areaId: string): ConversationDesignerNodeResource;
    recursiveReferenceThisAnabranchOrigin(node: IPalavyrNode): void;
    dereferenceThisAnabranchMergePoint(anabranchOriginNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources): void;
    UpdateTree(): void;
    unsetSelf(nodeTypeOptions: NodeTypeOptionResources): void;
    nodeIsSet(): boolean;
    nodeIsNotSet(): boolean;
    setValueOptions(newValueOptions: string[]): void;
    addValueOption(newOption: string): void;
    getValueOptions(): string[];
    addLine(parentId: string): void;
    setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    removeLine(toNode: IPalavyrNode): void;
    setNodeTypeOptions(newNodeTypeOptions: NodeTypeOptionResources): void;
    Equals(otherNode: IPalavyrNode): boolean;
    LoopbackContextIsSet(): boolean;
    InsertChildNodeLink(nodeTypeOptions: NodeTypeOptionResources): void;
    DeleteCurrentNode(nodeTypeOptions: NodeTypeOptionResources): void;
    SetLoopbackContext(anchorId: string): void;

    isRoot: boolean;
    nodeId: string;
    userText: string;
    isTerminal: boolean;
    shouldPresentResponse: boolean; // isCritical
    nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    isMultiOptionType: boolean;
    isPricingStrategyNode: boolean;
    nodeComponentType: string;
    resolveOrder: number;
    shouldShowMultiOption: boolean;
    pricingStrategyType: string | null;
    fileId: string | null | undefined;
    nodeTypeOptions: NodeTypeOptionResources;
    shouldDisableNodeTypeSelector: boolean;
    isImageNode: boolean;
    nodeTypeCodeEnum: NodeTypeCodeEnum;
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
