import { ConvoNode } from "@Palavyr-Types";
import { v4 as uuid } from "uuid";

export const createDefaultNode = (optionPath: string): ConvoNode => ({
    IsSplitMergeMergePoint: false,
    nodeId: uuid(),
    nodeType: "",
    text: "Ask your question!",
    nodeChildrenString: "",
    isRoot: false,
    fallback: false,
    isCritical: false,
    areaIdentifier: "",
    optionPath: optionPath,
    valueOptions: "",
    isMultiOptionType: false,
    isTerminalType: false,
    isSplitMergeType: false,
    shouldRenderChildren: true,
    shouldShowMultiOption: false,
    isAnabranchMergePoint: false,
    isAnabranchType: false,
    nodeComponentType: "",
    isDynamicTableNode: false,
    resolveOrder: 0,
    dynamicType: "",
    isImageNode: false,
    imageId: null,
});
