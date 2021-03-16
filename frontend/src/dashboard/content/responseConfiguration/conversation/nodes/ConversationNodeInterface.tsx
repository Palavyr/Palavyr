import { ConvoNode, SplitMergeMeta, NodeTypeOptions, AnabranchMeta } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography, FormControlLabel, Checkbox } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep, sum } from "lodash";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _createAndAddNewNodes, _getNodeById, _replaceNodeWithUpdatedNode, _truncateTheTreeAtSpecificNode } from "./nodeUtils/_coreNodeUtils";
import { useEffect } from "react";
import { checkedNodeOptionList, nodeMergesToPrimarySibling, updateSingleOptionType } from "./nodeUtils/commonNodeUtils";
import { uuid } from "uuidv4";
import { allOtherSplitMergeTypesAreResolved, AllNonTerminalLeavesReferenceThisNode, otherNodeAlreadySetAsAnabranchMerge, recursivelyReferenceCurrentNodeInNonTerminalLeafNodes, anyMultiChoiceTypesWithUnsetChildren } from "./nodeUtils/AnabranchUtils";
import { recursivelyDereferenceNodeIdFromChildren, recursivelyDereferenceNodeIdFromChildrenExcept } from "./nodeUtils/dereferenceUtils";

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

export interface IConversationNodeInterface extends SplitMergeMeta, AnabranchMeta {
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
    isDecendentOfAnabranch,
    decendentLevelFromAnabranch,
    nodeIdOfMostRecentAnabranch,
    isDirectChildOfAnabranch,
    isParentOfAnabranchMergePoint,
    isAncestorOfAnabranchMergePoint
}: IConversationNodeInterface) => {
    const { setNodes, nodeList, conversationHistory, historyTracker, conversationHistoryPosition } = React.useContext(ConversationTreeContext);
    const [modalState, setModalState] = useState<boolean>(false);
    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    const classes = useStyles({ nodeType: node.nodeType, nodeText: node.text, checked: node.isCritical, isDecendentOfSplitMerge, splitMergeRootSiblingIndex });

    const otherNodeAlreadySetAsMergeBranchBool = isDecendentOfAnabranch && otherNodeAlreadySetAsAnabranchMerge(nodeIdOfMostRecentAnabranch, nodeList, node.nodeId);
    const shouldDisabledNodeTypeSelector = isDecendentOfAnabranch && isAncestorOfAnabranchMergePoint;

    useEffect(() => {
        if (nodeMergesToPrimarySibling(node, isDecendentOfSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling)) {
            setMergeBoxChecked(true);
        }
    }, [node, nodeList]);

    useEffect(() => {
        if (node.isAnabranchMergePoint) {
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
            newNode.nodeChildrenString = nodeIdOfMostRecentSplitMergePrimarySibling;
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
            setMergeBoxChecked(event.target.checked);
            setNodes(cloneDeep(updatedNodeList));
        } else {
            if (conversationHistoryPosition === 0) {
                const childId = uuid();
                const newNode = cloneDeep(node);
                newNode.shouldRenderChildren = true;
                newNode.nodeChildrenString = childId;
                let updatedNodeList = _createAndAddNewNodes([childId], [childId], node, ["Continue"], nodeList, false);
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
            updatedNodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(newNode.nodeId, updatedNodeList, nodeIdOfMostRecentAnabranch);

            // if this box is available, then it means that we've passed the checks on whether or not there are unresolved splitandmerge type subtrees
            // need to find most recent Anabranch node, and then find all of the non-terminal leaf nodes. These will be assigned this nodes nodeId as their child node.
            const result = anyMultiChoiceTypesWithUnsetChildren(nodeIdOfMostRecentAnabranch, updatedNodeList);
            if (result.length > 0 && sum(result.map((x) => (x ? 1 : 0))) > 0) {
                alert("Please set any multioption types that have not been set under the most recent Anabranch node.");
                return;
            }

            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
            setAnabranchMergeChecked(true);
            setNodes(cloneDeep(updatedNodeList));
        } else {
            // if (conversationHistoryPosition === 0) {
            let updatedNodeList = cloneDeep(nodeList);
            const newNode = cloneDeep(node);
            newNode.isAnabranchMergePoint = false;
            updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
            const anabranchRootNode = _getNodeById(nodeIdOfMostRecentAnabranch, updatedNodeList);
            recursivelyDereferenceNodeIdFromChildrenExcept(parentNode?.nodeId,  anabranchRootNode, updatedNodeList, newNode.nodeId);
            setNodes(cloneDeep(updatedNodeList));
            setAnabranchMergeChecked(true);
            // } else {
            //     historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
            // }
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
                    nodeOptionList={(mergeBoxChecked && isDecendentOfSplitMerge) || (isParentOfAnabranchMergePoint) ? checkedNodeOptionList(nodeOptionList, isDecendentOfSplitMerge, splitMergeRootSiblingIndex, isParentOfAnabranchMergePoint) : nodeOptionList}
                    node={node}
                    parentState={parentState}
                    changeParentState={changeParentState}
                    shouldDisabledNodeTypeSelector={shouldDisabledNodeTypeSelector}
                />
                {!node.isTerminalType && !(node.nodeType === "ProvideInfo") && (
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
                {isDecendentOfAnabranch && node.nodeType !== "" && !node.isTerminalType && !node.isMultiOptionType && !isDirectChildOfAnabranch && !otherNodeAlreadySetAsMergeBranchBool && (
                    <FormControlLabel
                        className={classes.formstyle}
                        classes={{
                            label: classes.formLabelStyle,
                        }}
                        control={<Checkbox className={classes.formstyle} size="small" checked={anabranchMergeChecked} value="" name={"crit-" + node.nodeId + "-merge"} onChange={handleSetAsAnabranchMergePointClick} />}
                        label="Set as Anabranch merge point"
                    />
                )}
                {isDecendentOfSplitMerge && splitMergeRootSiblingIndex === 0 && decendentLevelFromSplitMerge === 1 && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />
            </CardContent>
        </Card>
    );
};

