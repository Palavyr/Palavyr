import { ConvoNode, MostRecentSplitMerge, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography, FormControlLabel, Checkbox } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep } from "lodash";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _replaceNodeWithUpdatedNode, _truncateTheTreeAtSpecificNode } from "./nodeUtils/_coreNodeUtils";
import { useEffect } from "react";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { checkedNodeOptionList } from "./nodeUtils/commonNodeUtils";

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
        maxWidth: "300px",
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

export interface IConversationNodeInterface extends MostRecentSplitMerge {
    node: ConvoNode;
    parentNode: ConvoNode | null;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    optionPath: string | null;
    nodeOptionList: NodeTypeOptions;
}

export const ConversationNodeInterface = ({
    nodeOptionList,
    node,
    parentNode,
    optionPath,
    parentState,
    changeParentState,
    isDecendentOfSplitMerge,
    decendentLevelFromSplitMerge,
    splitMergeRootSiblingIndex,
    nodeIdOfMostRecentSplitMergePrimarySibling,
}: IConversationNodeInterface) => {
    const { setNodes, nodeList, conversationHistory, historyTracker, conversationHistoryPosition } = React.useContext(ConversationTreeContext);

    const [previousChildren, setPreviousChildren] = useState<string>("");
    const [modalState, setModalState] = useState<boolean>(false);
    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);

    const classes = useStyles({ nodeType: node.nodeType, nodeText: node.text, checked: node.isCritical, isDecendentOfSplitMerge, splitMergeRootSiblingIndex });

    useEffect(() => {
        if (isNullOrUndefinedOrWhitespace(previousChildren)) {
            setPreviousChildren(node.nodeChildrenString);
        }
    }, []);

    const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
        if (event.target.checked){
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
            newNode.shouldRenderChildren = !event.target.checked;
            updatedNodeList = _truncateTheTreeAtSpecificNode(node, nodeList);
            newNode.nodeChildrenString = nodeIdOfMostRecentSplitMergePrimarySibling;
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
            setMergeBoxChecked(event.target.checked);
            setNodes(cloneDeep(updatedNodeList));
        } else {
            historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
        }
    };

    // TODO: the NodeTypeSelector uses 'checkedNodeOptionList and re-computes whether or not this is a child of mergesplit, and if its a non-primary-sibling decendant (in which case, it will allow filtering). This is defensive
    // which is not necessary if this is the only place this is used. Its cheap to perform this check.
    return (
        <Card className={classNames(classes.root, node.nodeId)} variant="outlined">
            <CardContent className={classes.card}>
                <Typography className={classes.interfaceElement} variant={node.isRoot ? "h5" : "body1"} align="center">
                    {node.isRoot ? "Begin" : node.optionPath === "Continue" ? optionPath : "If " + optionPath}
                </Typography>
                <Card elevation={0} className={classNames(classes.interfaceElement, classes.textCard)} onClick={() => setModalState(true)}>
                    <Typography className={classes.text} variant="body2" component="span" noWrap={false}>
                        {node.text}
                    </Typography>
                    <Typography align="center" className={classes.editorStyle} onClick={() => setModalState(true)}>
                        Click to Edit
                    </Typography>
                </Card>
                <NodeTypeSelector
                    nodeOptionList={mergeBoxChecked ? checkedNodeOptionList(nodeOptionList, isDecendentOfSplitMerge, splitMergeRootSiblingIndex) : nodeOptionList}
                    node={node}
                    parentNode={parentNode}
                    parentState={parentState}
                    changeParentState={changeParentState}
                />
                {!node.isTerminalType && (
                    <FormControlLabel
                        className={classes.formstyle}
                        classes={{
                            label: classes.formLabelStyle,
                        }}
                        control={<Checkbox className={classes.formstyle} size="small" checked={node.isCritical} value="" name={"crit-" + node.nodeId} onChange={showResponseInPdfCheckbox} />}
                        label="Show response in PDF"
                    />
                )}
                {isDecendentOfSplitMerge && splitMergeRootSiblingIndex > 0 && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType && (
                    <FormControlLabel
                        className={classes.formstyle}
                        classes={{
                            label: classes.formLabelStyle,
                        }}
                        control={<Checkbox className={classes.formstyle} size="small" checked={!node.shouldRenderChildren} value="" name={"crit-" + node.nodeId + "-merge"} onChange={handleMergeBackInOnClick} />}
                        label="Merge with primary sibling branch"
                    />
                )}
                {isDecendentOfSplitMerge && splitMergeRootSiblingIndex === 0 && decendentLevelFromSplitMerge === 1 && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />
            </CardContent>
        </Card>
    );
};
