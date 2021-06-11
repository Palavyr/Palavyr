import { Conversation, ConvoNode, NodeSetterWithHistory } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrLinkedList } from "../../../convoDataStructure/PalavyrLinkedList";
import { ConversationHistoryTracker } from "../../ConversationHistoryTracker";
import { _replaceNodeWithUpdatedNode } from "../../nodeUtils/_coreNodeUtils";

export const _showResponseInPdfCheckbox = (
    checked: boolean,
    node: ConvoNode,
    nodeList: Conversation,
    setNodes: NodeSetterWithHistory,
    conversationHistoryPosition: number,
    historyTracker: ConversationHistoryTracker,
    conversationHistory: PalavyrLinkedList[],
) => {
    if (checked) {
        const newNode = cloneDeep(node);
        newNode.isCritical = checked;
        const updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
        // setNodes(updatedNodeList);
    } else {
        historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
    }
};
