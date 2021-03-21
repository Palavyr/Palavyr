import { ConvoNode, NodeIdentity } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography, Divider } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep } from "lodash";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _createAndAddNewNodes, _getLeftMostParentNode, _getNodeById, _getParentNode, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString, _truncateTheTreeAtSpecificNode } from "./nodeUtils/_coreNodeUtils";
import { useEffect } from "react";
import { checkedNodeOptionList, updateSingleOptionType } from "./nodeUtils/commonNodeUtils";
import { uuid } from "uuidv4";
import { recursivelyReferenceCurrentNodeInNonTerminalLeafNodes } from "./nodeUtils/AnabranchUtils";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeCheckBox } from "./NodeCheckBox";
import { recursivelyDereferenceNodeIdFromChildrenExceptWhen } from "./nodeUtils/dereferenceUtils";

type StyleProps = {
    nodeText: string;
    nodeType: string;
    checked: boolean;
    isDecendentOfSplitMerge: boolean;
    splitMergeRootSiblingIndex: number;
};

const useStyles = makeStyles(() => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: "600px",
        minHeight: "320px",
        borderColor: props.nodeType === "" ? "red" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "purple" : "#54585A",
        borderWidth: props.nodeType === "" ? "5px" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "8px" : "2px",
        borderRadius: "3px",
        backgroundColor: "#C7ECEE",
    }),
    card: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "area",

    },
    bullet: {
        display: "inline-block",
        margin: "0 2px",
        transform: "scale(0.8)",
    },
    title: {
        fontSize: 14,
    },
    pos: {
        marginBottom: 12,
    },
    textCard: (props: StyleProps) => ({
        border: "1px solid gray",
        padding: "10px",
        textAlign: "center",
        color: props.nodeText === "Ask your question!" ? "white" : "black",
        background: props.nodeText === "Ask your question!" ? "red" : "white",
        "&:hover": {
            background: "lightgray",
            color: "black",
        },

    }),
    text: {
        margin: ".1rem",
        fontSize: "16px",
    },
    formstyle: {
        fontSize: "12px",
        alignSelf: "bottom",
    },
    editorStyle: {
        fontSize: "12px",
        color: "lightgray",
    },
    formLabelStyle: (props: StyleProps) => ({
        fontSize: "12px",
        color: props.checked ? "black" : "gray",
    }),
    interfaceElement: {
        paddingBottom: "1rem",
    },
}));

export interface IConversationNodeInterface {
    node: ConvoNode;
    identity: NodeIdentity;
    reRender: () => void;
}

