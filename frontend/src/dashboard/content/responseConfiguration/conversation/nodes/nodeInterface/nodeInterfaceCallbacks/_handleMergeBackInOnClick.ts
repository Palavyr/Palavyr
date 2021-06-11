import { ConvoNode, Conversation, NodeSetterWithHistory } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { uuid } from "uuidv4";
import { PalavyrLinkedList } from "../../../convoDataStructure/PalavyrLinkedList";
import { ConversationHistoryTracker } from "../../ConversationHistoryTracker";
import { updateSingleOptionType } from "../../nodeUtils/commonNodeUtils";
import { _truncateTheTreeAtSpecificNode, _replaceNodeWithUpdatedNode, _createAndAddNewNodes } from "../../nodeUtils/_coreNodeUtils";

export const _handleMergeBackInOnClick = (
    checked: boolean,
    node: ConvoNode,
    nodeList: Conversation,
    conversationHistoryPosition: number,
    historyTracker: ConversationHistoryTracker,
    conversationHistory: PalavyrLinkedList[],
    setNodes: NodeSetterWithHistory,
    setMergeBoxChecked: React.Dispatch<React.SetStateAction<boolean>>,
    mostRecentSplitMergePrimarySiblingNodeId: string
) => {

    if (checked) {
        const newNode = cloneDeep(node);
        let updatedNodeList = cloneDeep(nodeList);
        newNode.shouldRenderChildren = false;
        updatedNodeList = _truncateTheTreeAtSpecificNode(node, nodeList);
        newNode.nodeChildrenString = mostRecentSplitMergePrimarySiblingNodeId;
        updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
        setMergeBoxChecked(checked);
        // setNodes(cloneDeep(updatedNodeList));
    } else {
        if (conversationHistoryPosition === 0) {
            const childId = uuid();
            const newNode = cloneDeep(node);
            newNode.shouldRenderChildren = true;
            newNode.nodeChildrenString = childId;
            let updatedNodeList = _createAndAddNewNodes([childId], [childId], node.areaIdentifier, ["Continue"], nodeList, false, false);
            // updateSingleOptionType(newNode, updatedNodeList, setNodes);
            setMergeBoxChecked(false);
        } else {
            historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
        }
    }
}