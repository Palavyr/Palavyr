import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { _joinNodeChildrenStringArray } from "../nodes/nodeUtils/_coreNodeUtils";
import { PalavyrNode } from "./PalavyrNode";

export class NodeReferences {
    private nodeReferences: PalavyrNode[] = [];

    constructor(nodeReferences?: PalavyrNode[]) {
        if (nodeReferences) {
            this.nodeReferences = nodeReferences;
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

    public get getPalavyrNodesReferences() {
        return this.nodeReferences;
    }

    public get Length() {
        return this.nodeReferences.length;
    }

    private joinNodeChildrenStringArray(nodeChildrenStrings: string[]) {
        return nodeChildrenStrings.join(",");
    }

    public contains(nodeId: string) {
        return this.nodeReferences.filter((x: PalavyrNode) => x.nodeId === nodeId).length > 0;
    }

    public addReference(node: PalavyrNode) {
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
        this.nodeReferences = sortByPropertyAlphabetical((x: PalavyrNode) => x.optionPath.toUpperCase(), this.nodeReferences);
    }

    public Clear() {
        this.nodeReferences = [];
    }

    public getByIndex(index: number) {
        try {
            return this.nodeReferences[index];
        } catch {
            throw new Error(`Failed to find node reference index: Index: ${index} out of range ${this.Length}`)
        }
    }
}
