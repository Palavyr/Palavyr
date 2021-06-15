import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { getRootNode } from "../nodes/nodeUtils/commonNodeUtils";
import { _splitAndRemoveEmptyNodeChildrenString, _getNodeById } from "../nodes/nodeUtils/_coreNodeUtils";
import { ILinkedListBucket, INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "./Contracts";
import { LinkedListBucket } from "./LinkedListBucket";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrLinkedList implements IPalavyrLinkedList {
    private linkedListBucket: ILinkedListBucket = new LinkedListBucket();
    private areaId: string; // areaIdentifier
    public rootNode: IPalavyrNode;
    private head: ConvoNode;
    private setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    private repository: PalavyrRepository = new PalavyrRepository();
    private nodeTypeOptions: NodeTypeOptions;

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], nodeTypeOptions: NodeTypeOptions, areaId: string, setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void) {
        this.areaId = areaId;
        this.head = getRootNode(nodeList);
        this.nodeTypeOptions = nodeTypeOptions;
        this.setTreeWithHistory = setTreeWithHistory;
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList);
    }

    public retrieveCleanHeadNode(): IPalavyrNode {
        const headNode = cloneDeep(this.rootNode);
        headNode.childNodeReferences.Clear();
        headNode.parentNodeReferences.Clear();
        return headNode;
    }

    private assembleDoubleLinkedMultiBranchLinkedList(nodeList: ConvoNode[]) {
        const headNode = PalavyrNode.convertToPalavyrNode(this, this.repository, this.nodeTypeOptions, this.head, nodeList, this.setTreeWithHistory, true);
        this.rootNode = headNode;
        this.linkedListBucket.addToBucket(headNode);
        this.recursivelyAssembleLinkedList(headNode, this.head.nodeChildrenString, nodeList);
    }

    private recursivelyAssembleLinkedList(parentNode: IPalavyrNode, nodeChildrenString: string, nodeList: ConvoNode[]) {
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

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket.convertToConvoNodes(this.areaId); // or perhaps we recursively traverse and hit all nodes, adding them to the bucket....
    }

    compileToBucket(): ILinkedListBucket {
        this.linkedListBucket.clear();

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket;
    }

    renderNodeTree() {
        return this.rootNode.createPalavyrNodeComponent();
    }

    findNode(nodeId: string): IPalavyrNode {
        this.linkedListBucket.clear();

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        const node = this.linkedListBucket.findById(nodeId);
        return node;
    }

    traverse(perNodeCallback?: any): void {
        const recurseTheTree = (childReferences: INodeReferences) => {
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
