import { ConversationDesignerNodeResource } from "@Palavyr-Types";
import { ILinkedListBucket, IPalavyrNode } from "@Palavyr-Types";

export class LinkedListBucket implements ILinkedListBucket {
    public linkedListBucket: IPalavyrNode[] = [];

    constructor(linkedList?: IPalavyrNode[]) {
        if (linkedList) {
            this.linkedListBucket;
        }
    }

    public addToBucket(node: IPalavyrNode) {
        if (!this.alreadyPresent(node)) {
            this.linkedListBucket.push(node);
        }
    }

    private alreadyPresent(node: IPalavyrNode) {
        return this.linkedListBucket.filter((x) => x.nodeId === node.nodeId).length > 0;
    }

    public convertToConvoNodes(IntentId: string) {
        const convoBucket: ConversationDesignerNodeResource[] = [];
        this.linkedListBucket.forEach((x: IPalavyrNode) => {
            convoBucket.push(x.compileConvoNode(IntentId));
        });
        return convoBucket;
    }

    public clear() {
        this.linkedListBucket = [];
    }

    public findById(nodeId: string) {
        const node = this.linkedListBucket.filter((x: IPalavyrNode) => x.nodeId === nodeId);
        if (node.length === 0) return null;//throw new Error("Attempting to find a node that does not exist in the tree");
        if (node.length > 1) throw new Error("Attempting to find a node that was somehow duplicated.");
        return node[0];
    }
}

