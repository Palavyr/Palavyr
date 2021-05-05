import { Conversation } from "@Palavyr-Types";
import { Dispatch, SetStateAction } from "react";

type SetConversationHistory = Dispatch<SetStateAction<Conversation[]>>
type SetConversation = Dispatch<SetStateAction<Conversation>>
type SetConversationHistoryPosition = Dispatch<SetStateAction<number>>;

export class ConversationHistoryTracker {

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
    addConversationHistoryToQueue(newConversationRecord: Conversation, conversationHistoryPosition: number, conversationHistory: Conversation[]) {
        const newPos = conversationHistoryPosition + 1;

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
    };

    stepConversationBackOneStep(conversationHistoryPosition: number, conversationHistory: Conversation[]) {
        if (conversationHistoryPosition === 0) {
            alert("Currently at the beginning the history.");
            return;
        }
        const newPosition = conversationHistoryPosition - 1;
        this.setConversationHistoryPosition(newPosition);
        this.setNodes(conversationHistory[newPosition]);
    };

    stepConversationForwardOneStep(conversationHistoryPosition: number, conversationHistory: Conversation[]) {
        const newPosition = conversationHistoryPosition + 1;
        if (newPosition <= conversationHistory.length - 1) {
            this.setNodes(conversationHistory[newPosition]);
            this.setConversationHistoryPosition(newPosition);
        } else {
            alert("Currently at the end of the history.");
        }
    };
}