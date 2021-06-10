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

    public get childNodeString() {
        return this.joinNodeChildrenStringArray(this.childNodeStringAsArray);
    }

    public get childNodeStringAsArray() {
        return this.nodeReferences.map((x) => x.nodeId);
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
}
