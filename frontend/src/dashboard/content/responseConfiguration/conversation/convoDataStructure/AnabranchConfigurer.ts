import { SetState, NodeTypeOptions, NodeTypeCode } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "./Contracts";
import NodeTypeOptionConfigurer from "./NodeTypeOptionConfigurer";

class AnabranchConfigurer {
    constructor() {}

    private RecursiveDeconfigureAnabranchMergePointChildren(currentNode: IPalavyrNode) {
        const recurse = (childNodeReferences: INodeReferences) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((child: IPalavyrNode) => {
                if (child.isAnabranchType) return;
                child.isPalavyrAnabranchMember = false;
                this.ClearAnabranchContext(child);
                if (child.nodeType === "Loopback") {
                    // TODO: Assert against NodeCodeType == VIII
                    return;
                }
                recurse(child.childNodeReferences);
            });
        };

        recurse(currentNode.childNodeReferences);
    }

    private RecursiveReConfigureAnabranchMergePointChildren(currentNode: IPalavyrNode, anabranchLeftmost: boolean) {
        const parentalOriginId = currentNode.parentNodeReferences.retrieveLeftmostReference()?.anabranchContext.anabranchOriginId!;
        this.SetAnabranchContext(currentNode, parentalOriginId, anabranchLeftmost);

        const recurse = (childNodeReferences: INodeReferences, anabranchLeftmost: boolean) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((child: IPalavyrNode, index: number) => {
                if (child.isAnabranchType) return;
                if (index === 0 && anabranchLeftmost) {
                    child.anabranchContext.leftmostAnabranch = true;
                } else {
                    child.anabranchContext.leftmostAnabranch = false;
                }
                child.isPalavyrAnabranchMember = true;

                child.anabranchContext.anabranchOriginId = child.parentNodeReferences.retrieveLeftmostReference()?.anabranchContext.anabranchOriginId!;

                if (child.nodeType === "Loopback") {
                    return;
                }
                recurse(child.childNodeReferences, index === 0);
            });
        };

        recurse(currentNode.childNodeReferences, anabranchLeftmost);
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
                    this.RecursiveDeconfigureAnabranchMergePointChildren(child);
                    child.isPalavyrAnabranchMember = false;
                    this.SetAnabranchContext(child, "", false);
                }
                NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
            });
        } else {
            node.dereferenceThisAnabranchMergePoint(anabranchOriginNode, nodeTypeOptions);
            node.isPalavyrAnabranchEnd = false;
            node.isAnabranchMergePoint = false;
            setAnabranchMergeChecked(false);
            node.anabranchContext.leftmostAnabranch = true; // This is problematic probably
            node.childNodeReferences.forEach((child: IPalavyrNode, index: number) => {
                if (!child.isAnabranchType) {
                    this.RecursiveReConfigureAnabranchMergePointChildren(child, index === 0);
                    child.isPalavyrAnabranchMember = true;

                    const parentAnabranchContext = child.parentNodeReferences.retrieveLeftmostReference()!.anabranchContext;
                    this.SetAnabranchContext(child, parentAnabranchContext.anabranchOriginId, parentAnabranchContext.leftmostAnabranch);
                }
                NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
            });
        }
        node.UpdateTree();
    }

    public configureAnabranchWhenRoot(rootNode: IPalavyrNode) {
        rootNode.isPalavyrAnabranchStart = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchMember = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchEnd = false;

        if (rootNode.isAnabranchType) {
            this.SetAnabranchContext(rootNode, rootNode.nodeId, false);
        } else {
            this.ClearAnabranchContext(rootNode);
        }
    }

    public configureAnabranch(currentNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        // all nodes establish their own anabranch context
        // possibly update this if parent has anabranch origin node set
        this.SetAnabranchContext(currentNode, "", false);

        // the current node is an anabranch member if:
        // - the current node is anabranch type
        // - the parent is anabranch member and the current is not the end
        // - the parent is anabranch member and the current is the end and the current is anabranch type

        currentNode.isPalavyrAnabranchMember =
            currentNode.isAnabranchType ||
            (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd) ||
            (parentNode.isPalavyrAnabranchMember && currentNode.isPalavyrAnabranchEnd && currentNode.isPalavyrAnabranchMember);

        if (currentNode.isAnabranchType) {
            currentNode.isPalavyrAnabranchStart = true;
            currentNode.anabranchContext.anabranchOriginId = currentNode.nodeId;
        } else if (parentNode.isPalavyrAnabranchMember) {
            currentNode.anabranchContext.anabranchOriginId = parentNode.anabranchContext.anabranchOriginId;
            currentNode.isPalavyrAnabranchStart = false;
        } else {
            currentNode.isPalavyrAnabranchStart = false;
        }

        if (parentNode.isPalavyrAnabranchStart) {
            // then I need to determine if this is the leftmost child if I need to reach in to parent child references and see if this node is the
            const index = parentNode.childNodeReferences.findIndexOf(currentNode);
            const isLeftMost = index === 0;
            currentNode.anabranchContext.leftmostAnabranch = isLeftMost;
        } else if (currentNode.isPalavyrAnabranchMember) {
            // if parents.childnoderefs is more than one, then if this is not leftmost - change
            // otherwise original code
            if (parentNode.childNodeReferences.Length > 1) {
                const index = parentNode.childNodeReferences.findIndexOf(currentNode);
                const isLeftmost = index === 0;
                currentNode.anabranchContext.leftmostAnabranch = isLeftmost;
            } else {
                const parentLeftmost = parentNode.anabranchContext.leftmostAnabranch;
                currentNode.anabranchContext.leftmostAnabranch = parentLeftmost;
            }
        } else {
            this.ClearAnabranchContext(currentNode);
        }

        // merge points are considered endings - and this is a switch set on the node
        currentNode.isPalavyrAnabranchEnd = currentNode.isAnabranchMergePoint;

        if (currentNode.isPalavyrAnabranchMember) {
            const notAllowedInsideAnabranch = [NodeTypeCode.VI, NodeTypeCode.VII];
            if (currentNode.anabranchContext.leftmostAnabranch) {
                notAllowedInsideAnabranch.push(NodeTypeCode.IV);
                notAllowedInsideAnabranch.push(NodeTypeCode.V);
            }
            const options = NodeTypeOptionConfigurer.filterUnallowedNodeOptions(notAllowedInsideAnabranch, nodeTypeOptions);
            currentNode.setNodeTypeOptions(options);
        } else {
            const options = NodeTypeOptionConfigurer.filterUnallowedNodeOptions([], nodeTypeOptions);
            currentNode.setNodeTypeOptions(options);
        }
    }

    GetAnabranchOriginNode(node: IPalavyrNode) {
        if (!node.isPalavyrAnabranchMember) throw new Error("Not an anabranch member");
        const originId = node.anabranchContext.anabranchOriginId;
        const anabranchOriginNode = node.palavyrLinkedList.findNode(originId);
        if (anabranchOriginNode === null) throw new Error("anabranchOrigin Node not found.");

        return anabranchOriginNode;
    }

    public SetAnabranchContext(node: IPalavyrNode, originId: string, leftMost: boolean) {
        if (!node.anabranchContext) {
            this.ClearAnabranchContext(node);
        }
        this.SetAnabranchOriginId(node, originId);
        this.SetAnabranchLeftMost(node, leftMost);
    }

    public SetAnabranchOriginId(node: IPalavyrNode, originId: string) {
        node.anabranchContext.anabranchOriginId = originId;
    }

    public SetAnabranchLeftMost(node: IPalavyrNode, leftMost: boolean) {
        node.anabranchContext.leftmostAnabranch = leftMost;
    }

    public ClearAnabranchContext(node: IPalavyrNode) {
        node.anabranchContext = { anabranchOriginId: "", leftmostAnabranch: false };
    }

    public shouldShowAnabranchCheckBox(node: IPalavyrNode) {
        const _shouldShow =
            node.nodeIsSet() &&
            !node.isPalavyrAnabranchStart &&
            node.isPalavyrAnabranchMember &&
            !node.isTerminal &&
            node.anabranchContext.leftmostAnabranch &&
            !node.isAnabranchLocked &&
            node.nodeType !== "Loopback";

        if (node.isAnabranchMergePoint) {
            return true;
        } else {
            return _shouldShow;
        }
    }

    public recursivelyClearAnabranchContext(currentNode: IPalavyrNode) {
        // recurse and clear anabrach context -
        // if loopback, break. if anabranch type break;
        this.ClearAnabranchContext(currentNode);
        const recurse = (childNodeReferences: INodeReferences) => {
            childNodeReferences.forEach((childNode) => {
                if (childNode.isPalavyrAnabranchStart) return;
                this.ClearAnabranchContext(childNode);
                if (childNode.nodeType === "Loopback") return;
                recurse(childNode.childNodeReferences);
            });
        };

        recurse(currentNode.childNodeReferences);
    }
}

export default new AnabranchConfigurer();
