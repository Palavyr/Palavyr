import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import { getRootNode } from "../nodes/nodeUtils/commonNodeUtils";
import { _getNodeById, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "../nodes/nodeUtils/_coreNodeUtils";
import { LinkedListBucket } from "./LinkedListBucket";
import { PalavyrNode } from "./PalavyrNode";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrImageNode } from "./PalavyrImageNode";
import { PalavyrTextNode } from "./PalavyrTextNode";

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
    private rerender: () => null;
    private repository: PalavyrRepository = new PalavyrRepository();
    private nodeTypeOptions: NodeTypeOptions;

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], nodeTypeOptions: NodeTypeOptions, areaId: string, rerender: () => null) {
        this.areaId = areaId;
        this.head = getRootNode(nodeList);
        this.nodeTypeOptions = nodeTypeOptions;
        this.rerender = rerender;
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList);
    }

    private assembleDoubleLinkedMultiBranchLinkedList(nodeList: ConvoNode[]) {
        const headNode = this.convertToPalavyrNode(this, this.repository, this.head, nodeList, this.rerender, true);
        this.rootNode = headNode;
        this.linkedListBucket.addToBucket(headNode);
        this.recursivelyAssembleLinkedList(headNode, this.head.nodeChildrenString, nodeList);
    }

    private convertToPalavyrNode(container, repository, rawNode, nodeList, rerender, leftMostBranch) {
        let palavyrNode: PalavyrNode;
        switch (rawNode.isImageNode) {
            case true:
                palavyrNode = new PalavyrImageNode(container, this.nodeTypeOptions, repository, rawNode, nodeList, rerender, leftMostBranch);
                break;
            case false:
                palavyrNode = new PalavyrTextNode(container, this.nodeTypeOptions, repository, rawNode, nodeList, rerender, leftMostBranch);
                break;
            default:
                throw new Error("Node type couldn't be determined when construting the palavyr convo tree.");
        }
        return palavyrNode;
    }

    private recursivelyAssembleLinkedList(parentNode: PalavyrNode, nodeChildrenString: string, nodeList: ConvoNode[]) {
        const childIds = _splitAndRemoveEmptyNodeChildrenString(nodeChildrenString);
        if (childIds.length === 0) return;
        for (let index = 0; index < childIds.length; index++) {
            // childnodes add themselve to their parent node reference
            const childId = childIds[index];

            const childConvoNode = _getNodeById(childId, nodeList);
            const newNode = this.convertToPalavyrNode(this, this.repository, childConvoNode, nodeList, this.rerender, index === 0);

            // double linked
            parentNode.childNodeReferences.addReference(newNode);

            newNode.configure(parentNode);

            this.linkedListBucket.addToBucket(newNode); // I think this works since we rebuild the list on every render. Otherwise the compile method needs to call a method that will traverse the list recursively
            this.recursivelyAssembleLinkedList(newNode, childConvoNode.nodeChildrenString, nodeList);
        }

        parentNode.sortChildReferences();
    }

    compile(): ConvoNode[] {
        return this.linkedListBucket.convertToConvoNodes(this.areaId); // or perhaps we recursively traverse and hit all nodes, adding them to the bucket....
    }

    renderNodeTree() {
        return this.rootNode.createPalavyrNodeComponent();
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
