import { NodeTypeCode } from "@Palavyr-Types";
import { IPalavyrNode } from "./Contracts";
import { IPalavyrNodeChanger, PalavyrNodeChanger } from "./NodeChanger";

export class NodeUpdater {

    private nodeChanger: IPalavyrNodeChanger;
    constructor() {
        this.nodeChanger = new PalavyrNodeChanger();
    }

    public updateNode(currentNode: IPalavyrNode, textUpdate: string, valueOptions: string[]) {
        this.updateText(currentNode, textUpdate);
        this.updateValueOptions(currentNode, valueOptions);
        currentNode.UpdateTree();
    }

    private updateText(currentNode: IPalavyrNode, textUpdate: string) {
        currentNode.userText = textUpdate;
    }

    private updateValueOptions(currentNode: IPalavyrNode, valueOptions: string[]) {
        switch (currentNode.nodeTypeCode) {
            case NodeTypeCode.III: // Multioption Continue
                currentNode.setValueOptions(valueOptions);
                break;

            case NodeTypeCode.IV: // Multioption Path
                currentNode.setValueOptions(valueOptions);
                this.nodeChanger.createOrTruncateChildNodes(currentNode, valueOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            case NodeTypeCode.VI: //Anabranch
                throw new Error("Not yet implemented")
                break;

            case NodeTypeCode.VII: // SplitMerge
            throw new Error ("Not yet implemented")
                break;

            default:
                break;
        }
    }
}
