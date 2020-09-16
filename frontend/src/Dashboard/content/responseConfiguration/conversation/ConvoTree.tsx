import { makeStyles } from "@material-ui/core";
import React, { useState, useCallback, useEffect } from "react";
import { Conversation, DynamicTableMetas, DynamicTableMeta } from "@Palavyr-Types";
import { getRootNode, getMissingNodes, removeNodes, addNodes } from "./nodes/conversationNodeUtils";
import { NodeTypeOptions } from "./nodes/NodeTypeOptions";
import { ApiClient } from "@api-client/Client";
import { TableData } from "../response/tables/dynamicTable/tableComponents/SelectOneFlat/SelectOneFlatTypes";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import classNames from "classnames";
import "./ConvoTree.css";


export interface IConvoTree {
    areaIdentifier: string;
    treeName: string;
}

// const useStyles = makeStyles(
//     {
        // fieldset: {
        //     position: "relative",
        //     borderWidth: "1px",
        //     borderStyle: "solid",
        //     marginBottom: "10px",
        //     padding: "50px"
        // },

        // treeTest: {
        //     position: "relative"
        // },

        // treeWrap: {
        //     display: "flex",
        //     flexDirection: "column",
        //     alignItems: "flex-start",
        //     position: "relative"
        // }
//     }
// )

export type RequiredDetails = {
    type: string;
    prettyName: string;
}


export const makeUniqueTableName = (tableMeta: DynamicTableMeta) => {
    return [tableMeta.tableType, tableMeta.tableId].join("-")
}

export const ConvoTree = ({ areaIdentifier, treeName }: IConvoTree) => {

    // const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Conversation>([]); // nodeList and state updater for the tree
    const rootNode = getRootNode(nodeList);
    const [dynamicNodeTypes, setDynamicNodeTypes] = useState<NodeTypeOptions>({});

    const [requiredNodes, setRequiredNodes] = useState<Array<RequiredDetails>>([]);
    const [missingNodeTypes, setMissingNodeTypes] = useState<Array<RequiredDetails>>([]);

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
        }

        if (formattedRequiredNodes.length > 0) {
            var dynamicNodeTypes = {};

            // used in the dropdown select menu in the convotree
            dynamicTableMetas.forEach(async (tableMeta: DynamicTableMeta) => {

                var res = await client
                    .Configuration
                    .Tables
                    .Dynamic
                    .getDynamicTableData(
                        areaIdentifier,
                        tableMeta.tableType,
                        tableMeta.tableId
                    );
                var dynamicTableRows = res.data as TableData;

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
        setNodes(cloneDeep(nodes));
        setLoaded(true);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
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
        </>
    );
};
