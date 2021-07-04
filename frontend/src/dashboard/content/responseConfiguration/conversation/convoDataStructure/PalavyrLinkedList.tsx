import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { ILinkedListBucket, INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "./Contracts";
import { LinkedListBucket } from "./LinkedListBucket";
import { NodeConfigurer } from "./NodeConfigurer";
import { NodeCreator } from "./NodeCreator";
import { PalavyrImageNode } from "./PalavyrImageNode";
import { PalavyrTextNode } from "./PalavyrTextNode";

export class PalavyrLinkedList implements IPalavyrLinkedList {
    private linkedListBucket: ILinkedListBucket = new LinkedListBucket();
    private areaId: string;
    public rootNode: IPalavyrNode;
    private head: ConvoNode;
    public setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    private repository: PalavyrRepository = new PalavyrRepository();
    private configurer: NodeConfigurer = new NodeConfigurer();
    private nodeCreator: NodeCreator = new NodeCreator();
    private rawNodeList: ConvoNode[];

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], areaId: string, setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void, nodeTypeOptions: NodeTypeOptions) {
        this.rawNodeList = nodeList;
        this.areaId = areaId;
        this.head = this.getRootNode(nodeList);
        this.setTreeWithHistory = setTreeWithHistory;
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList, nodeTypeOptions);
    }

    public retrieveCleanHeadNode(): IPalavyrNode {
        const headNode = cloneDeep(this.rootNode);
        headNode.childNodeReferences.Clear();
        headNode.parentNodeReferences.Clear();
        return headNode;
    }

    private getRootNode(nodeList: ConvoNode[]) {
        return nodeList.filter((node) => node.isRoot === true)[0];
    }

    private _getNodeById = (nodeId: string, nodeList: ConvoNode[]) => {
        return nodeList.filter((node: ConvoNode) => node.nodeId === nodeId)[0];
    };

    private _splitAndRemoveEmptyNodeChildrenString(nodeChildrenString: string) {
        const childrenArray = this._splitNodeChildrenString(nodeChildrenString);
        return childrenArray.filter((childstring: string) => !isNullOrUndefinedOrWhitespace(childstring));
    }

    private _splitNodeChildrenString(nodeChildrenString: string) {
        return nodeChildrenString.split(",");
    }

    private assembleDoubleLinkedMultiBranchLinkedList(nodeList: ConvoNode[], nodeTypeOptions: NodeTypeOptions) {
        const headNode = this.convertToPalavyrNode(this, this.repository, this.head, nodeList, this.setTreeWithHistory, true);
        this.configurer.configure(headNode, null, nodeTypeOptions);
        this.rootNode = headNode;
        this.recursivelyAssembleLinkedList(headNode, this.head.nodeChildrenString, nodeList, nodeTypeOptions);
    }

    private recursivelyAssembleLinkedList(parentNode: IPalavyrNode, nodeChildrenString: string, nodeList: ConvoNode[], nodeTypeOptions: NodeTypeOptions) {
        if (parentNode.parentNodeReferences.containsNodeType("Loopback")) return;
        const childIds = this._splitAndRemoveEmptyNodeChildrenString(nodeChildrenString);
        if (childIds.length === 0) return;
        for (let index = 0; index < childIds.length; index++) {
            // childnodes add themselves to their parent node reference
            const childId = childIds[index];

            const existingNode = this.findNode(childId);

            if (existingNode === null) {
                const childConvoNode = this._getNodeById(childId, nodeList);
                const newNode = this.convertToPalavyrNode(this, this.repository, childConvoNode, nodeList, this.setTreeWithHistory, index === 0);

                newNode.addNewNodeReferenceAndConfigure(newNode, parentNode, nodeTypeOptions);
                this.recursivelyAssembleLinkedList(newNode, childConvoNode.nodeChildrenString, nodeList, nodeTypeOptions);
            } else {
                existingNode.addNewNodeReferenceAndConfigure(existingNode, parentNode, nodeTypeOptions);
                if (this.guardAgainstInfiniteLoops(existingNode)) return;
            }
        }

        parentNode.sortChildReferences();
    }

    private guardAgainstInfiniteLoops(node: IPalavyrNode) {
        if (node.nodeType === "Loopback") {
            return true;
        } else {
            return false;
        }
    }

    public convertToPalavyrNode(
        container: IPalavyrLinkedList,
        repository: PalavyrRepository,
        rawNode: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftMostBranch: boolean
    ) {
        let palavyrNode: IPalavyrNode;
        switch (rawNode.isImageNode) {
            case true:
                palavyrNode = new PalavyrImageNode(container, repository, rawNode, nodeList, setTreeWithHistory, leftMostBranch);
                break;
            case false:
                palavyrNode = new PalavyrTextNode(container, repository, rawNode, nodeList, setTreeWithHistory, leftMostBranch);
                break;
            default:
                throw new Error("Node type couldn't be determined when construting the palavyr convo tree.");
        }
        return palavyrNode;
    }

    compileToConvoNodes(): ConvoNode[] {
        this.linkedListBucket.clear();

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket.convertToConvoNodes(this.areaId);
    }

    compileToBucket(): ILinkedListBucket {
        this.linkedListBucket.clear();

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        return this.linkedListBucket;
    }

    renderNodeTree(pBuffer: number) {
        return this.rootNode.createPalavyrNodeComponent(pBuffer);
    }

    findNode(nodeId: string): IPalavyrNode | null {
        this.linkedListBucket.clear();

        const compileCallback = (node: IPalavyrNode) => {
            this.linkedListBucket.addToBucket(node);
        };
        this.traverse(compileCallback);

        const node = this.linkedListBucket.findById(nodeId); // could be null
        return node;
    }

    traverse(perNodeCallback?: any): void {
        const recurseTheTree = (childReferences: INodeReferences, parent: IPalavyrNode) => {
            for (let index = 0; index < childReferences.Length; index++) {
                const childNode = childReferences.references[index];
                if (perNodeCallback) perNodeCallback(childNode);
                if (this.guardAgainstInfiniteLoops(parent)) return;
                recurseTheTree(childNode.childNodeReferences, childNode);
            }
        };
        const headNode = this.rootNode;
        perNodeCallback(headNode);
        recurseTheTree(headNode.childNodeReferences, headNode);
    }

    reconfigureTree(nodeTypeOptions: NodeTypeOptions): void {
        this.traverse((node: IPalavyrNode) => {
            if (node.isRoot) {
                this.configurer.configure(node, null, nodeTypeOptions);
            } else {
                const leftmostParent = node.parentNodeReferences.retrieveLeftmostReference()!;
                this.configurer.configure(node, leftmostParent, nodeTypeOptions);
            }
        });
        this.setTreeWithHistory(this);
    }

    insert(): void {
        throw new Error("Method not implemented.");
    }
    delete(): void {
        throw new Error("Method not implemented.");
    }
    resetRootNode(): void {
        const restOfTree = this.rootNode.childNodeReferences;
        const currentText = this.rootNode.userText;
        this.nodeCreator.addDefaultRootNode(this, this.repository, restOfTree, this.rawNodeList, currentText);
    }
}
