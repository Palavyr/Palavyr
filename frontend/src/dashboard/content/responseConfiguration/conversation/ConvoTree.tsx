import React, { useState, useCallback, useEffect } from "react";
import { Conversation, NodeTypeOptions } from "@Palavyr-Types";
import { getRootNode } from "./nodes/conversationNodeUtils";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from '@material-ui/icons/Redo';

import "./ConvoTree.css";

const MaxConversationHistory = 50; // the number of times you can hit the back button

const useStyles = makeStyles(() => ({
    conversation: {
        position: "static",
        overflow: "auto",
    },
}));

export const ConvoTree = () => {
    var client = new ApiClient();
    const { setIsLoading } = React.useContext(DashboardContext);

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Conversation>([]); // nodeList and state updater for the tree
    const [nodeOptionList, setNodeOptionList] = useState<NodeTypeOptions>([]);
    const [missingNodeTypes, setMissingNodeTypes] = useState<string[]>([]);
    const [conversationHistory, setConversationHistory] = useState<Conversation[]>([]);
    const [conversationHistoryPosition, setConversationHistoryPosition] = useState<number>(0);

    const rootNode = getRootNode(nodeList);

    const classes = useStyles();

    const loadNodes = useCallback(async () => {
        const client = new ApiClient();

        const { data: nodes } = await client.Conversations.GetConversation(areaIdentifier);
        const { data: nodeOptionList } = await client.Conversations.GetNodeOptionsList(areaIdentifier);

        setNodeOptionList(nodeOptionList);
        setNodes(cloneDeep(nodes));
        setIsLoading(false);
        setConversationHistory([cloneDeep(nodes)]);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const onSave = async () => {
        const { data: updatedConversation } = await client.Conversations.ModifyConversation(nodeList, areaIdentifier);
        setNodes(updatedConversation);
        setConversationHistoryPosition(0);
        setConversationHistory([updatedConversation]);
        return true;
    };

    useEffect(() => {
        setIsLoading(true);
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    const getMissingNodes = useCallback(async () => {
        const client = new ApiClient();
        const { data: missingNodes } = await client.Conversations.GetMissingNodes(areaIdentifier);
        setMissingNodeTypes(missingNodes);
    }, []);

    useEffect(() => {
        if (nodeList.length > 0) {
            getMissingNodes();
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, nodeList]);

    const addConversationHistoryToQueue = (newConversationRecord: Conversation) => {
        const newPos = conversationHistoryPosition + 1;

        if (conversationHistory.length < MaxConversationHistory) {
            if (newPos < conversationHistory.length - 1) {
                setConversationHistory([...conversationHistory.slice(0, newPos), newConversationRecord]);
            } else {
                setConversationHistory([...conversationHistory, newConversationRecord]);
            }
        } else {
            if (newPos < MaxConversationHistory) {
                setConversationHistory([...conversationHistory.slice(0, newPos), newConversationRecord]);
            } else {
                setConversationHistory([...conversationHistory.slice(1), newConversationRecord]);
            }
        }

        setConversationHistoryPosition(newPos);
    };

    const stepConversationBackOneStep = (conversationHistoryPosition: number, conversationHistory: Conversation[]) => {
        if (conversationHistoryPosition === 0) {
            alert("Currently at the beginning the history.");
            return;
        }
        const newPosition = conversationHistoryPosition - 1;
        setConversationHistoryPosition(newPosition);
        setNodes(conversationHistory[newPosition]);
        console.log(newPosition);
    };

    const stepConversationForwardOneStep = (conversationHistoryPosition: number, conversationHistory: Conversation[]) => {
        const newPosition = conversationHistoryPosition + 1;
        if (newPosition <= conversationHistory.length - 1) {
            setNodes(conversationHistory[newPosition]);
            setConversationHistoryPosition(newPosition);
            console.log(newPosition);
        } else {
            alert("Currently at the end of the history.");
        }
    };

    return (
        <ConversationTreeContext.Provider value={{ nodeList, setNodes, conversationHistory, setConversationHistory: addConversationHistoryToQueue }}>
            <AreaConfigurationHeader
                divider={missingNodeTypes.length > 0}
                title="Palavyr"
                subtitle="Your palavyr is the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
            />
            <AlignCenter>
                <SaveOrCancel onSave={onSave} useModal />
                <Button
                    variant="outlined"
                    style={{marginLeft: "0.7rem", borderRadius: "10px"}}
                    startIcon={<UndoIcon />}
                    onClick={() => {
                        stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Undo
                </Button>
                <Button
                    variant="outlined"
                    style={{marginLeft: "0.7rem", borderRadius: "10px"}}
                    endIcon={<RedoIcon />}
                    onClick={() => {
                        stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Redo
                </Button>
            </AlignCenter>
            <div className={classes.conversation}>
                <div style={{ margin: "0.5rem 0rem 1rem 2rem" }}>{missingNodeTypes.length > 0 && <MissingDynamicNodes missingNodeTypes={missingNodeTypes} />}</div>
                <form onSubmit={() => null}>
                    <fieldset className="fieldset" id="tree-test">
                        <div className="main-tree tree-wrap">
                            {nodeList.length > 0 ? <ConversationNode key="tree-start" parentId={rootNode.nodeId} node={rootNode} parentState={true} changeParentState={() => null} nodeOptionList={nodeOptionList} /> : null}
                        </div>
                    </fieldset>
                </form>
            </div>
        </ConversationTreeContext.Provider>
    );
};
