import React, { useState, useEffect } from "react";
import { ConvoNode, Conversation, Responses, NodeTypeOptions, NodeOption, AlertType } from "@Palavyr-Types";
import { addNodes, createNewChildIDs } from "./conversationNodeUtils";
import { CustomNodeSelect } from "./CustomNodeSelect";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";

export interface INodeTypeSelector {
    node: ConvoNode;
    nodeList: Array<ConvoNode>;
    setNodes: (nodeList: Conversation) => void;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    nodeOptionList: NodeTypeOptions;
}

export const NodeTypeSelector = ({ node, nodeList, setNodes, parentState, changeParentState, nodeOptionList }: INodeTypeSelector) => {
    const [alertState, setAlertState] = useState<boolean>(false);
    const [alertDetails, setAlertDetails] = useState<AlertType>();
    const [label, setLabel] = useState<string>("");

    useEffect(() => {
        const currentNodeOption = nodeOptionList.filter((option: NodeOption) => option.value === node.nodeType)[0];
        if (currentNodeOption) {
            setLabel(currentNodeOption.text)
        }
    },[node])

    const duplicateDynamicFeeNodeFound = (option: string) => {
        const dynamicNodeTypeOptions = nodeOptionList.filter((x: NodeOption) => x.isDynamicType);
        if (dynamicNodeTypeOptions.length > 0) {
            const dynamicNodeTypes = dynamicNodeTypeOptions.map((x: NodeOption) => x.value);
            const dynamicNodesPresentInTheCurrentNodeList = nodeList.filter((x: ConvoNode) => dynamicNodeTypes.includes(x.nodeType))
            const dynamicNodes = dynamicNodesPresentInTheCurrentNodeList.map((x: ConvoNode) => x.nodeType);
            return dynamicNodes.includes(option) ? true : false;
        }
        return false
    }

    const autocompleteOnChange = (event: any, nodeOption: NodeOption) => {

        if (nodeOption === null){
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

        const pathOptions = nodeOption.pathOptions;
        const valueOptions = nodeOption.valueOptions;

        if (pathOptions === undefined) {
            throw new Error("Ill defined path options");
        }
        if (valueOptions === undefined) {
            throw new Error("Ill defined value options - cannot be undefined");
        }

        const numChildren: number = pathOptions.filter((x) => x !== null).length;

        const childIds = createNewChildIDs(numChildren);

        // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
        // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
        // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
        node.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s

        addNodes(node, nodeList, childIds, pathOptions, valueOptions, setNodes); // create new nodes and update the Database
        // setSelectedOption(nodeOption); // change option in curent node
        changeParentState(!parentState); // rerender lines
    };


    return (
        <>
            {nodeOptionList && <CustomNodeSelect onChange={autocompleteOnChange} label={label} nodeOptionList={nodeOptionList} />}
            {alertDetails && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
        </>
    );
};
