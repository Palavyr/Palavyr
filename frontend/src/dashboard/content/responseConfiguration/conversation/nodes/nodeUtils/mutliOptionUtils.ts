import { ConvoNode, Conversation, ValueOptionDelimiter } from "@Palavyr-Types";
import { createAndReattachNewNodes, getNewNumChildren } from "./commonNodeUtils";
import { _createAndAddNewNodes, _createNewChildIDs, _removeNodeByID, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

export const updateMultiTypeOption = (node: ConvoNode, nodeList: Conversation, valueOptions: string[], setNodes: (updatedNodeList: Conversation) => void) => {
    let optionPaths: string[];
    if (node.nodeType === "MultipleChoiceContinue") {
        optionPaths = ["Continue"];
    } else {
        optionPaths = valueOptions;
    }

    const newNumChildren = getNewNumChildren(optionPaths);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

    node.nodeChildrenString = newChildNodeIds.join(",");
    node.valueOptions = valueOptions.join(ValueOptionDelimiter); // TODO: get this seperator from server
    node.isMultiOptionType = true;
    node.isTerminalType = false;

    // add updated node to nodelist
    let updatedNodeList = _removeNodeByID(node.nodeId, newNodeList);
    delete node.id;
    updatedNodeList.push(node);
    updatedNodeList =_createAndAddNewNodes(childIdsToCreate, newChildNodeIds, node, optionPaths, updatedNodeList);

    // childIdsToCreate.forEach((id: string, index: number) => {
    //     let newNode: ConvoNode = {
    //         nodeId: id, // replace with uuid
    //         nodeType: "", // default
    //         text: "Ask your question!",
    //         nodeChildrenString: "",
    //         isRoot: false,
    //         fallback: false,
    //         isCritical: false,
    //         areaIdentifier: node.areaIdentifier,
    //         optionPath: optionPaths[index],
    //         valueOptions: "",
    //         isMultiOptionType: false,
    //         isTerminalType: false,
    //         isSplitMergeType: false,
    //         shouldRenderChildren: true,
    //     };

    //     filteredNodeList.push(newNode);
    // });

    setNodes(updatedNodeList);
};
