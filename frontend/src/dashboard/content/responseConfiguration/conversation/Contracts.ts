import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { ConvoNode, EmptyComponentType, LineMap, AnabranchContext, NodeTypeOptions, NodeTypeCode, LoopbackContext } from "@Palavyr-Types";

export interface NodeOptionalProps {
    node: IPalavyrNode;
}

export interface ILinkedListBucket {
    addToBucket(node: IPalavyrNode): void;
    convertToConvoNodes(areaId: string): ConvoNode[];
    addToBucket(node: IPalavyrNode): void;
    clear(): void;
    findById(nodeId: string): IPalavyrNode | null;
}

export interface IPalavyrLinkedList {
    rootNode: IPalavyrNode;
    areaId: string;
    traverse(): void;
    insert(): void;
    delete(): void;
    compileToConvoNodes(): ConvoNode[];
    reconfigureTree(nodeTypeOptions: NodeTypeOptions): void;
    findNode(nodeId: string): IPalavyrNode | null;
    retrieveCleanHeadNode(): IPalavyrNode;
    setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    resetRootNode(): void;
    convertToPalavyrNode(repository: PalavyrRepository, rawNode: ConvoNode, setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void, leftMostBranch: boolean): IPalavyrNode;
}

export interface INodeReferences {
    nodes: IPalavyrNode[];
    joinedReferenceString: string;
    referenceStringArray: string[];
    references: IPalavyrNode[];
    Length: number;
    contains(nodeId: string): boolean;
    addReference(node: IPalavyrNode): void;
    addReferences(nodes: IPalavyrNode[]): void;
    Empty(): boolean;
    NotEmpty(): boolean;
    OrderByOptionPath(): void;
    Clear(): void;
    getByIndex(index: number): IPalavyrNode;
    removeReference(palavyrNode: IPalavyrNode): void;
    checkIfReferenceExistsOnCondition(condition: (nodeReference: IPalavyrNode) => boolean): boolean;
    truncateAt(index: number): void;
    applyOptionPaths(valueOptions: string[]): void;
    collectPathOptions(): string[];
    retrieveLeftmostReference(): IPalavyrNode | null;
    findIndexOf(node: IPalavyrNode): number | null;
    containsNode(node: IPalavyrNode): boolean;
    forEach(callBack: (node: IPalavyrNode, index?: number | undefined) => void): void;
    Single(): IPalavyrNode;
    Where(condition: (node: IPalavyrNode) => boolean): INodeReferences;
    containsNodeType(nodeType: string): boolean;
    AllChildrenUnset(): boolean;
    replaceAtIndex(index: number, newNode: IPalavyrNode): void;
    ShiftLeft(currentNode: IPalavyrNode): void;
    ShiftRight(currentNode: IPalavyrNode): void;
}

export interface IPalavyrNode {
    lock(): void;
    unlock(): void;
    setAsProvideInfo(): void;
    nodeIsNotSet(): boolean;
    AddNewChildReference(newChildReference: IPalavyrNode): void;
    sortChildReferences(): void;
    addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions): void;
    compileConvoNode(areaId: string): ConvoNode;
    recursiveReferenceThisAnabranchOrigin(node: IPalavyrNode): void;
    dereferenceThisAnabranchMergePoint(anabranchOriginNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions): void;
    UpdateTree(): void;
    unsetSelf(nodeTypeOptions: NodeTypeOptions): void;
    nodeIsSet(): boolean;
    nodeIsNotSet(): boolean;
    setValueOptions(newValueOptions: string[]): void;
    addValueOption(newOption: string): void;
    getValueOptions(): string[];
    addLine(parentId: string): void;
    setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    removeLine(toNode: IPalavyrNode): void;
    setNodeTypeOptions(newNodeTypeOptions: NodeTypeOptions): void;
    Equals(otherNode: IPalavyrNode): boolean;
    LoopbackContextIsSet(): boolean;
    InsertChildNodeLink(nodeTypeOptions: NodeTypeOptions): void;
    DeleteCurrentNode(nodeTypeOptions: NodeTypeOptions): void;

    isRoot: boolean;
    nodeId: string;
    userText: string;
    isTerminal: boolean;
    shouldPresentResponse: boolean; // isCritical
    nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    isMultiOptionType: boolean;
    isDynamicTableNode: boolean;
    nodeComponentType: string;
    resolveOrder: number;
    shouldShowMultiOption: boolean;
    dynamicType: string | null;
    imageId: string | null | undefined;
    nodeTypeOptions: NodeTypeOptions;
    shouldDisableNodeTypeSelector: boolean;
    isImageNode: boolean;
    nodeTypeCode: NodeTypeCode;
    repository: PalavyrRepository;
    isLocked: boolean;

    // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    optionPath: string; // the value option that was used with the parent of this node.

    // transient
    shouldRenderChildren: boolean;
    isCurrency: boolean;

    // core
    childNodeReferences: INodeReferences;
    parentNodeReferences: INodeReferences;

    isMemberOfLeftmostBranch: boolean;
    lineMap: LineMap;

    palavyrLinkedList: IPalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    isAnabranchType: boolean;
    isAnabranchMergePoint: boolean;

    isPalavyrAnabranchStart: boolean;
    isPalavyrAnabranchMember: boolean;
    isPalavyrAnabranchEnd: boolean;
    isAnabranchLocked: boolean;
    anabranchContext: AnabranchContext;

    // LOOPBACK
    isLoopbackAnchorType: boolean;
    isLoopbackStart: boolean;
    isLoopbackMember: boolean;
    loopbackContext: LoopbackContext;
}
