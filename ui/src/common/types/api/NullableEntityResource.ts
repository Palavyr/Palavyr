import { StaticFeeResource } from "./EntityResources";
import { NodeTypeCodeEnum } from "./Enums";

export type NullableEntityRsource = {
    id: number | null;
};

export type ConversationHistoryRowResources = ConversationHistoryRowResource[];

export type ConversationHistoryRowResource = NullableEntityRsource & {
    conversationId: string;
    prompt: string;
    userResponse: string;
    nodeId: string;
    nodeCritical: boolean;
    nodeType: string;
    timeStamp: string;
};

export type ConversationDesignerNodeResources = ConversationDesignerNodeResource[];

export type ConversationDesignerNodeResource = NullableEntityRsource & {
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

export type StaticTableMetaResources = StaticTableMetaResource[];
export type StaticTableMetaResource = NullableEntityRsource & {
    tableOrder: number;
    description: string;
    intentId: string;
    staticTableRowResources: StaticTableRowResources;
    perPersonInputRequired: boolean;
    includeTotals: boolean;
    tableId: string;
};

export type StaticTableRowResources = StaticTableRowResource[];
export type StaticTableRowResource = NullableEntityRsource & {
    rowOrder: number;
    description: string;
    fee: StaticFeeResource;
    range: boolean;
    perPerson: boolean;
    tableOrder: number;
    intentId: string;
};
