import { Groups, GroupNodeType } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import React, { useState, useEffect } from "react";
import { uuid } from "uuidv4";
import { getChildNodes } from "../groupNodeUtils";


export interface IGroupNode {
    nodeList: Groups;
    node: GroupNodeType;
    addGroup: (parentNode: GroupNodeType, nodeList: Groups, newIDs: Array<string>, setNodes: (nodeList: Groups) => void) => void;
    removeNodes: (nodeList: Groups, nodeID: string, setNodes: (nodeList: Groups) => void) => void;
    setNodes: (nodeList: Groups) => void;
    createGroupTreeStep: any;
    children: React.ReactNode;
}

export type lineStyle = {
    borderColor: "black";
    borderStyle: "solid";
    borderWidth: number;
    zIndex: number;
};

export const connectionStyle: lineStyle = {
    borderColor: "black",
    borderStyle: "solid",
    borderWidth: 1,
    zIndex: 0,
};


const useStyles = makeStyles({
    treeItem: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        fontSize: "7px",
    },
    treeBlockWrap: {
        padding: "1rem 1rem 1rem 1rem",
    },
    treeRow: {
        fontSize: "7px",
        display: "flex",
        flexDirection: "row",
        alignItems: "flex-start",
    }
})


export const GroupNode = ({ nodeList, node, addGroup, removeNodes, setNodes, createGroupTreeStep, children }: IGroupNode) => {

    const classes = useStyles();

    const [nodeState, changeNodeState] = useState<boolean>(true);
    const [, setLoaded] = useState(false)

    let childNodes = getChildNodes(node.nodeChildrenString, nodeList);

    useEffect(() => {
        setLoaded(true)
        return () => setLoaded(false)
    }, []);

    const nodeWrapper = "tree-item-" + node.nodeId;

    if (node === null) {
        return null
    }

    return (
        <>
            <div className={"tree-item " + nodeWrapper}>
                <div className="tree-block-wrap">
                    {children}
                </div>
                {
                    childNodes.length > 0 && ( // if there are childNodes, then render them.
                        <div key={uuid()} className="tree-row">
                            {
                                childNodes.map((n) => createGroupTreeStep(n, addGroup))
                            }
                        </div>
                    )
                }
            </div>
        </>
    );
};
