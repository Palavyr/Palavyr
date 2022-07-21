import { NodeTypeCodeEnum, NodeTypeOptionResources } from "@Palavyr-Types";
import { IPalavyrNode } from "@Palavyr-Types";
import NodeChanger from "./NodeChanger";

export class NodeUpdater {
    public updateNode(currentNode: IPalavyrNode, valueOptions: string[], nodeTypeOptions: NodeTypeOptionResources) {
        this.updateValueOptions(currentNode, valueOptions, nodeTypeOptions);
        currentNode.UpdateTree();
    }

    public async updateText(currentNode: IPalavyrNode, textUpdate: string, useNewEditor: boolean) {
        currentNode.userText = textUpdate;
        if (useNewEditor) {
            currentNode.UpdateTree();
        }
    }

    private updateValueOptions(currentNode: IPalavyrNode, valueOptions: string[], nodeTypeOptions: NodeTypeOptionResources) {
        switch (currentNode.nodeTypeCodeEnum) {
            case NodeTypeCodeEnum.III: // Multioption Continue
                currentNode.setValueOptions(valueOptions);
                break;

            case NodeTypeCodeEnum.IV: // Multioption Path
                currentNode.setValueOptions(valueOptions);
                NodeChanger.createOrTruncateChildNodes(currentNode, valueOptions, nodeTypeOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            case NodeTypeCodeEnum.VI: //Anabranch
                currentNode.setValueOptions(valueOptions);
                NodeChanger.createOrTruncateChildNodes(currentNode, valueOptions, nodeTypeOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            case NodeTypeCodeEnum.VII:
                currentNode.setValueOptions(valueOptions);
                NodeChanger.createOrTruncateChildNodes(currentNode, valueOptions, nodeTypeOptions);
                currentNode.childNodeReferences.applyOptionPaths(valueOptions);
                break;

            default:
                break;
        }
    }
}

export default new NodeUpdater();
