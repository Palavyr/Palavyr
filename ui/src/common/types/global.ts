import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { COULD_NOT_FIND_SERVER, INVALID_EMAIL, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, VERIFICATION_EMAIL_SEND } from "@constants";
import { PalavyrLinkedList } from "frontend/dashboard/content/responseConfiguration/conversation/PalavyrDataStructure/PalavyrLinkedList";
import React, { Dispatch, SetStateAction } from "react";

import "./api/EntityResources";
import { FileAssetResource, PricingStrategyTableMetaResource, PricingStrategyTableTypeResource, TableData } from "./api/EntityResources";
import { NodeTypeCodeEnum, PurchaseTypes, UnitGroups, UnitPrettyNames } from "./api/Enums";
import { QuantUnitDefinition, NodeTypeOptionResources } from "./api/ApiContracts";
import { ConversationDesignerNodeResource, ConversationDesignerNodeResources } from "./api/NullableEntityResource";

export * from "./api/EntityResources";
export * from "./api/ApiContracts";
export * from "./api/Enums";
export * from "./api/NullableEntityResource";
export * from "./widget/widget";

// / <reference types="node" />
// / <reference types="react" />
// / <reference types="react-dom" />

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

export const ValueOptionDelimiter = "|peg|";

export type StaticTableValidationResult = {
    result: boolean;
    message: string;
};

export type SelectionMap = {
    [conversationId: string]: boolean;
};

export type MarkAsSeenUpdate = {
    conversationId: string;
    seen: boolean;
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
    | "intentsettings"
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

export type Images = string[];
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
    allowedIntents: number;

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
    intentDisplayTitle: string;
    intentName: string;
    reason: string[];
};

export type IncompleteIntents = IncompleteIntent[];

export type RememberMe = {
    emailAddress: string;
    password: string;
};

export type Settings = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
    intentName: string;
    intentTitle: string;
    subject: string;
    isEnabled: boolean;
    useIntentFallbackEmail: boolean;
};

export type IntentsEnabled = {
    intentId: string;
    isEnabled: boolean;
    intentName: string;
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

export type TableNameMap = PricingStrategyTableTypeResource[];

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
    checkIntentCount(): void;
    intentName: string;
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
    intentNameDetails: IntentNameDetails;
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
    nodeTypeOptions: NodeTypeOptionResources;
    showDebugData: boolean;
    useNewEditor: boolean;
}

// export interface ImageState {
//     src: string;
//     alt?: string;
//     width: number;
//     height: number;
// }

export interface NodeOptionalProps {
    node: IPalavyrNode;
}

export interface ILinkedListBucket {
    addToBucket(node: IPalavyrNode): void;
    convertToConvoNodes(intentId: string): ConversationDesignerNodeResources;
    addToBucket(node: IPalavyrNode): void;
    clear(): void;
    findById(nodeId: string): IPalavyrNode | null;
}

export interface IPalavyrLinkedList {
    rootNode: IPalavyrNode;
    intentId: string;
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
    compileConvoNode(intentId: string): ConversationDesignerNodeResource;
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
