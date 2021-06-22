import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { INodeReferences, IPalavyrNode } from "./Contracts";

export class NodeReferences implements INodeReferences {
    private nodeReferences: IPalavyrNode[] = [];

    constructor(nodeReferences?: IPalavyrNode[]) {
        if (nodeReferences) {
            nodeReferences.forEach((ref: IPalavyrNode) => {
                this.addReference(ref);
            });
        }
    }
    collectPathOptions(): string[] {
        const pathOptions = this.nodeReferences.map((x: IPalavyrNode) => {
            return x.optionPath;
        });
        return pathOptions;
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

    public addReference(node: IPalavyrNode) {
        if (!this.contains(node.nodeId)) {
            this.nodeReferences.push(node);
        }
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
        this.nodeReferences.filter((node: IPalavyrNode) => node.nodeId !== palavyrNode.nodeId);
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
}
