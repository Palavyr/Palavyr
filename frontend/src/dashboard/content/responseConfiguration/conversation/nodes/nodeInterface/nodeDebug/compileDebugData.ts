import { ConvoNode, NodeIdentity } from "@Palavyr-Types";

export const debugDataItems = (identity: NodeIdentity) => {
    return [
        { isDecendentOfSplitMerge: identity.isDecendentOfSplitMerge },
        { decendentLevelFromSplitMerge: identity.decendentLevelFromSplitMerge },
        { splitMergeRootSiblingIndex: identity.splitMergeRootSiblingIndex },
        { nodeIdOfMostRecentSplitMergePrimarySibling: identity.nodeIdOfMostRecentSplitMergePrimarySibling },
        { shouldCheckSplitMergeBox: identity.shouldCheckSplitMergeBox },
        { isInternalToSplitMerge: identity.isInternalToSplitMerge },
        { shouldShowSplitMergePrimarySiblingLabel: identity.shouldShowSplitMergePrimarySiblingLabel },

        { isDecendentOfAnabranch: identity.isDecendentOfAnabranch },
        { decendentLevelFromAnabranch: identity.decendentLevelFromAnabranch },
        { nodeIdOfMostRecentAnabranch: identity.nodeIdOfMostRecentAnabranch },
        { isDirectChildOfAnabranch: identity.isDirectChildOfAnabranch },
        { isParentOfAnabranchMergePoint: identity.isParentOfAnabranchMergePoint },
        { isAncestorOfAnabranchMergePoint: identity.isAncestorOfAnabranchMergePoint },
        { isAnabranchMergePoint: identity.isAnabranchMergePoint },
        { otherNodeAlreadySetAsMergeBranchBool: identity.otherNodeAlreadySetAsMergeBranchBool },
        { isOnLeftmostAnabranchBranch: identity.isOnLeftmostAnabranchBranch },

        { isInternalToAnabranch: identity.isInternalToAnabranch },
        { shouldShowAnabranchMergepointLabel: identity.shouldShowAnabranchMergepointLabel },

        { shouldDisabledNodeTypeSelector: identity.shouldDisabledNodeTypeSelector },
        { canUnSetNodeType: identity.canUnSetNodeType },
        { shouldShowUnsetNodeTypeOption: identity.shouldShowUnsetNodeTypeOption },
        { shouldShowMergeWithPrimarySiblingBranchOption: identity.shouldShowMergeWithPrimarySiblingBranchOption },
    ];
};

export const debugNodeProperties = (node: ConvoNode) => {
    return [
        { isRoot: node.isRoot },
        { nodeId: node.nodeId },
        { nodeType: node.nodeType },
        { text: node.text },
        { isCritical: node.isCritical },
        { nodeChildrenString: node.nodeChildrenString },
        { isTerminalType: node.isTerminalType },
        { isMultiOptionType: node.isMultiOptionType },
        { shouldShowMultiOption: node.shouldShowMultiOption },
        { optionPath: node.optionPath },
        { valueOptions: node.valueOptions },
        { shouldRenderChildren: node.shouldRenderChildren },
        { isSplitMergeType: node.isSplitMergeType },
        { isAnabranchType: node.isAnabranchType },
        { isAnabranchMergePoint: node.isAnabranchMergePoint },
    ];
};
