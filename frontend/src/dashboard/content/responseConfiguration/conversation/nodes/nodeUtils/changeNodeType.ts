import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, Conversation, NodeOption, NodeIdentity } from "@Palavyr-Types";
import { Dispatch, SetStateAction } from "react";
import { getNewNumChildren, createAndReattachNewNodes } from "./commonNodeUtils";
import { recursivelyDereferenceNodeIdFromChildren } from "./dereferenceUtils";
import { getPrimarySiblingIdFromChildNodeChildrenString } from "./splitMergeUtils";
import { _splitAndRemoveEmptyNodeChildrenString, _getNodeById, _replaceNodeWithUpdatedNode, _createAndAddNewNodes, _resetOptionPaths, _joinValueOptionArray, _splitValueOptionString } from "./_coreNodeUtils";

const AnabranchName = "Anabranch"; // TODO: Get this from the server as nodeOption.stringName

// if we switch to an anavbranch or a splitmerge, and we are inside an anabranch, then we need to set this node AS
// the anabranch merge point.

export const changeNodeType = async (previousNode: ConvoNode, nodeList: Conversation, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption, identity: NodeIdentity, selectionCallback: (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string) => Conversation) => {
    let valueOptions = previousNode.valueOptions; // if valueOptions is "", its because it was from a non-multioptionType
    if (nodeOption.isMultiOptionType && nodeOption.valueOptions.length > 0) {
        valueOptions = _joinValueOptionArray(nodeOption.valueOptions);
    } else if (nodeOption.isTerminalType) {
        valueOptions = "";
    } else if (isNullOrUndefinedOrWhitespace(valueOptions)) {
        if (nodeOption.value === AnabranchName) { //TODO: Why is nodeOption.stringName null?
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

    // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
    // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
    // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
    previousNode.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s

    const newNumChildren = getNewNumChildren(pathOptions);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(previousNode, nodeList, newNumChildren);

    let updatedNodeList = [...newNodeList];

    const previousNodeChildrenString = previousNode.nodeChildrenString;
    previousNode.nodeChildrenString = newChildNodeIds.join(",");
    previousNode.isMultiOptionType = nodeOption.isMultiOptionType;
    previousNode.isTerminalType = nodeOption.isTerminalType;
    previousNode.isSplitMergeType = nodeOption.isSplitMergeType;
    previousNode.shouldShowMultiOption = nodeOption.shouldShowMultiOption;
    previousNode.isAnabranchType = nodeOption.isAnabranchType;

    if (identity.shouldShowSetAsAnabranchMergePointOption && nodeOption.isAnabranchType){
        previousNode.isAnabranchMergePoint = true; // needs to set true if inside anabranch and
        updatedNodeList = selectionCallback(previousNode, updatedNodeList, identity.nodeIdOfMostRecentAnabranch)
    } else {
        previousNode.isAnabranchMergePoint = nodeOption.isAnabranchMergePoint
    }

    // set any value options
    previousNode.valueOptions = valueOptions;
    updatedNodeList = _replaceNodeWithUpdatedNode(previousNode, updatedNodeList);
    updatedNodeList = _createAndAddNewNodes(childIdsToCreate, newChildNodeIds, previousNode.areaIdentifier, pathOptions, updatedNodeList, nodeOption.shouldShowMultiOption);

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
