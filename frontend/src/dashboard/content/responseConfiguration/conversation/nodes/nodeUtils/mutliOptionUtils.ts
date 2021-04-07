import { ConvoNode, Conversation, ValueOptionDelimiter } from "@Palavyr-Types";
import { createAndReattachNewNodes, getNewNumChildren } from "./commonNodeUtils";
import { _createAndAddNewNodes, _createNewChildIDs, _removeNodeByID, _resetOptionPaths, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

export const updateMultiTypeOption = (node: ConvoNode, nodeList: Conversation, valueOptions: string[], setNodes: (updatedNodeList: Conversation) => void) => {
    let optionPaths: string[];
    if (node.nodeComponentType === "MultipleChoiceContinue") {
        optionPaths = ["Continue"];
    } else {
        optionPaths = valueOptions;
    }

    const newNumChildren = getNewNumChildren(optionPaths);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

    const previousNodeChildrenString = node.nodeChildrenString;
    node.nodeChildrenString = newChildNodeIds.join(",");
    node.valueOptions = valueOptions.join(ValueOptionDelimiter);
    node.isMultiOptionType = true;
    node.isTerminalType = false;

    // add updated node to nodelist
    let updatedNodeList = _removeNodeByID(node.nodeId, newNodeList);
    delete node.id;
    updatedNodeList.push(node);
    updatedNodeList =_createAndAddNewNodes(childIdsToCreate, newChildNodeIds, node.areaIdentifier, optionPaths, updatedNodeList, true, false);

    // need to call _resetNodePathOptions I think
    const previousChildren = previousNodeChildrenString.split(",");
    updatedNodeList = _resetOptionPaths(previousChildren, updatedNodeList, optionPaths);


    setNodes(updatedNodeList);
};