export const ConversationNodeInterface = ({ node, identity, reRender }: IConversationNodeInterface) => {
    const { setNodes, nodeList, nodeTypeOptions, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData } = React.useContext(ConversationTreeContext);
    const parentNode = _getParentNode(node, nodeList);

    const [modalState, setModalState] = useState<boolean>(false);
    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    const classes = useStyles({
        nodeType: node.nodeType,
        nodeText: node.text,
        checked: node.isCritical,
        isDecendentOfSplitMerge: identity.isDecendentOfSplitMerge,
        splitMergeRootSiblingIndex: identity.splitMergeRootSiblingIndex,
    });

    useEffect(() => {
        if (identity.shouldCheckSplitMergeBox) {
            setMergeBoxChecked(true);
        }
    }, [node, nodeList]);

    useEffect(() => {
        if (identity.isAnabranchMergePoint) {
            setAnabranchMergeChecked(true);
        }
    }, []);

    const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
        if (event.target.checked) {
            const newNode = cloneDeep(node);
            newNode.isCritical = event.target.checked;
            const updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
            setNodes(updatedNodeList);
        } else {
            historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
        }
    };

    const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
        if (event.target.checked) {
            const newNode = cloneDeep(node);
            let updatedNodeList = cloneDeep(nodeList);
            newNode.shouldRenderChildren = false;
            updatedNodeList = _truncateTheTreeAtSpecificNode(node, nodeList);
            newNode.nodeChildrenString = identity.nodeIdOfMostRecentSplitMergePrimarySibling;
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
            setMergeBoxChecked(event.target.checked);
            setNodes(cloneDeep(updatedNodeList));
        } else {
            if (conversationHistoryPosition === 0) {
                const childId = uuid();
                const newNode = cloneDeep(node);
                newNode.shouldRenderChildren = true;
                newNode.nodeChildrenString = childId;
                let updatedNodeList = _createAndAddNewNodes([childId], [childId], node.areaIdentifier, ["Continue"], nodeList, false);
                updateSingleOptionType(newNode, updatedNodeList, setNodes);
                setMergeBoxChecked(false);
            } else {
                historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
            }
        }
    };

    const handleSetAsAnabranchMergePointClick = (event: { target: { checked: boolean } }) => {
        if (event.target.checked) {
            const newNode = cloneDeep(node);
            let updatedNodeList = cloneDeep(nodeList);

            newNode.isAnabranchMergePoint = true;
            updatedNodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(newNode.nodeId, updatedNodeList, identity.nodeIdOfMostRecentAnabranch);
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
            setAnabranchMergeChecked(true);
            setNodes(cloneDeep(updatedNodeList));
        } else {
            // if (conversationHistoryPosition === 0) {
            let updatedNodeList = cloneDeep(nodeList);
            const newNode = cloneDeep(node);
            newNode.isAnabranchMergePoint = false;
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);

            const anabranchRootNode = _getNodeById(identity.nodeIdOfMostRecentAnabranch, updatedNodeList);
            const leftmostParentNode = _getLeftMostParentNode(node, nodeList, (node: ConvoNode) => node.isAnabranchType);
            if (leftmostParentNode){
                recursivelyDereferenceNodeIdFromChildrenExceptWhen(leftmostParentNode.nodeId, anabranchRootNode, updatedNodeList, newNode.nodeId);
                setAnabranchMergeChecked(false);
                setNodes(cloneDeep(updatedNodeList));
            }
            // } else {
            //     historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
            // }
        }
    };

    const handleUnsetCurrentNodeType = () => {
        let updatedNodeList = cloneDeep(nodeList);
        const newNode = cloneDeep(node);
        newNode.nodeType = "";
        newNode.nodeChildrenString = "";
        newNode.valueOptions = "";
        updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
        setNodes(cloneDeep([...updatedNodeList]));
    };

    const dataitems = [
        { isDecendentOfSplitMerge: identity.isDecendentOfSplitMerge },
        { decendentLevelFromSplitMerge: identity.decendentLevelFromSplitMerge },
        { splitMergeRootSiblingIndex: identity.splitMergeRootSiblingIndex },
        { nodeIdOfMostRecentSplitMergePrimarySibling: identity.nodeIdOfMostRecentSplitMergePrimarySibling },
        { isDecendentOfAnabranch: identity.isDecendentOfAnabranch },
        { decendentLevelFromAnabranch: identity.decendentLevelFromAnabranch },
        { nodeIdOfMostRecentAnabranch: identity.nodeIdOfMostRecentAnabranch },
        { isDirectChildOfAnabranch: identity.isDirectChildOfAnabranch },
        { isParentOfAnabranchMergePoint: identity.isParentOfAnabranchMergePoint },
        { isAncestorOfAnabranchMergePoint: identity.isAncestorOfAnabranchMergePoint },
        { conversationHistoryPosition: conversationHistoryPosition },
        { modalState: modalState },
        { mergeBoxChecked: mergeBoxChecked },
        { anabranchMergeChecked: anabranchMergeChecked },
        { otherNodeAlreadySetAsMergeBranchBool: identity.otherNodeAlreadySetAsMergeBranchBool },
        { shouldDisabledNodeTypeSelector: identity.shouldDisabledNodeTypeSelector },
        { canUnSetNodeType: identity.canUnSetNodeType },
        { shouldShowUnsetNodeTypeOption: identity.shouldShowUnsetNodeTypeOption},
        { isOnLeftmostAnabranchBranch: identity.isOnLeftmostAnabranchBranch }
    ];

    const nodeProperties = [
        { isRoot: node.isRoot },
        { nodeId: node.nodeId },
        { nodeType: node.nodeType },
        { text: node.text },
        { isCritical: node.isCritical },
        { nodeChildrenString: node.nodeChildrenString },
        { isTerminalType: node.isTerminalType },
        { isMultiOptionType: node.isMultiOptionType },
        { shouldShowMultiOption: node.shouldShowMultiOption },
        { optionPath: node.optionPath },
        { valueOptions: node.valueOptions },
        { shouldRenderChildren: node.shouldRenderChildren },
        { isSplitMergeType: node.isSplitMergeType },
        { isAnabranchType: node.isAnabranchType },
        { isAnabranchMergePoint: node.isAnabranchMergePoint },
    ];

    const filteredNodeTypeOptions = () => {
        const filtered = (mergeBoxChecked && identity.isDecendentOfSplitMerge) || identity.isParentOfAnabranchMergePoint ? checkedNodeOptionList(nodeTypeOptions, identity.isDecendentOfSplitMerge, identity.splitMergeRootSiblingIndex, identity.isParentOfAnabranchMergePoint) : nodeTypeOptions;
        return filtered;
    };

    return (
        <Card className={classNames(classes.root, node.nodeId)} variant="outlined">
            <CardContent className={classes.card}>
                {showDebugData && <DataLogging nodeProperties={nodeProperties} nodeChildren={node.nodeChildrenString} nodeId={node.nodeId} data={dataitems} />}
                <Typography className={classes.interfaceElement} variant={node.isRoot ? "h5" : "body1"} align="center">
                    {node.isRoot ? "Begin" : node.optionPath === "Continue" ? node.optionPath : "If " + node.optionPath}
                </Typography>
                <Card elevation={0} className={classNames(classes.interfaceElement, classes.textCard)} onClick={() => setModalState(true)}>
                    <Typography className={classes.text} variant="body2" component="span" noWrap={false}>
                        {node.text}
                    </Typography>
                    <Typography align="center" className={classes.editorStyle} onClick={() => setModalState(true)}>
                        Click to Edit
                    </Typography>
                </Card>
                <NodeTypeSelector nodeTypeOptions={filteredNodeTypeOptions()} node={node} reRender={reRender} shouldDisabledNodeTypeSelector={identity.shouldDisabledNodeTypeSelector} />
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />

                {identity.shouldShowResponseInPdfOption && <NodeCheckBox label="Show response in PDF" checked={node.isCritical} onChange={showResponseInPdfCheckbox} />}
                {identity.shouldShowMergeWithPrimarySiblingBranchOption && <NodeCheckBox label="Merge with primary sibling branch" checked={!node.shouldRenderChildren} onChange={handleMergeBackInOnClick} />}
                {identity.shouldShowSetAsAnabranchMergePointOption && <NodeCheckBox label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={handleSetAsAnabranchMergePointClick} />}
                {identity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}
                {identity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
            </CardContent>
        </Card>
    );
};

