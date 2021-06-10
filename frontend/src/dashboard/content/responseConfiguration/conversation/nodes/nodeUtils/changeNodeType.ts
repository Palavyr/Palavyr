import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, Conversation, NodeOption, NodeIdentity } from "@Palavyr-Types";
import { getNewNumChildren, createAndReattachNewNodes } from "./commonNodeUtils";
import { recursivelyDereferenceNodeIdFromChildren } from "./dereferenceUtils";
import { getPrimarySiblingIdFromChildNodeChildrenString } from "./splitMergeUtils";
import { _splitAndRemoveEmptyNodeChildrenString, _getNodeById, _replaceNodeWithUpdatedNode, _createAndAddNewNodes, _resetOptionPaths, _joinValueOptionArray, _splitValueOptionString, _joinNodeChildrenStringArray } from "./_coreNodeUtils";

const AnabranchName = "Anabranch"; // TODO: Get this from the server as nodeOption.stringName

// if we switch to an anabranch or a splitmerge, and we are inside an anabranch, then we need to set this node AS
// the anabranch merge point.

export const changeNodeType = async (
    previousNode: ConvoNode,
    nodeList: Conversation,
    setNodes: (nodeList: Conversation) => void,
    nodeOption: NodeOption,
    identity: NodeIdentity,
    selectionCallback: (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string) => Conversation
) => {
    let valueOptions = previousNode.valueOptions; // if valueOptions is "", its because it was from a non-multioptionType
    if (nodeOption.isMultiOptionType && nodeOption.valueOptions.length > 0) {
        valueOptions = _joinValueOptionArray(nodeOption.valueOptions);
    } else if (nodeOption.isTerminalType) {
        valueOptions = "";
    } else if (nodeOption.isDynamicType) {
        valueOptions = _joinValueOptionArray(nodeOption.valueOptions);
    } else if (isNullOrUndefinedOrWhitespace(valueOptions)) {
        if (nodeOption.value === AnabranchName) {
            //TODO: Why is nodeOption.stringName null?
            valueOptions = _joinValueOptionArray(["Left Branch", "Right Branch"]);
        } else {
            valueOptions = previousNode.isTerminalType ? "" : "Placeholder";
        }
    }

    let pathOptions: string[];
    if (nodeOption.isTerminalType) {
        pathOptions = [""];
    } else if (nodeOption.isMultiOptionType) {
        if (nodeOption.pathOptions.length >= 1) {
            pathOptions = nodeOption.pathOptions; // if its a predefined yes or no
        } else {
            // use the previous node and attach - but if we've set to placeholder, use that instead
            const currentPathOptions = _splitValueOptionString(previousNode.valueOptions);
            if (currentPathOptions.filter((option: string) => !isNullOrUndefinedOrWhitespace(option)).length >= 1) {
                pathOptions = currentPathOptions;
            } else {
                pathOptions = _splitValueOptionString(valueOptions);
            }
        }
    } else {
        pathOptions = ["Continue"];
    }

    if (pathOptions === undefined) {
        throw new Error("Ill defined path options");
    }
    if (valueOptions === undefined) {
        throw new Error("Ill defined value options - cannot be undefined");
    }

    if (previousNode.nodeType.toUpperCase() === "SplitMerge".toUpperCase() && nodeOption.stringName?.toUpperCase() !== "SplitMerge".toUpperCase()) {
        const nonPrimarySiblingNodeChildren = _splitAndRemoveEmptyNodeChildrenString(previousNode.nodeChildrenString).slice(1);
        const primarySiblingNodeId = getPrimarySiblingIdFromChildNodeChildrenString(previousNode);
        if (nonPrimarySiblingNodeChildren.length > 0) {
            nonPrimarySiblingNodeChildren.forEach((id: string) => {
                const nonPrimaryChildNode = _getNodeById(id, nodeList);
                recursivelyDereferenceNodeIdFromChildren(nonPrimaryChildNode, nodeList, primarySiblingNodeId); // walks the tree down the non-primary nodes and removes childNodeString references to primarySiblingNodeId
            });
        }
    }

    const newNumChildren = getNewNumChildren(pathOptions);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(previousNode, nodeList, newNumChildren);
    let updatedNodeList = [...newNodeList];
    const previousNodeChildrenString = previousNode.nodeChildrenString;
    const previousText = previousNode.text;

    previousNode.nodeChildrenString = _joinNodeChildrenStringArray(newChildNodeIds);

    const nodeOptionKeys = Object.keys(nodeOption);
    nodeOptionKeys.forEach((key: string) => {
        previousNode[key] = nodeOption[key];
    })

    // override specific properties
    previousNode.text = previousText;
    previousNode.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s
    if (identity.shouldShowSetAsAnabranchMergePointOption && nodeOption.isAnabranchType) {
        previousNode.isAnabranchMergePoint = true; // needs to set true if inside anabranch and
        updatedNodeList = selectionCallback(previousNode, updatedNodeList, identity.nodeIdOfMostRecentAnabranch);
    }

    // set any value options
    previousNode.valueOptions = valueOptions;
    updatedNodeList = _replaceNodeWithUpdatedNode(previousNode, updatedNodeList);
    updatedNodeList = _createAndAddNewNodes(childIdsToCreate, newChildNodeIds, previousNode.areaIdentifier, pathOptions, updatedNodeList, nodeOption.shouldShowMultiOption, nodeOption.isDynamicType);

    if (newChildNodeIds.length > 0) {
        updatedNodeList = _resetOptionPaths(newChildNodeIds, updatedNodeList, pathOptions);
    } else {
        const previousChildren = previousNodeChildrenString.split(",");
        if (previousChildren.length === 1 && previousChildren[0] === "") {
            updatedNodeList = updatedNodeList;
        } else {
            updatedNodeList = _resetOptionPaths(previousChildren, updatedNodeList, pathOptions);
        }
    }
    setNodes(updatedNodeList);
};
