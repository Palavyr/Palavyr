import { ConvoNode, Conversation, Responses } from "@Palavyr-Types";
import { NodeTypeOptions } from "./NodeTypeOptions";
import React, { useState } from "react";
import { ApiClient } from "@api-client/Client";
import { makeStyles, Card, CardContent, Typography, Button, FormControlLabel, Checkbox, TextField } from "@material-ui/core";
import classNames from "classnames";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { cloneDeep } from "lodash";
import { updateNodeList } from "./conversationNodeUtils";
import { NodeEditorModal } from "./nodeEditor/ConversationNodeEditor";


export interface IConversationNodeInterface {
    node: ConvoNode;
    nodeList: Array<ConvoNode>;
    addNodes: (parentNode: ConvoNode, nodeList: Conversation, newIDs: Array<string>, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void) => void;
    setNodes: (nodeList: Conversation) => void;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    optionPath: string | null;
    dynamicNodeTypes: NodeTypeOptions;
}

type StyleProps = {
    nodeText: string;
    nodeType: string;
}
const useStyles = makeStyles(theme => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: "300px",
        // maxHeight: "350px",
        borderColor: props.nodeType === "" ? "Red" : "lightgray",
        borderWidth: props.nodeType === "" ? "5px" : "2px",
        borderRadius: "3px"
    }),
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
    textCard: (props: StyleProps) => {
        return {
            padding: "10px",
            textAlign: "left",
            // minHeight: "50px",
            background: (props.nodeText === "Ask your question!") ? "red" : "white",
        }
    },
    text: {
        margin: ".1rem",
        fontSize: "12px"
    },
    formstyle: {
        fontSize: "12px"
    }
}))


export const ConversationNodeInterface = ({ dynamicNodeTypes, node, nodeList, optionPath, addNodes, setNodes, parentState, changeParentState }: IConversationNodeInterface) => {

    const [modalState, setModalState] = useState<boolean>(false);

    var client = new ApiClient();
    const classes = useStyles({nodeType: node.nodeType, nodeText: node.text})

    return (
        <Card className={classNames(classes.root, node.nodeId)} variant="outlined">
            <CardContent>
                <Typography variant={node.isRoot ? "h5" : "body1"} align={"center"}>
                    {node.isRoot ? "Convo Start" : optionPath}
                </Typography>
                <NodeTypeSelector dynamicNodeTypes={dynamicNodeTypes} node={node} nodeList={nodeList} addNodes={addNodes} setNodes={setNodes} parentState={parentState} changeParentState={changeParentState} />
                <hr></hr>
                <Card className={classes.textCard} onClick={() => setModalState(true)}>
                    <Typography className={classes.text}variant="body2" component="span" noWrap={false} >
                        {node.text}
                    </Typography>
                </Card>
                <hr></hr>
                <FormControlLabel
                    className={classes.formstyle}
                    control={
                        <Checkbox
                            className={classes.formstyle}
                            size="small"
                            checked={node.isCritical}
                            value={"WTF"}
                            name={"crit-" + node.nodeId}
                            onChange={async (event) => {
                                var newNode = cloneDeep(node);
                                newNode.isCritical = event.target.checked;
                                await client.Conversations.PutConversationNode(node.nodeId, newNode);
                                var newNodeList = updateNodeList(nodeList, newNode);
                                setNodes(newNodeList);
                            }}
                        />}
                    label="Show response in PDF"
                />
                <NodeEditorModal setModalState={setModalState} modalState={modalState} node={node} nodeList={nodeList} setNodes={setNodes} changeParentState={changeParentState} parentState={parentState} />
            </CardContent>
        </Card>
    );
};
