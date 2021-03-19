import { Conversation, ConvoNode } from "@Palavyr-Types";
import { collectAnabranchMeta, otherNodeAlreadySetAsAnabranchMerge } from "./AnabranchUtils";
import { determineIfCanUnsetNodeType, nodeMergesToPrimarySibling } from "./commonNodeUtils";
import { collectSplitMergeMeta } from "./splitMergeUtils";

export type NodeId = string;

export type NodeIdentity = {
    // splitmerge
    isDecendentOfSplitMerge: boolean;
    decendentLevelFromSplitMerge: number;
    splitMergeRootSiblingIndex: number;
    nodeIdOfMostRecentSplitMergePrimarySibling: NodeId;
    canShowMergeWithPrimarySiblingBranchOption: boolean;
    canShowSplitMergePrimarySiblingLabel: boolean;
    shouldCheckSplitMergeBox: boolean;

    // anabranch
    isAnabranchMergePoint: boolean;
    isDecendentOfAnabranch: boolean;
    decendentLevelFromAnabranch: number;
    nodeIdOfMostRecentAnabranch: NodeId;
    isDirectChildOfAnabranch: boolean;
    isParentOfAnabranchMergePoint: boolean;
    isAncestorOfAnabranchMergePoint: boolean;
    canShowSetAsAnabranchMergePointOption: boolean;
    otherNodeAlreadySetAsMergeBranchBool: boolean;

    //
    shouldDisabledNodeTypeSelector: boolean;
    canUnSetNodeType: boolean;
    canShowResponseInPdfOption: boolean;
    shouldShowUnsetNodeTypeOption: boolean;


};

export const getNodeIdentity = (node: ConvoNode, nodeList: Conversation): NodeIdentity => {
    const {
        isDecendentOfSplitMerge,
        decendentLevelFromSplitMerge,
        splitMergeRootSiblingIndex,
        nodeIdOfMostRecentSplitMergePrimarySibling,
        orderedChildren
    } = collectSplitMergeMeta(node, nodeList);

    const {
        isDecendentOfAnabranch,
        decendentLevelFromAnabranch,
        nodeIdOfMostRecentAnabranch,
        isDirectChildOfAnabranch,
        isParentOfAnabranchMergePoint,
        isAncestorOfAnabranchMergePoint
    } = collectAnabranchMeta(node, nodeList);

    /*
    * boolean - Is another node already set as the anabranch merge point for this subtree. Searches from most recent anabranch node id down to all leaf nodes
    */
    const otherNodeAlreadySetAsMergeBranchBool = isDecendentOfAnabranch && otherNodeAlreadySetAsAnabranchMerge(nodeIdOfMostRecentAnabranch, nodeList, node.nodeId);

    /*
    * boolean - Should node type selector be disabled. Disable selector once anabranch boundaries are set.
    */
    const shouldDisabledNodeTypeSelector = (isDecendentOfAnabranch && isAncestorOfAnabranchMergePoint) || (isAncestorOfAnabranchMergePoint && node.isAnabranchType);


    /*
    * boolean - for if a node is set and none of its children are set, then the node can be unset. Currently the type selector does not clear bc material UI.
    */
    const canUnSetNodeType = determineIfCanUnsetNodeType(node, nodeList, isDecendentOfAnabranch, nodeIdOfMostRecentAnabranch);
    /*
    * boolean - should node show the button to unset node type.
    */
    const shouldShowUnsetNodeTypeOption = canUnSetNodeType && node.nodeType !== "" && !node.isAnabranchType && !node.isAnabranchMergePoint && !node.isSplitMergeType;


    /*
    * booelan - for node to show the 'include in pdf response' check box. Yes if this is a node type that elicits a response.
    */
    const canShowResponseInPdfOption = !node.isTerminalType && !(node.nodeType === "ProvideInfo");

    /*
    * boolean - Should node be able to set child node as the primary sibling branch - should show merge check box
    */
    const canShowMergeWithPrimarySiblingBranchOption = isDecendentOfSplitMerge && splitMergeRootSiblingIndex > 0 && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType;

    /*
    * boolean - Should node be able be an anabranch merge point. Node cannot have siblings
    */
    const canShowSetAsAnabranchMergePointOption = isDecendentOfAnabranch && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType && !isDirectChildOfAnabranch && !otherNodeAlreadySetAsMergeBranchBool;


    /*
    * boolean - can show the text in the node that says 'im the primary sibling
    */
    const canShowSplitMergePrimarySiblingLabel = isDecendentOfSplitMerge && splitMergeRootSiblingIndex === 0 && decendentLevelFromSplitMerge === 1;

    /*
    * boolean - should check the split merge box
    */
    const shouldCheckSplitMergeBox = nodeMergesToPrimarySibling(node, isDecendentOfSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling)

    /*
    * boolean - is the most recent anabranch merge point
    */
    const isAnabranchMergePoint = node.isAnabranchMergePoint;

    return {
        isDecendentOfSplitMerge,
        decendentLevelFromSplitMerge,
        splitMergeRootSiblingIndex,
        nodeIdOfMostRecentSplitMergePrimarySibling,
        isDecendentOfAnabranch,
        decendentLevelFromAnabranch,
        nodeIdOfMostRecentAnabranch,
        isDirectChildOfAnabranch,
        isParentOfAnabranchMergePoint,
        isAncestorOfAnabranchMergePoint,
        otherNodeAlreadySetAsMergeBranchBool,
        shouldDisabledNodeTypeSelector,
        canUnSetNodeType,
        canShowResponseInPdfOption,
        canShowMergeWithPrimarySiblingBranchOption,
        canShowSetAsAnabranchMergePointOption,
        shouldShowUnsetNodeTypeOption,
        canShowSplitMergePrimarySiblingLabel,
        shouldCheckSplitMergeBox,
        isAnabranchMergePoint
    }
};
