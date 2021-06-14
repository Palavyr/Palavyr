import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import { getRootNode } from "../nodes/nodeUtils/commonNodeUtils";
import { _getNodeById, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "../nodes/nodeUtils/_coreNodeUtils";
import { LinkedListBucket } from "./LinkedListBucket";
import { PalavyrNode } from "./PalavyrNode";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { NodeReferences } from "./NodeReferences";

interface IPalavyrLinkedList {
    traverse(): void;
    insert(): void;
    delete(): void;
    compileToConvoNodes(): ConvoNode[];
}

export class PalavyrLinkedList implements IPalavyrLinkedList {
    private linkedListBucket: LinkedListBucket = new LinkedListBucket();
    private areaId: string; // areaIdentifier
    public rootNode: PalavyrNode;
    private head: ConvoNode;
    private setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void;
    private repository: PalavyrRepository = new PalavyrRepository();
    private nodeTypeOptions: NodeTypeOptions;

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], nodeTypeOptions: NodeTypeOptions, areaId: string, setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void) {
        this.areaId = areaId;
        this.head = getRootNode(nodeList);
        this.nodeTypeOptions = nodeTypeOptions;
        this.setTreeWithHistory = setTreeWithHistory;
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList);
    }

    private assembleDoubleLinkedMultiBranchLinkedList(nodeList: ConvoNode[]) {
        const headNode = PalavyrNode.convertToPalavyrNode(this, this.repository, this.nodeTypeOptions, this.head, nodeList, this.setTreeWithHistory, true);
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

            const newNode = PalavyrNode.convertToPalavyrNode(this, this.repository, this.nodeTypeOptions, childConvoNode, nodeList, this.setTreeWithHistory, index === 0);
            newNode.addNewNodeReferenceAndConfigure(newNode, parentNode);
            this.recursivelyAssembleLinkedList(newNode, childConvoNode.nodeChildrenString, nodeList);
        }

        parentNode.sortChildReferences();
    }

    compileToConvoNodes(): ConvoNode[] {
        this.linkedListBucket.clear();

        const compileCallback = (node: PalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket.convertToConvoNodes(this.areaId); // or perhaps we recursively traverse and hit all nodes, adding them to the bucket....
    }

    compileToBucket(): LinkedListBucket {
        this.linkedListBucket.clear();

        const compileCallback = (node: PalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket;
    }

    renderNodeTree() {
        return this.rootNode.createPalavyrNodeComponent();
    }

    findNode(nodeId: string): PalavyrNode {
        this.linkedListBucket.clear();

        const compileCallback = (node: PalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        const node = this.linkedListBucket.findById(nodeId);
        return node;
        // const rootNode = this.rootNode;
        // const findNodeRecursively = (parentNode: PalavyrNode) => {
        //     for (let index = 0; index < parentNode.childNodeReferences.Length; index++) {
        //         const childNode = parentNode.childNodeReferences.references[index];
        //         if (childNode.nodeId === nodeId) {
        //             return childNode;
        //         } else {
        //             const nodeToFind = findNodeRecursively(childNode);
        //             if (nodeToFind !== undefined) {
        //                 return nodeToFind;
        //             }
        //         }
        //     }
        // };
        // const node = findNodeRecursively(rootNode);
        // if (node === undefined) {
        //     throw new Error("Attempting to find a node that does not exist in the tree");
        // }
        // return node as PalavyrNode;
    }

    traverse(perNodeCallback?: any): void {
        const recurseTheTree = (childReferences: NodeReferences) => {
            for (let index = 0; index < childReferences.Length; index++) {
                const childNode = childReferences.references[index];
                if (perNodeCallback) perNodeCallback(childNode);
                recurseTheTree(childNode.childNodeReferences);
            }
        };
        const headNode = this.rootNode;
        perNodeCallback(headNode);
        recurseTheTree(headNode.childNodeReferences);
    }

    insert(): void {
        throw new Error("Method not implemented.");
    }
    delete(): void {
        throw new Error("Method not implemented.");
    }
}
