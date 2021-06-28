import { SetState, NodeTypeOptions } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "./Contracts";
import NodeTypeOptionConfigurer from "./NodeTypeOptionConfigurer";

class AnabranchConfigurer {
    constructor() {}

    public RecursiveDeconfigureAnabranchChildren(currentNode: IPalavyrNode) {
        const recurse = (childNodeReferences: INodeReferences) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((node: IPalavyrNode, index: number) => {
                if (node.isAnabranchType) return;
                node.isPalavyrAnabranchMember = false;
                recurse(node.childNodeReferences);
            });
        };

        recurse(currentNode.childNodeReferences);
    }

    public SetAnabranchCheckBox(checked: boolean, setAnabranchMergeChecked: SetState<boolean>, node: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        const origin = node.anabranchContext.anabranchOriginId;
        const anabranchOriginNode = node.palavyrLinkedList.findNode(origin);

        if (anabranchOriginNode === null) throw new Error("anabranchOrigin Node not found.");

        if (checked) {
            node.isPalavyrAnabranchEnd = true;
            node.isAnabranchMergePoint = true;
            anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(node);
            setAnabranchMergeChecked(true);
            NodeTypeOptionConfigurer.ConfigureNodeTypeOptions(node, nodeTypeOptions);
            node.childNodeReferences.forEach((child: IPalavyrNode) => {
                if (!node.isAnabranchType) {
                    this.RecursiveDeconfigureAnabranchChildren(node);
                    child.isPalavyrAnabranchMember = false;
                    child.anabranchContext = { anabranchOriginId: "", leftmostAnabranch: false };
                }
                NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
            });
        } else {
            node.dereferenceThisAnabranchMergePoint(anabranchOriginNode, nodeTypeOptions);
            node.isPalavyrAnabranchEnd = false;
            node.isAnabranchMergePoint = false;
            setAnabranchMergeChecked(false);
            node.childNodeReferences.forEach((child: IPalavyrNode) => {
                if (!node.isAnabranchType) {
                    this.RecursiveDeconfigureAnabranchChildren(node);
                    child.isPalavyrAnabranchMember = true;
                    child.anabranchContext = child.parentNodeReferences.retrieveLeftmostReference()?.anabranchContext!;
                }
                NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
            });
            node.anabranchContext.leftmostAnabranch = true;
        }
        node.UpdateTree();
    }
}

export default new AnabranchConfigurer();
