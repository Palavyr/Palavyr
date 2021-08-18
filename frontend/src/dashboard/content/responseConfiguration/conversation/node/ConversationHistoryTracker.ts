import { IConversationHistoryTracker, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrLinkedList } from "../PalavyrDataStructure/PalavyrLinkedList";

export type SetConversationHistory = SetState<PalavyrLinkedList[]>;
export type SetConversation = SetState<PalavyrLinkedList>;
export type SetConversationHistoryPosition = SetState<number>;

export class ConversationHistoryTracker implements IConversationHistoryTracker {
    private MaxConversationHistory = 50; // the number of times you can hit the back button

    setConversationHistory: SetConversationHistory;
    setConversationHistoryPosition: SetConversationHistoryPosition;
    setNodes: SetConversation;

    constructor(onSetHistory: SetConversationHistory, onSetHistoryPosition: SetConversationHistoryPosition, onSetNodes: SetConversation) {
        this.setConversationHistory = onSetHistory;
        this.setConversationHistoryPosition = onSetHistoryPosition;
        this.setNodes = onSetNodes;
    }

    // Maybe can set the historyPosition as internal state too.
    addConversationHistoryToQueue(dirtyConversationRecord: PalavyrLinkedList, conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]) {
        const newPos = conversationHistoryPosition + 1;
        const newConversationRecord = dirtyConversationRecord;
        if (conversationHistory.length < this.MaxConversationHistory) {
            if (newPos < conversationHistory.length - 1) {
                this.setConversationHistory([...conversationHistory.slice(0, newPos), newConversationRecord]);
            } else {
                this.setConversationHistory([...conversationHistory, newConversationRecord]);
            }
        } else {
            if (newPos < this.MaxConversationHistory) {
                this.setConversationHistory([...conversationHistory.slice(0, newPos), newConversationRecord]);
            } else {
                this.setConversationHistory([...conversationHistory.slice(1), newConversationRecord]);
            }
        }

        this.setConversationHistoryPosition(newPos);
    }

    stepConversationBackOneStep(conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]) {
        if (conversationHistoryPosition === 0) {
            alert("Currently at the beginning the history.");
            return;
        }
        const newPosition = conversationHistoryPosition - 1;
        this.setConversationHistoryPosition(newPosition);
        const oneBack = conversationHistory[newPosition];
        this.setNodes(cloneDeep(oneBack));
    }

    stepConversationForwardOneStep(conversationHistoryPosition: number, conversationHistory: PalavyrLinkedList[]) {
        const newPosition = conversationHistoryPosition + 1;
        if (newPosition <= conversationHistory.length - 1) {
            this.setNodes(cloneDeep(conversationHistory[newPosition]));
            this.setConversationHistoryPosition(newPosition);
        } else {
            alert("Currently at the end of the history.");
        }
    }
}
