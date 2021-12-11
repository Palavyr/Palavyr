import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { AlertType, NodeOption, NodeTypeOptions, ConvoNode } from "@Palavyr-Types";
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
        const currentNodeOption = nodeTypeOptions.filter((option: NodeOption) => option.value === currentNode.nodeType)[0];
        if (currentNodeOption) {
            setLabel(currentNodeOption.text);
        }
    }, [currentNode.nodeType]);

    const duplicateDynamicFeeNodeFound = (option: string, nodeTypeOptions: NodeTypeOptions) => {
        const dynamicNodeTypeOptions = nodeTypeOptions.filter((x: NodeOption) => x.isDynamicType);
        if (dynamicNodeTypeOptions.length > 0) {
            const dynamicNodeTypes = dynamicNodeTypeOptions.map((x: NodeOption) => x.value);
            const nodeList = currentNode.palavyrLinkedList.compileToConvoNodes(); // Write methods to handle this natively - this is a bit of a cheat atm.
            const dynamicNodesPresentInTheCurrentNodeList = nodeList.filter((x: ConvoNode) => dynamicNodeTypes.includes(x.nodeType));
            const dynamicNodes = dynamicNodesPresentInTheCurrentNodeList.map((x: ConvoNode) => x.nodeType);
            return dynamicNodes.includes(option);
        }
        return false;
    };

    const autocompleteOnChange = async (_: any, nodeOption: NodeOption) => {
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
