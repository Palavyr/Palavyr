import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { AlertType, NodeTypeOptionResource, NodeTypeOptionResources, ConversationDesignerNodeResource } from "@Palavyr-Types";
import { ConversationTreeContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { IPalavyrNode } from "@Palavyr-Types";
import NodeChanger from "../actions/NodeChanger";
import NodeTypeOptionConfigurer from "../actions/NodeTypeOptionConfigurer";
import { CustomNodeSelect } from "../nodeInterface/nodeSelector/CustomNodeSelect";

export interface NodeTypeSelectorProps {
    currentNode: IPalavyrNode;
    shouldDisableNodeTypeSelector: boolean;
}

export const NodeTypeSelector = ({ currentNode, shouldDisableNodeTypeSelector }: NodeTypeSelectorProps) => {
    const [alertState, setAlertState] = useState<boolean>(false);
    const [alertDetails, setAlertDetails] = useState<AlertType>();
    const [label, setLabel] = useState<string>("");

    const { nodeTypeOptions } = useContext(ConversationTreeContext);

    useEffect(() => {
        const currentNodeOption = nodeTypeOptions.filter((option: NodeTypeOptionResource) => option.value === currentNode.nodeType)[0];
        if (currentNodeOption) {
            setLabel(currentNodeOption.text);
        }
    }, [currentNode.nodeType]);

    const duplicateDynamicFeeNodeFound = (option: string, nodeTypeOptions: NodeTypeOptionResources) => {
        const dynamicNodeTypeOptions = nodeTypeOptions.filter((x: NodeTypeOptionResource) => x.isPricingStrategyType);
        if (dynamicNodeTypeOptions.length > 0) {
            const dynamicNodeTypes = dynamicNodeTypeOptions.map((x: NodeTypeOptionResource) => x.value);
            const nodeList = currentNode.palavyrLinkedList.compileToConvoNodes(); // Write methods to handle this natively - this is a bit of a cheat atm.
            const dynamicNodesPresentInTheCurrentNodeList = nodeList.filter((x: ConversationDesignerNodeResource) => dynamicNodeTypes.includes(x.nodeType));
            const dynamicNodes = dynamicNodesPresentInTheCurrentNodeList.map((x: ConversationDesignerNodeResource) => x.nodeType);
            return dynamicNodes.includes(option);
        }
        return false;
    };

    const autocompleteOnChange = async (_: any, nodeOption: NodeTypeOptionResource) => {
        if (nodeOption === null) {
            return;
        }

        if (duplicateDynamicFeeNodeFound(nodeOption.value, nodeTypeOptions)) {
            setAlertDetails({
                title: `You've already placed dynamic table ${nodeOption.text} in this conversation`,
                message:
                    "You can only place each dynamic table in your conversation once. If you would like to change where you've placed it in the conversation, you need to recreate that portion of the tree by selection a different node.",
            });
            setAlertState(true);
            return;
        }

        NodeChanger.ExecuteNodeSelectorUpdate(nodeOption, currentNode, nodeTypeOptions);
    };

    return (
        <>
            {nodeTypeOptions && (
                <CustomNodeSelect
                    onChange={autocompleteOnChange}
                    label={label}
                    nodeTypeOptions={NodeTypeOptionConfigurer.ConfigureNodeTypeOptions(currentNode, nodeTypeOptions)}
                    shouldDisabledNodeTypeSelector={shouldDisableNodeTypeSelector}
                />
            )}
            {alertDetails && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
        </>
    );
};
