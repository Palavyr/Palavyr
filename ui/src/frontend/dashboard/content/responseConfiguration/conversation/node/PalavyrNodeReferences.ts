import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { INodeReferences, IPalavyrNode } from "@Palavyr-Types";

export class NodeReferences implements INodeReferences {
    private nodeReferences: IPalavyrNode[] = [];

    constructor(nodeReferences?: IPalavyrNode[]) {
        if (nodeReferences) {
            nodeReferences.forEach((ref: IPalavyrNode) => {
                this.addReference(ref);
            });
        }
    }
    addReferences(nodes: IPalavyrNode[]): void {
        nodes.forEach((node: IPalavyrNode) => this.addReference(node));
    }
    collectPathOptions(): string[] {
        const pathOptions = this.nodeReferences.map((x: IPalavyrNode) => {
            return x.optionPath;
        });
        return pathOptions;
    }

    replaceAtIndex(index: number, newNode: IPalavyrNode): void {
        this.nodeReferences[index] = newNode;
    }

    public applyOptionPaths(valueOptions: string[]): void {
        if (valueOptions.length !== this.nodeReferences.length) throw new Error("Value options did not match number of node references");
        for (let index = 0; index < this.nodeReferences.length; index++) {
            const node = this.nodeReferences[index];
            node.optionPath = valueOptions[index];
        }
    }

    public get nodes() {
        return this.nodeReferences;
    }

    public get joinedReferenceString() {
        return this.joinNodeChildrenStringArray(this.referenceStringArray);
    }

    public get referenceStringArray() {
        return this.nodeReferences.map((x) => x.nodeId);
    }

    public get references() {
        return this.nodeReferences;
    }

    public get Length() {
        return this.nodeReferences.length;
    }

    private joinNodeChildrenStringArray(nodeChildrenStrings: string[]) {
        return nodeChildrenStrings.join(",");
    }

    public contains(nodeId: string) {
        return this.nodeReferences.filter((x: IPalavyrNode) => x.nodeId === nodeId).length > 0;
    }

    public containsNode(node: IPalavyrNode) {
        return this.contains(node.nodeId);
    }

    public addReference(node: IPalavyrNode) {
        if (!this.contains(node.nodeId)) {
            this.nodeReferences.push(node);
        }
    }

    public forEach(callBack: (node: IPalavyrNode, index: number) => void) {
        this.nodeReferences.forEach((node: IPalavyrNode, index: number) => callBack(node, index));
    }

    public Empty() {
        return this.nodeReferences.length === 0;
    }

    public NotEmpty() {
        return this.nodeReferences.length > 0;
    }

    public OrderByOptionPath() {
        this.nodeReferences = sortByPropertyAlphabetical((x: IPalavyrNode) => x.optionPath.toUpperCase(), this.nodeReferences);
    }

    public Clear() {
        this.nodeReferences = [];
    }

    public getByIndex(index: number) {
        try {
            return this.nodeReferences[index];
        } catch {
            throw new Error(`Failed to find node reference index: Index: ${index} out of range ${this.Length}`);
        }
    }

    public removeReference(palavyrNode: IPalavyrNode) {
        this.nodeReferences = this.nodeReferences.filter((node: IPalavyrNode) => node.nodeId !== palavyrNode.nodeId);
    }

    public checkIfReferenceExistsOnCondition(condition: (nodeReference: IPalavyrNode) => boolean) {
        const result = this.nodeReferences.map(condition);
        return result.some((x) => x); // TODO: Check this works;
    }

    public truncateAt(index: number) {
        this.nodeReferences = this.nodeReferences.slice(0, index);
    }

    public retrieveLeftmostReference(): IPalavyrNode | null {
        if (this.Length > 0) {
            return this.nodeReferences[0];
        }
        return null;
    }

    public findIndexOf(node: IPalavyrNode): number | null {
        const index = this.nodeReferences.findIndex((x: IPalavyrNode) => x.nodeId === node.nodeId);
        if (index === -1) return null;
        return index;
    }

    public Single() {
        if (this.Length !== 1) throw new Error(`Only 1 node not present in reference list. Total found: ${this.Length}`);
        return this.nodeReferences[0];
    }

    public Where(condition: (node: IPalavyrNode) => boolean): INodeReferences {
        const remaining = this.nodeReferences.filter(condition);
        return new NodeReferences(remaining) as INodeReferences;
    }

    public containsNodeType(nodeType: string) {
        return this.references.map((x) => x.nodeType).includes(nodeType);
    }

    public AllChildrenUnset(): boolean {
        return this.references.map((x) => x.nodeIsNotSet()).every((x) => x);
    }

    public ShiftLeft(currentNode: IPalavyrNode): void {
        const currentIndex = this.findIndexOf(currentNode);
        if (currentIndex === 0 || currentIndex === null) return;
        const nodeToTheLeft = this.nodeReferences[currentIndex - 1];
        const thisNode = this.nodeReferences[currentIndex];
        this.nodeReferences[currentIndex] = nodeToTheLeft;
        this.nodeReferences[currentIndex - 1] = thisNode;
    }

    public ShiftRight(currentNode: IPalavyrNode): void {
        const currentIndex = this.findIndexOf(currentNode);
        if (currentIndex === this.nodeReferences.length - 1 || currentIndex === null) return;

        const nodeToTheRight = this.nodeReferences[currentIndex + 1];
        const thisNode = this.nodeReferences[currentIndex];

        this.nodeReferences[currentIndex] = nodeToTheRight;
        this.nodeReferences[currentIndex + 1] = thisNode;
    }
}
