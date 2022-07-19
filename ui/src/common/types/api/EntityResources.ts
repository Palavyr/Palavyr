import { NodeTypeCodeEnum, UnitGroups, UnitIdEnum, UnitPrettyNames } from "./Enums";

export type EntityResource = {
    id: number;
};

// Entity Resources
export type WidgetNodeResources = WidgetNodeResource[];
export type WidgetNodeResource = EntityResource & {
    intentId: string;
    nodeId: string;
    text: string;
    nodeType: string;
    nodeChildrenString: string;
    isRoot: boolean;
    isCritical: boolean;
    optionPath: string;
    valueOptions: string;
    nodeComponentType: string;
    isPricingStrategyTableNode: boolean;
    pricingStrategyType: string;
    resolveOrder: number;
    unitId: string;
    fileAssetResource?: FileAssetResource;
};

export type IntentResources = IntentResource[];
export type IntentResource = EntityResource & {
    intentId: string;
    intentName: string;
    prologue: string;
    epilogue: string;
    emailTemplate: string;
    isEnabled: boolean;
    staticTablesMetaResources: StaticTableMetaResources;
    conversationNodeResources: ConversationDesignerNodeResources;
    pricingStrategyTableMetaResources: PricingStrategyTableMetaResources;
    intentSpecificEmail: string;
    emailIsVerified: boolean;
    attachmentRecordResources: AttachmentLinkRecordResource;
    useIntentFallbackEmail: boolean;
    fallbackSubject: string;
    fallbackEmailTemplate: string;
    sendAttachmentsOnFallback: boolean;
    sendPdfResponse: boolean;
    includePricingStrategyTableTotals: boolean;
    subject: string;
};

export type WidgetPreferencesResource = EntityResource & {
    placeholder: string;
    accountId: string;
    landingHeader: string;
    chatHeader: string;
    selectListColor: string;
    listFontColor: string;
    headerColor: string;
    headerFontColor: string;
    fontFamily: string;
    optionsHeaderColor: string;
    optionsHeaderFontColor: string;
    chatFontColor: string;
    chatBubbleColor: string;
    buttonColor: string;
    buttonFontColor: string;
    widgetState: boolean;
};

export type AttachmentLinkRecordResources = AttachmentLinkRecordResource[];
export type AttachmentLinkRecordResource = EntityResource & {
    intentId: string;
    fileId: string;
};

export type NewConversationResource = EntityResource & {
    conversationId: string;
    conversationNodes: WidgetNodeResources;
};

export type FileAssetResource = EntityResource & {
    fileName: string;
    fileId: string;
    link: string;
};

export type EnquiryResources = EnquiryResource[];
export type EnquiryResource = EntityResource & {
    id: number;
    conversationId: string;
    fileAssetResource: FileAssetResource;
    timeStamp: string;
    accountId: string;
    intentName: string;
    emailTemplateUsed: string;
    seen: boolean;
    name: string;
    email: string;
    phoneNumber: string;
    hasResponse: boolean;
};

export type StaticFeeResources = StaticFeeResource[];
export type StaticFeeResource = {
    min: number;
    max: number;
    feeId: string;
    intentId: string;
    int: number;
};

export type StaticTableRowResources = StaticTableRowResource[];
export type StaticTableRowResource = EntityResource & {
    rowOrder: number;
    description: string;
    fee: StaticFeeResource;
    range: boolean;
    perPerson: boolean;
    tableOrder: number;
    intentId: string;
};

export type StaticTableMetaResources = StaticTableMetaResource[];
export type StaticTableMetaResource = EntityResource & {
    tableOrder: number;
    description: string;
    intentId: string;
    staticTableRowResources: StaticTableRowResources;
    accountId: string;
    perPersonInputRequired: boolean;
    includeTotals: boolean;
};

export type PricingStrategyTableTypeResource = EntityResource & {
    prettyName: string;
    tableType: string;
};

export type PricingStrategyTableMetaResources = PricingStrategyTableMetaResource[];
export type PricingStrategyTableMetaResource = EntityResource & {
    tableTag: string;
    tableType: string;
    tableId: string;
    intentId: string;
    prettyName: string;
    unitPrettyName: UnitPrettyNames;
    unitGroup: UnitGroups;
    unitIdEnum: UnitIdEnum;
    accountId: string;
    valuesAsPaths: boolean;
};

export type TableData = CategorySelectTableRowResource[] | PercentOfThresholdResource[] | BasicThresholdResource[] | TwoNestedCategoryResource[] | CategoryNestedThresholdResource[] | any;

export type BasicThresholdResource = EntityResource & {
    accountId: string;
    intentId: string;
    rowId: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    itemName: string;
    rowOrder: number;
    triggerFallback: boolean;
    id: number;
    tableId: string;
};

export type CategoryNestedThresholdResource = EntityResource & {
    accountId: string;
    intentId: string;
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
    id: number;
    tableId: string;
};

export type PercentOfThresholdResource = EntityResource & {
    intentId: string;
    rowId: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    modifier: number;
    posNeg: boolean;
    rowOrder: number;
    triggerFallback: boolean;
    itemOrder: number;
    itemId: string;
    itemName: string;
    id: number;
    tableId: string;
};

export type CategorySelectTableRowResource = EntityResource & {
    intentId: string;
    category: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowOrder: number;
    id: number;
    tableId: string;
};

export type TwoNestedCategoryResource = EntityResource & {
    intentId: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowId: string;
    rowOrder: number;
    itemId: string;
    itemOrder: number;
    itemName: string;
    innerItemName: string;
    id: number;
    tableId: string;
};

export type ConversationHistoryRowResources = ConversationHistoryRowResource[];

export type ConversationHistoryRowResource = EntityResource & {
    conversationId: string;
    prompt: string;
    userResponse: string;
    nodeId: string;
    nodeCritical: boolean;
    nodeType: string;
    timeStamp: string;
};

export type ConversationDesignerNodeResources = ConversationDesignerNodeResource[];

export type ConversationDesignerNodeResource = EntityResource & {
    intentId: string;
    nodeId: string;
    text: string;
    isRoot: boolean;
    isCritical: boolean;
    isMultiOptionType: boolean;
    isTerminalType: boolean;
    shouldRenderChildren: boolean;
    isLoopbackAnchorType: boolean;
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;
    shouldShowMultiOption: boolean;
    isPricingStrategyNode: boolean;
    isMultiOptionEditable: boolean;
    isImageNode: boolean;
    fileId: string;
    optionPath: string;
    valueOptions: string;
    nodeType: string;
    pricingStrategyType: string;
    nodeComponentType: string;
    resolveOrder: number;
    isCurrency: boolean;
    nodeChildrenString: string;
    nodeTypeCodeEnum: NodeTypeCodeEnum;
};
