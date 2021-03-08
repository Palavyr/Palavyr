import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState } from "react";
import { makeStyles, Card, CardContent, Typography, FormControlLabel, Checkbox } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep } from "lodash";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { _replaceNodeWithUpdatedNode } from "./nodeUtils/commonNodeUtils";

export interface IConversationNodeInterface {
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
};
const useStyles = makeStyles(() => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: "300px",
        borderColor: props.nodeType === "" ? "Red" : "#54585A",
        borderWidth: props.nodeType === "" ? "5px" : "2px",
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

export const ConversationNodeInterface = ({ nodeOptionList, node, parentNode, optionPath, parentState, changeParentState }: IConversationNodeInterface) => {
    const { setNodes, nodeList } = React.useContext(ConversationTreeContext);

    const [modalState, setModalState] = useState<boolean>(false);
    const classes = useStyles({ nodeType: node.nodeType, nodeText: node.text, checked: node.isCritical });

    const showResponseInPdfCheckbox = async (event: { target: { checked: boolean } }) => {
        const newNode = cloneDeep(node);
        newNode.isCritical = event.target.checked;

        const filteredNodeList = _replaceNodeWithUpdatedNode(newNode, nodeList);
        setNodes(filteredNodeList);
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
                <ConversationNodeEditor setModalState={setModalState} modalState={modalState} node={node} parentNode={parentNode} />
            </CardContent>
        </Card>
    );
};
