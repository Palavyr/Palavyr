import React, { useState, useCallback, useEffect } from "react";
import { Conversation, DynamicTableMetas, DynamicTableMeta } from "@Palavyr-Types";
import { getRootNode, getMissingNodes, addNodes } from "./nodes/conversationNodeUtils";
import { NodeTypeOptions } from "./nodes/NodeTypeOptions";
import { ApiClient } from "@api-client/Client";
import { TableData } from "../response/tables/dynamicTable/tableComponents/SelectOneFlat/SelectOneFlatTypes";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import "./ConvoTree.css";
import { makeStyles } from "@material-ui/core";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";

export interface IConvoTree {
    areaIdentifier: string;
    treeName: string;
}

export type RequiredDetails = {
    type: string;
    prettyName: string;
}

export const makeUniqueTableName = (tableMeta: DynamicTableMeta) => {
    return [tableMeta.tableType, tableMeta.tableId].join("-")
}

const useStyles = makeStyles(theme => ({
    conversation: {
        position: "static",
        overflow: "auto",
    }
}))

export const ConvoTree = ({ areaIdentifier, treeName }: IConvoTree) => {

    const [loaded, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Conversation>([]); // nodeList and state updater for the tree
    const rootNode = getRootNode(nodeList);
    const [dynamicNodeTypes, setDynamicNodeTypes] = useState<NodeTypeOptions>({});

    const [requiredNodes, setRequiredNodes] = useState<Array<RequiredDetails>>([]);
    const [missingNodeTypes, setMissingNodeTypes] = useState<Array<RequiredDetails>>([]);

    const classes = useStyles();

    const loadNodes = useCallback(async () => {
        var client = new ApiClient();
        var convoRes = await client.Conversations.GetConversation(areaIdentifier);
        var dynRes = await client.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);

        var dynamicTableMetas = dynRes.data as DynamicTableMetas;
        var nodes = convoRes.data as Conversation;

        var formattedRequiredNodes = dynamicTableMetas.map(x => {
            return {
                type: makeUniqueTableName(x),
                prettyName: [x.prettyName, x.tableTag].join(" - ")
            }
        })
        if (formattedRequiredNodes.length > 0) {
            setRequiredNodes(formattedRequiredNodes);
        } else {
            setRequiredNodes([])
        }

        if (formattedRequiredNodes.length > 0) {
            var dynamicNodeTypes = {};

            // used in the dropdown select menu in the convotree
            dynamicTableMetas.forEach(async (tableMeta: DynamicTableMeta) => {

                var dynamicTableRows = (await client
                    .Configuration
                    .Tables
                    .Dynamic
                    .getDynamicTableData(
                        areaIdentifier,
                        tableMeta.tableType,
                        tableMeta.tableId
                    )).data as TableData;

                var valueOptions = dynamicTableRows.map(x => x.option);

                var uniqueTableSpecifier = makeUniqueTableName(tableMeta);
                var splattable = {
                    [uniqueTableSpecifier]: { // key must be unique
                        value: uniqueTableSpecifier, // selectonOneFlat-hwhio-wjkr-ugiwer
                        pathOptions: tableMeta.valuesAsPaths ? valueOptions : ["Continue"],
                        valueOptions: valueOptions,
                        text: [tableMeta.prettyName, tableMeta.tableTag].join(" - ")
                    }
                }

                dynamicNodeTypes = {
                    ...dynamicNodeTypes,
                    ...splattable
                }
                setDynamicNodeTypes(dynamicNodeTypes);
            })
        }
        // eslint-disable-next-line
        setNodes(cloneDeep(nodes));

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false)
        }
    }, [areaIdentifier, loadNodes])

    useEffect(() => {
        if (nodeList.length > 0) {
            const nodeTypes = getMissingNodes(nodeList, requiredNodes);
            setMissingNodeTypes(nodeTypes);
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, nodeList])

    return (
        <>
            <ConversationHelp />
            <div className={classes.conversation}>
                {missingNodeTypes.length > 0 && <MissingDynamicNodes missingNodeTypes={missingNodeTypes} />}
                <form onSubmit={() => null}>
                    <fieldset className="fieldset" id="tree-test">
                        <legend>{treeName}</legend>
                        <div className="main-tree tree-wrap">
                            {
                                nodeList.length > 0
                                    ? <ConversationNode
                                        key={"tree-start"}
                                        parentId={rootNode.nodeId}
                                        node={rootNode}
                                        nodeList={nodeList}
                                        setNodes={setNodes}
                                        addNodes={addNodes}
                                        parentState={true}
                                        changeParentState={() => null}
                                        dynamicNodeTypes={dynamicNodeTypes}
                                    />
                                    : null
                            }
                        </div>
                    </fieldset>
                </form>
            </div>
        </>
    );
};
