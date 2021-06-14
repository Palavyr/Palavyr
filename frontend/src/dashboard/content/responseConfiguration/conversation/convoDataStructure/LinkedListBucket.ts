import { ConvoNode } from "@Palavyr-Types";
import { PalavyrNode } from "./PalavyrNode";

export interface ILinkedListBucket {
    addToBucket(node: PalavyrNode): void;
    convertToConvoNodes(areaId: string): ConvoNode[];
}

export class LinkedListBucket implements ILinkedListBucket {
    public linkedListBucket: PalavyrNode[] = [];

    constructor(linkedList?: PalavyrNode[]) {
        if (linkedList) {
            this.linkedListBucket;
        }
    }

    public addToBucket(node: PalavyrNode) {
        if (!this.alreadyPresent(node)) {
            this.linkedListBucket.push(node);
        }
    }

    private alreadyPresent(node: PalavyrNode) {
        return this.linkedListBucket.filter((x) => x.nodeId === node.nodeId).length > 0;
    }

    public convertToConvoNodes(areaId: string) {
        const convoBucket: ConvoNode[] = [];
        this.linkedListBucket.forEach((x) => {
            convoBucket.push(x.compileConvoNode(areaId));
        });
        return convoBucket;
    }

    public clear() {
        this.linkedListBucket = [];
    }

    public findById(nodeId: string) {
        const node = this.linkedListBucket.filter((x: PalavyrNode) => x.nodeId === nodeId);
        if (node.length === 0) throw new Error("Attempting to find a node that does not exist in the tree");
        if (node.length > 1) throw new Error("Attempting to find a node that was somehow duplicated.");
        return node[0];
    }
}
