import { makeStyles, Card, CardContent, Button, Typography, Chip } from "@material-ui/core";
import { GroupNodeType, Groups } from "@Palavyr-Types";
import React, { useState, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import classNames from "classnames";
import { GroupNodeEditorModal } from "../GroupNodeEditor";

const useStyles = makeStyles({
    root: {
        minWidth: 275,
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
});

interface IGroupNodeInterface {
    node: GroupNodeType;
    text: string;
    setNodes: (nodeList: Groups) => void;
}

// holds the buttons and needs to elevate the state or somethign
export const GroupInterface = ({ node, text, setNodes}: IGroupNodeInterface) => {
    const [modalState, setModalState] = useState<boolean>(false);
    const [textState, setText] = useState<string>("");
    const classes = useStyles();
    var client = new ApiClient();

    useEffect(() => {
        setText(text);
    }, [text]);

    useEffect(() => {
        if (textState !== "") {
            node.text = textState;
        }
    }, [textState, node.text]) // is node.text really needed here?

    const classnames = classNames(classes.root, node.nodeId);

    return (
        <Card className={classnames} variant="outlined">
            <CardContent>
                <hr></hr>
                <Button variant="text" fullWidth>
                    <Typography align={"center"} variant="subtitle2" component="span" noWrap={false} onClick={() => setModalState(true)}>
                        {textState}
                    </Typography>
                </Button>
                <hr></hr>
                {
                    node.areaMeta.map(
                        areaMeta => <Chip
                            key={areaMeta.areaIdentifier}
                            variant="outlined"
                            color="primary"
                            label={areaMeta.areaName} onDelete={
                                async () => {
                                    var res = await client.Settings.Groups.DeleteAreaGroup(areaMeta.areaIdentifier);
                                    setNodes(res.data);
                                    console.log("Deleting")
                                }
                            } />
                    )
                }
                <GroupNodeEditorModal groupId={node.groupId} setModalState={setModalState} modalState={modalState} setText={setText} text={textState} setNodes={setNodes} />
            </CardContent>
        </Card>
    );
};