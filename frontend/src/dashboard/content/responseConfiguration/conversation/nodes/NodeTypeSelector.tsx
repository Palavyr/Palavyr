import React, { useState, useEffect } from "react";
import { ConvoNode, Conversation, Responses } from "@Palavyr-Types";
import { NodeTypeOptionsDefinition, NodeTypeOptions } from "./NodeTypeOptions";
import { createNewChildIDs } from "./conversationNodeUtils";
import { CustomNodeSelect } from "./CustomNodeSelect";


export interface INodeTypeSelector {
    node: ConvoNode;
    nodeList: Array<ConvoNode>;
    addNodes: (parentNode: ConvoNode, nodeList: Conversation, newIDs: Array<string>, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void) => void;
    setNodes: (nodeList: Conversation) => void;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    dynamicNodeTypes: NodeTypeOptions;
}

export const NodeTypeSelector = ({ node, nodeList, addNodes, setNodes, parentState, changeParentState, dynamicNodeTypes }: INodeTypeSelector) => {

    const [option, setSelectedOption] = useState<string>("");
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {

        setLoaded(true);

        if (node.nodeType === null) {
            setSelectedOption("");
        } else {
            setSelectedOption(node.nodeType);
        }
        return () => {
            setLoaded(false);
        }

    }, [node.nodeType]);

    let completeNodeTypes: NodeTypeOptions;
    if (loaded) {
        completeNodeTypes = { ...NodeTypeOptionsDefinition, ...dynamicNodeTypes }
    } else {
        completeNodeTypes = {}
    }

    const handleChange = (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => {

        const option = event.target.value as string; // unique selection i.e. SelectOneFlat-hlk-lkl-34kjl

        const optionPaths = completeNodeTypes[option].pathOptions;
        const numChildren: number = optionPaths.filter(x => x !== null).length;

        const childIds = createNewChildIDs(numChildren);

        // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
        // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
        // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
        node.nodeType = option; // SelectOneFlat-sdfs-sdfs-sgs-s

        var valueOptions = completeNodeTypes[option].valueOptions;

        addNodes(node, nodeList, childIds, optionPaths, valueOptions, setNodes); // create new nodes and update the Database
        setSelectedOption(option); // change option in curent node
        changeParentState(!parentState); // rerender lines
    };

    return (
        loaded ? <CustomNodeSelect onChange={handleChange} option={option} completeNodeTypes={completeNodeTypes} /> : null
    );
};
