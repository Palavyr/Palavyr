import { Conversation, ConvoNode, NodeIdentity } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography } from "@material-ui/core";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _createAndAddNewNodes, _getLeftMostParentNode, _getNodeById, _getParentNode, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString, _truncateTheTreeAtSpecificNode } from "../nodeUtils/_coreNodeUtils";
import { useEffect } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeCheckBox } from "./NodeCheckBox";
import { DataLogging } from "./nodeDebug/DataLogging";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { NodeTypeSelector } from "./nodeSelector/NodeTypeSelector";
import { filteredNodeTypeOptions } from "./nodeTypeFilter";
import { debugDataItems, debugNodeProperties } from "./nodeDebug/compileDebugData";
import { _handleMergeBackInOnClick } from "./nodeInterfaceCallbacks/_handleMergeBackInOnClick";
import { _showResponseInPdfCheckbox } from "./nodeInterfaceCallbacks/_showResponseInPdfCheckbox";
import { setNodeAsAnabranchMergePoint, _handleSetAsAnabranchMergePointClick } from "./nodeInterfaceCallbacks/_handleSetAsAnabranchMergePointClick";
import { _handleUnsetCurrentNodeType } from "./nodeInterfaceCallbacks/_handleUnsetCurrentNodeType";

type StyleProps = {
    nodeText: string;
    nodeType: string;
    checked: boolean;
    isDecendentOfSplitMerge: boolean;
    splitMergeRootSiblingIndex: number;
    debugOn: boolean;
};

const useStyles = makeStyles(() => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: props.debugOn ? "600px" : "250px",
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
        debugOn: showDebugData,
    });

    const dataItems = [...debugDataItems(identity), ...[{ anabranchMergeChecked: anabranchMergeChecked }, { conversationHistoryPosition: conversationHistoryPosition }, { modalState: modalState }, { mergeBoxChecked: mergeBoxChecked }]];
    const nodeProperties = debugNodeProperties(node);

    useEffect(() => {
        setMergeBoxChecked(identity.shouldCheckSplitMergeBox);
    }, [node, nodeList]);

    useEffect(() => {
        if (identity.isAnabranchMergePoint) {
            setAnabranchMergeChecked(true);
        }
    }, []);

    const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        _showResponseInPdfCheckbox(checked, node, nodeList, setNodes, conversationHistoryPosition, historyTracker, conversationHistory);
    };

    const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        _handleMergeBackInOnClick(checked, node, nodeList, conversationHistoryPosition, historyTracker, conversationHistory, setNodes, setMergeBoxChecked, identity.nodeIdOfMostRecentSplitMergePrimarySibling);
    };

    const handleSetAsAnabranchMergePointClick = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        _handleSetAsAnabranchMergePointClick(checked, node, nodeList, identity.nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked, setNodes);
    };

    const handleUnsetCurrentNodeType = () => {
        _handleUnsetCurrentNodeType(node, nodeList, setNodes);
    };

    const selectionCallback = (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string): Conversation => {
        return setNodeAsAnabranchMergePoint(node, nodeList, nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked);
    }

    return (
        <Card className={classNames(classes.root, node.nodeId)} variant="outlined">
            <CardContent className={classes.card}>
                {showDebugData && <DataLogging nodeProperties={nodeProperties} nodeChildren={node.nodeChildrenString} nodeId={node.nodeId} data={dataItems} />}
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
                <NodeTypeSelector nodeIdentity={identity} nodeTypeOptions={filteredNodeTypeOptions(identity, nodeTypeOptions)} node={node} reRender={reRender} shouldDisabledNodeTypeSelector={identity.shouldDisabledNodeTypeSelector} selectionCallback={selectionCallback} />
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />

                {identity.shouldShowResponseInPdfOption && <NodeCheckBox label="Show response in PDF" checked={node.isCritical} onChange={showResponseInPdfCheckbox} />}
                {identity.shouldShowMergeWithPrimarySiblingBranchOption && <NodeCheckBox label="Merge with primary sibling branch" checked={!node.shouldRenderChildren} onChange={handleMergeBackInOnClick} />}
                {identity.shouldShowSetAsAnabranchMergePointOption && <NodeCheckBox disabled={node.isAnabranchType && node.isAnabranchMergePoint} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={handleSetAsAnabranchMergePointClick} />}
                {identity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}
                {identity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                {identity.shouldShowAnabranchMergepointLabel && <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography>}
            </CardContent>
        </Card>
    );
};
