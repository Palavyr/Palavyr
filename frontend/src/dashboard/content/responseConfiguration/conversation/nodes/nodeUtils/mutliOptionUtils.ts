import { ConvoNode, Conversation, ValueOptionDelimiter } from "@Palavyr-Types";
import { createAndReattachNewNodes, getNewNumChildren, getNodeById } from "./commonNodeUtils";
import { _createNewChildIDs, _removeNodeByID, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

export const updateMultiTypeOption = (node: ConvoNode, nodeList: Conversation, valueOptions: string[], setNodes: (updatedNodeList: Conversation) => void) => {
    const optionPaths = valueOptions;

    const newNumChildren = getNewNumChildren(optionPaths);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

    node.nodeChildrenString = newChildNodeIds.join(",");
    node.valueOptions = valueOptions.join(ValueOptionDelimiter); // TODO: get this seperator from server
    node.isMultiOptionType = true;
    node.isTerminalType = false;
    node.isSplitMergeType = false;

    // add updated node to nodelist
    const filteredNodeList = _removeNodeByID(node.nodeId, newNodeList);
    filteredNodeList.push(node);

    delete node.id;

    childIdsToCreate.forEach((id: string, index: number) => {
        let newNode: ConvoNode = {
            nodeId: id, // replace with uuid
            nodeType: "", // default
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            fallback: false,
            isCritical: false,
            areaIdentifier: node.areaIdentifier,
            optionPath: optionPaths[index],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: true,
        };

        filteredNodeList.push(newNode);
    });

    setNodes(filteredNodeList);
};
