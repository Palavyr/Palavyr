import { Conversation, ConvoNode, NodeIdentity } from "@Palavyr-Types";
import { collectAnabranchMeta, otherNodeAlreadySetAsAnabranchMerge } from "./AnabranchUtils";
import { determineIfCanUnsetNodeType, determineIfIsOnLeftmostBranchGivenAnOriginNode, nodeMergesToPrimarySibling } from "./commonNodeUtils";
import { collectSplitMergeMeta } from "./splitMergeUtils";


export const getNodeIdentity = (node: ConvoNode, nodeList: Conversation): NodeIdentity => {
    const { isDecendentOfSplitMerge, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling } = collectSplitMergeMeta(node, nodeList);

    const { isDecendentOfAnabranch, decendentLevelFromAnabranch, nodeIdOfMostRecentAnabranch, isDirectChildOfAnabranch, isParentOfAnabranchMergePoint, isAncestorOfAnabranchMergePoint } = collectAnabranchMeta(node, nodeList);

    /*
     * boolean - Is another node already set as the anabranch merge point for this subtree. Searches from most recent anabranch node id down to all leaf nodes
     */
    const otherNodeAlreadySetAsMergeBranchBool = isDecendentOfAnabranch && otherNodeAlreadySetAsAnabranchMerge(nodeIdOfMostRecentAnabranch, nodeList, node.nodeId);

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
    const shouldShowResponseInPdfOption = !node.isTerminalType && !(node.nodeType === "ProvideInfo");

    /*
     * boolean - Should node be able to set child node as the primary sibling branch - should show merge check box
     */
    const shouldShowMergeWithPrimarySiblingBranchOption = isDecendentOfSplitMerge && splitMergeRootSiblingIndex > 0 && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType;

    /*
     * boolean - is the node on the left most branch from an origin node Id
     */
    const isOnLeftmostAnabranchBranch = nodeIdOfMostRecentAnabranch && determineIfIsOnLeftmostBranchGivenAnOriginNode(node.nodeId, nodeList, nodeIdOfMostRecentAnabranch);

    /*
     * boolean - Should node be able be an anabranch merge point. Node cannot have siblings
     */
    const shouldShowSetAsAnabranchMergePointOption =
        isDecendentOfAnabranch && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType && !isDirectChildOfAnabranch && !otherNodeAlreadySetAsMergeBranchBool && isOnLeftmostAnabranchBranch && decendentLevelFromAnabranch < 4;

    /*
     * boolean - Should node type selector be disabled. Disable selector once anabranch boundaries are set. Also disable if the tree depth goes beyond 3 below an anabranch point or mergetype
     */
    const shouldDisabledNodeTypeSelector =
        (isDecendentOfAnabranch && isAncestorOfAnabranchMergePoint) ||
        (isAncestorOfAnabranchMergePoint && node.isAnabranchType) ||
        (!isDecendentOfSplitMerge && isDecendentOfAnabranch && !isOnLeftmostAnabranchBranch && decendentLevelFromAnabranch > 2) ||
        (isDecendentOfSplitMerge && decendentLevelFromSplitMerge > 2 && splitMergeRootSiblingIndex > 0);

    /*
     * boolean - can show the text in the node that says 'im the primary sibling'
     */
    const shouldShowSplitMergePrimarySiblingLabel = isDecendentOfSplitMerge && splitMergeRootSiblingIndex === 0 && decendentLevelFromSplitMerge === 1;

    /*
     * boolean - should check the split merge box
     */
    const shouldCheckSplitMergeBox = nodeMergesToPrimarySibling(node, isDecendentOfSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling);

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
        shouldShowResponseInPdfOption,
        shouldShowMergeWithPrimarySiblingBranchOption,
        shouldShowSetAsAnabranchMergePointOption,
        shouldShowUnsetNodeTypeOption,
        shouldShowSplitMergePrimarySiblingLabel,
        shouldCheckSplitMergeBox,
        isAnabranchMergePoint,
        isOnLeftmostAnabranchBranch,
    };
};
