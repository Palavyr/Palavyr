import { NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { IPalavyrNode } from "./Contracts";
import { IPalavyrNodeChanger, PalavyrNodeChanger } from "./NodeChanger";

export class NodeUpdater {
    private nodeChanger: IPalavyrNodeChanger;
    constructor() {
        this.nodeChanger = new PalavyrNodeChanger();
    }

    public updateNode(currentNode: IPalavyrNode, textUpdate: string, valueOptions: string[], nodeTypeOptions: NodeTypeOptions) {
        this.updateText(currentNode, textUpdate);
        this.updateValueOptions(currentNode, valueOptions, nodeTypeOptions);
        currentNode.UpdateTree();
    }

    private updateText(currentNode: IPalavyrNode, textUpdate: string) {
        currentNode.userText = textUpdate;
    }

    private updateValueOptions(currentNode: IPalavyrNode, valueOptions: string[], nodeTypeOptions: NodeTypeOptions) {
        switch (currentNode.nodeTypeCode) {
            case NodeTypeCode.III: // Multioption Continue
                currentNode.setValueOptions(valueOptions);
                break;

            case NodeTypeCode.IV: // Multioption Path
                currentNode.setValueOptions(valueOptions);
                this.nodeChanger.createOrTruncateChildNodes(currentNode, valueOptions, nodeTypeOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            case NodeTypeCode.VI: //Anabranch
                currentNode.setValueOptions(valueOptions);
                this.nodeChanger.createOrTruncateChildNodes(currentNode, valueOptions, nodeTypeOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            default:
                break;
        }
    }
}
