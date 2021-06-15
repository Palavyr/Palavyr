import { ConvoNode, EmptyComponentType, LineMap, AnabranchContext, SplitmergeContext } from "@Palavyr-Types";

export interface ILinkedListBucket {
    addToBucket(node: IPalavyrNode): void;
    convertToConvoNodes(areaId: string): ConvoNode[];
    addToBucket(node: IPalavyrNode): void;
    clear(): void;
    findById(nodeId: string): IPalavyrNode;
}

export interface IPalavyrLinkedList {
    traverse(): void;
    insert(): void;
    delete(): void;
    compileToConvoNodes(): ConvoNode[];
    findNode(nodeId: string): IPalavyrNode;
    retrieveCleanHeadNode(): IPalavyrNode;
}

export interface INodeReferences {
    nodes: IPalavyrNode[];
    joinedReferenceString: string;
    referenceStringArray: string[];
    references: IPalavyrNode[];
    Length: number;
    contains(nodeId: string): boolean;
    addReference(node: IPalavyrNode): void;
    Empty(): boolean;
    NotEmpty(): boolean;
    OrderByOptionPath(): void;
    Clear(): void;
    getByIndex(index: number): IPalavyrNode;
    removeReference(palavyrNode: IPalavyrNode): void;
    checkIfReferenceExistsOnCondition(condition: (nodeReference: IPalavyrNode) => boolean): boolean;
}

export interface IPalavyrNode {
    createPalavyrNodeComponent(): EmptyComponentType;
    lock(): void;
    unlock(): void;
    setAsProvideInfo(): void;
    nodeIsNotSet(): boolean;
    AddNewChildReference(newChildReference: IPalavyrNode): void;
    configure(parentNode: IPalavyrNode): void;
    sortChildReferences(): void;
    addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode): void;
    compileConvoNode(areaId: string): ConvoNode;
    recursiveReferenceThisAnabranchOrigin(node: IPalavyrNode): void;
    recursiveDereferenceThisAnabranchOrigin(node: IPalavyrNode): void;
    UpdateTree(): void;
    removeSelf(): void;
    nodeIsSet(): boolean;
    nodeIsNotSet(): boolean;
    isPenultimate(): void;
    RouteToMostRecentSplitMerge(): void;
    addDefaultChild(valueOption: string): void;

    isRoot: boolean;
    nodeId: string;

    isTerminal: boolean;
    shouldPresentResponse: boolean; // isCritical
    nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    isMultiOptionType: boolean;

    nodeChildrenString: string;

    // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    optionPath: string; // the value option that was used with the parent of this node.

    // transient
    shouldRenderChildren: boolean;

    isCurrency: boolean;

    // core
    childNodeReferences: INodeReferences;
    parentNodeReferences: INodeReferences;

    isMemberOfLeftmostBranch: boolean;

    rawNode: ConvoNode; // Get Rid Of This.
    rawNodeList: ConvoNode[]; // Get Rid of this

    lineMap: LineMap;

    setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;

    palavyrLinkedList: IPalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    isPalavyrAnabranchStart: boolean;
    isPalavyrAnabranchMember: boolean;
    isPalavyrAnabranchEnd: boolean;
    isAnabranchLocked: boolean;
    anabranchContext: AnabranchContext;

    // SPLIT MERGE (we also get isSplitMerge from Db, so samesy - infer the rest here)
    isPalavyrSplitmergeStart: boolean;
    isPalavyrSplitmergeMember: boolean;
    isPalavyrSplitmergePrimarybranch: boolean;
    isPalavyrSplitmergeEnd: boolean;
    isPalavyrSplitmergeMergePoint: boolean;
    splitmergeContext: SplitmergeContext;
}