type DataItem = {
    [key: string]: any;
};

interface DataProps {
    data: DataItem[];
    nodeId: string;
    nodeChildren: string;
    nodeProperties: DataItem;
}

const DataLogging = (props: DataProps) => {
    return (
        <div>
            <Typography align="center">{props.nodeId}</Typography>
            <ul>
                {props.data.map((item: DataItem) => {
                    const key = Object.keys(item)[0];
                    let val = Object.values(item)[0];
                    if (typeof val === "boolean") {
                        val = val.toString().toUpperCase();
                    }
                    return (
                        <>
                            <li>
                                <Typography>
                                    {key}: {val}
                                </Typography>
                            </li>
                        </>
                    );
                })}
            </ul>
            <Divider />
            <ul>
                {props.nodeProperties.map((item: DataItem) => {
                    const key = Object.keys(item)[0];
                    let val = Object.values(item)[0];
                    if (typeof val === "boolean") {
                        val = val.toString().toUpperCase();
                    }
                    return (
                        <>
                            <li>
                                <Typography>
                                    {key}: {val}
                                </Typography>
                            </li>
                        </>
                    );
                })}
            </ul>
            <Divider />
            <Typography align="center">Children</Typography>
            <Typography>{props.nodeChildren}</Typography>
        </div>
    );
};
