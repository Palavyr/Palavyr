import { ConvoNode, MostRecentSplitMerge, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography, FormControlLabel, Checkbox } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep } from "lodash";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _replaceNodeWithUpdatedNode } from "./nodeUtils/_coreNodeUtils";
import { useEffect } from "react";

export interface IConversationNodeInterface extends MostRecentSplitMerge {
    node: ConvoNode;
    parentNode: ConvoNode | null;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    optionPath: string | null;
    nodeOptionList: NodeTypeOptions;
}

type StyleProps = {
    nodeText: string;
    nodeType: string;
    checked: boolean;
    isChildOfSplitMerge: boolean;
    splitMergeRootSiblingIndex: number;
};

const useStyles = makeStyles(() => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: "300px",
        borderColor: props.nodeType === "" ? "Red" : props.isChildOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "purple" : "#54585A",
        borderWidth: props.nodeType === "" ? "5px" : props.isChildOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "8px" : "2px",
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

export const ConversationNodeInterface = ({
    nodeOptionList,
    node,
    parentNode,
    optionPath,
    parentState,
    changeParentState,
    isChildOfSplitMerge,
    decendentLevelFromSplitMerge,
    splitMergeRootSiblingIndex,
    nodeIdOfMostRecentSplitMergePrimarySibling,
}: IConversationNodeInterface) => {
    const { setNodes, nodeList } = React.useContext(ConversationTreeContext);

    const [previousChildren, setPreviousChildren] = useState<string>("");
    const [modalState, setModalState] = useState<boolean>(false);
    const classes = useStyles({ nodeType: node.nodeType, nodeText: node.text, checked: node.isCritical, isChildOfSplitMerge, splitMergeRootSiblingIndex });

    useEffect(() => {
        setPreviousChildren(node.nodeChildrenString);
    }, []);
    const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
        const newNode = cloneDeep(node);
        newNode.isCritical = event.target.checked;

        const updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
        setNodes(updatedNodeList);
    };

    const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
        const newNode = cloneDeep(node);

        newNode.shouldRenderChildren = !event.target.checked;
        if (splitMergeRootSiblingIndex > 0) {
            if (event.target.checked) {
                newNode.nodeChildrenString = nodeIdOfMostRecentSplitMergePrimarySibling;
            } else {
                newNode.nodeChildrenString = previousChildren;
            }
        }
        const updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
        setNodes(updatedNodeList);
    };

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
                <NodeTypeSelector nodeOptionList={nodeOptionList} node={node} parentNode={parentNode} parentState={parentState} changeParentState={changeParentState} />
                <FormControlLabel
                    className={classes.formstyle}
                    classes={{
                        label: classes.formLabelStyle,
                    }}
                    control={<Checkbox className={classes.formstyle} size="small" checked={node.isCritical} value="" name={"crit-" + node.nodeId} onChange={showResponseInPdfCheckbox} />}
                    label="Show response in PDF"
                />
                {isChildOfSplitMerge && splitMergeRootSiblingIndex > 0 && (
                    <FormControlLabel
                        className={classes.formstyle}
                        classes={{
                            label: classes.formLabelStyle,
                        }}
                        control={<Checkbox className={classes.formstyle} size="small" checked={!node.shouldRenderChildren} value="" name={"crit-" + node.nodeId + "-merge"} onChange={handleMergeBackInOnClick} />}
                        label="Merge with primary sibling branch"
                    />
                )}
                {isChildOfSplitMerge && splitMergeRootSiblingIndex === 0 && decendentLevelFromSplitMerge === 1 && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />
            </CardContent>
        </Card>
    );
};
