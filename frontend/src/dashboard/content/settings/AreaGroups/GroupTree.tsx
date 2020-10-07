import { GroupRow, GroupTable, AreaMeta, Groups, GroupNodeType, Areas, AlertType, AreaTable } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { getRootNodes } from "./groupNodeUtils";
import { cloneDeep } from "lodash";
import { GroupNode } from "./nodes/GroupNode";
import { GroupInterface } from "./nodes/GroupNodeInterface";
import { Statement } from "@common/components/Statement";
import { Button, Divider, Grid } from "@material-ui/core";
import { UnassignedAreas } from "./UnassignedAreas";
import { v4 as uuid } from "uuid";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";


const getNodeChildrenString = (group: GroupRow, groups: GroupTable) => {

    // find all ndes that reference THIS node
    var children = groups.filter(r => r.parentId === group.groupId)

    let childrenString = "";
    if (children.length > 0) {
        childrenString = children.map(r => r.groupId).join(",");
    }
    return childrenString;
}

const getAreaMeta = (groupRow: GroupRow, areaMetas: Array<AreaMeta>) => {
    var areasInThisNode = areaMetas.filter((a: AreaMeta) => groupRow.groupId === a.groupId)
    return areasInThisNode;
}

const assembleNodeList = (groups: GroupTable, areaMetas: Array<AreaMeta>) => {
    var nodeList: Groups = [];
    groups.forEach((groupRow: GroupRow) => {

        var childrenString = getNodeChildrenString(groupRow, groups);
        var areaMeta = getAreaMeta(groupRow, areaMetas);

        var newNode: GroupNodeType = {
            nodeId: groupRow.groupId,
            parentId: groupRow.parentId,
            nodeChildrenString: childrenString,
            isRoot: groupRow.parentId === null,
            id: groupRow.id,
            text: groupRow.groupName,
            optionPath: "",
            areaMeta: areaMeta,
            groupId: groupRow.groupId
        }
        nodeList.push(newNode);
    });
    return nodeList
}

export type NullArea = {
    areaIdentifier: string;
    areaName: string;
}

export type AreaChips = {
    areaIdentifier: string;
    areaName: string;
}

const getNullAreaChips = (areas: Areas) => areas.filter(x => x.groupId === null);


interface IAddGroup {
    parentId: string | null;
}


export const GroupTree = () => {
    var client = new ApiClient();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Groups>([]);
    const [nullAreas, setNullAreas] = useState<Array<NullArea>>([]);
    const [alertState, setAlert] = useState<boolean>(false);
    const [alertMessage, setAlertMessage] = useState<AlertType>({ title: '', message: '' });
    const [groups, setGroups] = useState<Array<GroupRow>>([]);

    var rootNodes = getRootNodes(nodeList);

    const addGroup = async (parentId: string | null = null) => {
        if (!(nodeList.length >= 6)) {

            var groups = await client.Settings.Groups.AddGroup(parentId, "Default Group Name")
            var areas = await client.Area.GetAreas();
            setData(groups.data, areas.data);
        } else {
            // alert("Can only have up to 6 groups.")
            var alertMessage: AlertType = {
                title: "Stop!",
                message: "Only up to 6 groups allowed."
            }
            setAlertMessage(alertMessage);
            setAlert(true);
        }
    }

    const setData = (groups: GroupTable, areas: Areas) => {
        var areaMetas = areas.map((a: AreaTable) => {
            return {
                areaIdentifier: a.areaIdentifier,
                groupId: a.groupId,
                areaName: a.areaName
            }
        })

        var nodeList = assembleNodeList(groups, areaMetas);
        var nullAreas = getNullAreaChips(areas);

        setNullAreas(nullAreas);
        setNodes(cloneDeep(nodeList));
        setGroups(groups);
    }

    const loadNodes = useCallback(async () => {

        var groups = await client.Settings.Groups.GetGroups();
        var areas = await client.Area.GetAreas();
        setData(groups.data, areas.data)

    }, [])

    useEffect(() => {
        if (loaded === false) {
            loadNodes();
            setLoaded(true);
        }

        return () => {
            setLoaded(false)
        }
    }, [nodeList, loadNodes])


    const createGroupTreeStep = (node, addGroup) => {

        return (
            <GroupNode
                key={node.nodeId}
                addGroup={() => null}
                removeNodes={() => null}
                setNodes={setNodes}
                node={node}
                nodeList={nodeList}
                createGroupTreeStep={createGroupTreeStep}
            >
                <GroupInterface
                    node={node} // node object
                    setNodes={setNodes}
                    text={node.text} // node text to ask in chat
                />
            </GroupNode>
        )
    }

    return (
        <>
            <Statement title={"Area Groups"} details={"These groups organize area content for display in the chat widget."} />
            <Button color="primary" variant="outlined" onClick={() => addGroup(null)}>Add Group</Button>
            <form onSubmit={(e) => e.preventDefault()}>
                <fieldset className="fieldset" id="tree-test">
                    <legend>Area Groups</legend>
                    {nullAreas.length > 0 && <UnassignedAreas nullAreas={nullAreas} groups={groups} setNodes={setNodes} />}
                    <Divider variant="fullWidth" />
                    <h3>Assigned Areas</h3>
                    <div className="main-tree tree-wrap">
                        <Grid container spacing={0}>
                            {
                                (rootNodes.length > 0) && (nodeList.length > 0)
                                && rootNodes.map(rootNode => {
                                    return (
                                        <Grid key={uuid()} item xs={4}>
                                            {createGroupTreeStep(rootNode, addGroup)}
                                        </Grid>
                                    )
                                }
                                )
                            }
                        </Grid>

                    </div>
                </fieldset>
            </form>
            <CustomAlert alertState={alertState} setAlert={setAlert} alert={alertMessage} />
        </>
    );
};
