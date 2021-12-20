import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { ILinkedListBucket, INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "@Palavyr-Types";
import { LinkedListBucket } from "./LinkedListBucket";
import { NodeConfigurer } from "../node/actions/NodeConfigurer";
import { NodeCreator } from "../node/actions/NodeCreator";
import { PalavyrNode } from "../node/PalavyrNode";
import { Edge, ElementId, HandleType, Node as FlowNode, Position } from "react-flow-renderer";

export class PalavyrLinkedList implements IPalavyrLinkedList {
    private linkedListBucket: ILinkedListBucket = new LinkedListBucket();
    public areaId: string;
    public rootNode: IPalavyrNode;
    private head: ConvoNode;
    public updateTree: (updatedTree: IPalavyrLinkedList) => void;
    public repository: PalavyrRepository;
    private configurer: NodeConfigurer = new NodeConfigurer();
    private nodeCreator: NodeCreator = new NodeCreator();

    /**
     * List object for interacting with the list. This will have methods for performing insertions, deletions, additions, subtractions, etc
     */
    constructor(nodeList: ConvoNode[], areaId: string, updateTree: (updatedTree: IPalavyrLinkedList) => void, nodeTypeOptions: NodeTypeOptions, repository: PalavyrRepository) {
        this.areaId = areaId;
        this.repository = repository;
        this.head = this.getRootNode(nodeList);
        this.updateTree = updateTree;
        this.assembleDoubleLinkedMultiBranchLinkedList(nodeList, nodeTypeOptions);
    }

    public retrieveCleanHeadNode(): IPalavyrNode {
        const headNode = cloneDeep(this.rootNode);
        headNode.childNodeReferences.Clear();
        headNode.parentNodeReferences.Clear();
        return headNode;
    }

    private getRootNode(nodeList: ConvoNode[]) {
        return nodeList.filter(node => node.isRoot === true)[0];
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
        const headNode = this.convertToPalavyrNode(this.repository, this.head, this.updateTree, true);
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
                if (childConvoNode === null || childConvoNode === undefined) {
                    throw new Error("Could not find node with id: " + childId);
                }
                const newNode = this.convertToPalavyrNode(this.repository, childConvoNode, this.updateTree, index === 0);

                newNode.addNewNodeReferenceAndConfigure(newNode, parentNode, nodeTypeOptions);
                this.recursivelyAssembleLinkedList(newNode, childConvoNode.nodeChildrenString, nodeList, nodeTypeOptions);
            } else {
                existingNode.addNewNodeReferenceAndConfigure(existingNode, parentNode, nodeTypeOptions);
                if (this.guardAgainstInfiniteLoops(existingNode)) return;
            }
        }

        // parentNode.sortChildReferences(); // HO BOY. Not sorting anymore.
    }

    private guardAgainstInfiniteLoops(node: IPalavyrNode) {
        if (node.nodeType === "Loopback") {
            return true;
        } else {
            return false;
        }
    }

    public convertToPalavyrNode(repository: PalavyrRepository, rawNode: ConvoNode, updateTree: (updatedTree: IPalavyrLinkedList) => void, leftMostBranch: boolean) {
        return new PalavyrNode(this, repository, rawNode, updateTree, leftMostBranch);
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
        const update = this as IPalavyrLinkedList;
        this.updateTree(update);
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
        const update = this as IPalavyrLinkedList;
        this.nodeCreator.addDefaultRootNode(update, this.repository, restOfTree, currentText);
    }

    compileToNodeFlow() {
        const nodeElements: Partial<FlowNode>[] = [];
        const edgeElements: Edge[] = [];
        console.log("Compiled to flow");
        const nodeFlowCallback = (node: IPalavyrNode, index: number) => {
            //convert node into Handle
            nodeElements.push({
                id: node.nodeId,
                type: "nodeflowinterface",
                data: { label: node.optionPath, currentNode: node },
                sourcePosition: "top" as Position,
                targetPosition: "bottom" as Position,
            });

            // create all edges
            node.parentNodeReferences.forEach((parent: IPalavyrNode) => {
                edgeElements.push({
                    id: `${parent.nodeId}-${node.nodeId}`,
                    source: parent.nodeId,
                    target: node.nodeId,
                    type: "smoothstep",
                    sourceHandle: `a`,
                    targetHandle: `b`,
                    animated: true,
                    style: { stroke: "white", strokeWidth: 2, background: "repeating-linear-gradient(to right,red 0,red 10px,transparent 10px,transparent 12px)" },
                });
            });
        };
        this.traverse((node: IPalavyrNode, index: number) => nodeFlowCallback(node, index));
        const nodeFlowElements = [...nodeElements, ...edgeElements];
        return nodeFlowElements;
    }
}
