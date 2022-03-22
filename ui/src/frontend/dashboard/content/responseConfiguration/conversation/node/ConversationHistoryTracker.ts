import { IPalavyrLinkedList } from "@Palavyr-Types";
import { IConversationHistoryTracker, NodeTypeOptions, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";

export class ConversationHistoryTracker implements IConversationHistoryTracker {
    private maxConversationHistory: number = 50; // the number of times you can hit the back button
    private currentPosition: number = 0;
    private conversationHistory: IPalavyrLinkedList[] = [];

    private setLinkedNodes: React.Dispatch<React.SetStateAction<IPalavyrLinkedList | undefined>>;

    public linkedNodeList: IPalavyrLinkedList;
    public nodeTypeOptions: NodeTypeOptions;

    constructor(setLinkedNodes: SetState<IPalavyrLinkedList>, linkedNodeList: IPalavyrLinkedList | undefined, nodeTypeOptions: NodeTypeOptions) {
        this.setLinkedNodes = setLinkedNodes;
        this.nodeTypeOptions = nodeTypeOptions;
        if (linkedNodeList) {
            this.linkedNodeList = linkedNodeList;
        }
    }

    private setConversationHistory = (newHistory: IPalavyrLinkedList[]) => {
        this.conversationHistory = newHistory;
    };

    public initializeConversation(conversation: IPalavyrLinkedList) {
        this.currentPosition = 0;
        this.setConversationHistory([cloneDeep(conversation)]);
        this.resetLinkedNodes(conversation);
    }

    public addConversationHistoryToQueue(dirtyConversationRecord: IPalavyrLinkedList) {
        this.currentPosition += 1;

        const newConversationRecord = dirtyConversationRecord;

        let newHistory: IPalavyrLinkedList[];
        if (this.conversationHistory.length < this.maxConversationHistory) {
            if (this.currentPosition < this.conversationHistory.length - 1) {
                newHistory = [...this.conversationHistory.slice(0, this.currentPosition), newConversationRecord];
                this.setConversationHistory(newHistory);
            } else {
                newHistory = [...this.conversationHistory, newConversationRecord];
                this.setConversationHistory(newHistory);
            }
        } else {
            if (this.currentPosition < this.maxConversationHistory) {
                newHistory = [...this.conversationHistory.slice(0, this.currentPosition), newConversationRecord];
                this.setConversationHistory(newHistory);
            } else {
                newHistory = [...this.conversationHistory.slice(1), newConversationRecord];
                this.setConversationHistory(newHistory);
            }
        }
        this.resetLinkedNodes(newConversationRecord);
    }

    public stepConversationBackOneStep() {
        if (this.currentPosition === 0) {
            alert("Currently at the beginning the history.");
            return;
        }
        this.currentPosition -= 1;
        // console.log(`Back Current Position: ${this.currentPosition}`);
        this.refreshConversation();
    }

    public stepConversationForwardOneStep() {
        if (this.currentPosition === this.conversationHistory.length - 1) {
            alert("This is the latest version.");
            return;
        }

        this.currentPosition += 1;
        // console.log(`Forward Current Position: ${this.currentPosition}`);
        this.refreshConversation();
    }

    public refreshConversation() {
        // console.log("Refreshing Conversation");
        const linkedNodeList = this.conversationHistory[this.currentPosition];
        this.resetLinkedNodes(linkedNodeList);
    }

    private resetLinkedNodes(linkedNodeList: IPalavyrLinkedList) {
        this.linkedNodeList = linkedNodeList;
        this.setLinkedNodes(cloneDeep(linkedNodeList));
    }
}
