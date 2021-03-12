import React, { useState, useEffect } from "react";
import { ConvoNode, NodeTypeOptions, NodeOption, AlertType } from "@Palavyr-Types";
import { CustomNodeSelect } from "./CustomNodeSelect";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { changeNodeType } from "./nodeUtils/commonNodeUtils";

export interface INodeTypeSelector {
    node: ConvoNode;
    parentNode: ConvoNode | null;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    nodeOptionList: NodeTypeOptions;
}

export const NodeTypeSelector = ({ node, parentNode, parentState, changeParentState, nodeOptionList }: INodeTypeSelector) => {
    const [alertState, setAlertState] = useState<boolean>(false);
    const [alertDetails, setAlertDetails] = useState<AlertType>();
    const [label, setLabel] = useState<string>("");

    const { nodeList, setNodes } = React.useContext(ConversationTreeContext);

    useEffect(() => {
        const currentNodeOption = nodeOptionList.filter((option: NodeOption) => option.value === node.nodeType)[0];
        if (currentNodeOption) {
            setLabel(currentNodeOption.text);
        }
    }, [node]);

    const duplicateDynamicFeeNodeFound = (option: string) => {
        const dynamicNodeTypeOptions = nodeOptionList.filter((x: NodeOption) => x.isDynamicType);
        if (dynamicNodeTypeOptions.length > 0) {
            const dynamicNodeTypes = dynamicNodeTypeOptions.map((x: NodeOption) => x.value);
            const dynamicNodesPresentInTheCurrentNodeList = nodeList.filter((x: ConvoNode) => dynamicNodeTypes.includes(x.nodeType));
            const dynamicNodes = dynamicNodesPresentInTheCurrentNodeList.map((x: ConvoNode) => x.nodeType);
            return dynamicNodes.includes(option) ? true : false;
        }
        return false;
    };

    const autocompleteOnChange = async (_: any, nodeOption: NodeOption) => {
        if (nodeOption === null) {
            return;
        }
        if (duplicateDynamicFeeNodeFound(nodeOption.value)) {
            setAlertDetails({
                title: `You've already placed dynamic table ${nodeOption.text} in this conversation`,
                message: "You can only place each dynamic table in your conversation once. If you would like to change where you've placed it in the conversation, you need to recreate that portion of the tree by selection a different node.",
            });
            setAlertState(true);
            return;
        }

        changeNodeType(node, nodeList, setNodes, nodeOption);
        changeParentState(!parentState); // rerender lines
    };

    return (
        <>
            {nodeOptionList && <CustomNodeSelect onChange={autocompleteOnChange} label={label} nodeOptionList={nodeOptionList} />}
            {alertDetails && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
        </>
    );
};
