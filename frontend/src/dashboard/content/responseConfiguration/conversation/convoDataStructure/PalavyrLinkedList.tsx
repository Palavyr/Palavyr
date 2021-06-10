import React from 'react'

import { ConvoNode } from "@Palavyr-Types";
import { getRootNode } from "../nodes/nodeUtils/commonNodeUtils";
import { _getNodeById, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "../nodes/nodeUtils/_coreNodeUtils";
import { LinkedListBucket } from "./LinkedListBucket";
import { PalavyrNode } from "./PalavyrNode";


interface IPalavyrLinkedList {
    traverse(): void;
    insert(): void;
    delete(): void;
    compile(): ConvoNode[];
}

export class PalavyrLinkedList implements IPalavyrLinkedList {
    private linkedListBucket: LinkedListBucket = new LinkedListBucket();
    private areaId: string; // areaIdentifier
    public rootNode: PalavyrNode;
    private head: ConvoNode;
    private rerender: () => void;

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], areaId: string, rerender: () => void) {
        this.areaId = areaId;
        this.head = getRootNode(nodeList);
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList);
    }

    private assembleDoubleLinkedMultiBranchLinkedList(nodeList: ConvoNode[]) {
        const headNode = new PalavyrNode(this.head, nodeList, this.rerender, true);
        this.rootNode = headNode;
        this.linkedListBucket.addToBucket(headNode);
        this.recursivelyAssembleLinkedList(headNode, this.head.nodeChildrenString, nodeList);
    }

    private recursivelyAssembleLinkedList(parentNode: PalavyrNode, nodeChildrenString: string, nodeList: ConvoNode[]) {
        const childIds = _splitAndRemoveEmptyNodeChildrenString(nodeChildrenString);
        if (childIds.length === 0) return;
        for (let index = 0; index < childIds.length; index++) {
            // childnodes add themselve to their parent node reference
            const childId = childIds[index];

            const childConvoNode = _getNodeById(childId, nodeList);
            const newNode = new PalavyrNode(childConvoNode, nodeList, this.rerender, index === 0);

            // double linked
            parentNode.childNodeReferences.addReference(newNode);
            newNode.parentNodeReferences.addReference(parentNode);

            this.linkedListBucket.addToBucket(newNode); // I think this works since we rebuild the list on every render. Otherwise the compile method needs to call a method that will traverse the list recursively

            this.recursivelyAssembleLinkedList(newNode, childConvoNode.nodeChildrenString, nodeList);
        }
    }

    compile(): ConvoNode[] {
        return this.linkedListBucket.convertToConvoNodes(this.areaId); // or perhaps we recursively traverse and hit all nodes, adding them to the bucket....
    }

    renderNodeTree() {
        // ideally this will execute a recursive function that will traverse the tree recursively (to account for all branches)
        // keep track of all the node Ids visited globally scoped, and then return a node object to the JSX with all of copies of the rendered
        // nodes.

        return this.rootNode.renderPalavyrNode();

    }


    traverse(): void {
        throw new Error("Method not implemented.");
    }
    insert(): void {
        throw new Error("Method not implemented.");
    }
    delete(): void {
        throw new Error("Method not implemented.");
    }
}
