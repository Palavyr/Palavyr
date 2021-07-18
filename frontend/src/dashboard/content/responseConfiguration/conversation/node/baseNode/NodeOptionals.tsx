import React from "react";
import { IPalavyrNode } from "../../Contracts";
import { AnabranchMergeCheckBox } from "../options/AnabranchMergeCheckBox";
import { AnabranchMergeNodeLabel } from "../options/AnabranchMergeNodeLabel";
import { DeleteMyself } from "../options/DeleteMyself";
import { InsertNodeChild } from "../options/InsertNodeChild";
import { ShowResponseInPdf } from "../options/ShowResponseInPdf";
import { UnsetNodeButton } from "../options/UnsetNodeButton";

export interface NodeOptionalsProps {
    currentNode: IPalavyrNode;
}

export const NodeOptionals = ({ currentNode }: NodeOptionalsProps) => {
    return (
        <>
            <ShowResponseInPdf node={currentNode} />
            <AnabranchMergeCheckBox node={currentNode} />
            <UnsetNodeButton node={currentNode} />
            <AnabranchMergeNodeLabel node={currentNode} />
            <div style={{ display: "flex", justifyContent: "space-between" }}>
                <InsertNodeChild node={currentNode} />
                <DeleteMyself node={currentNode} />
            </div>
        </>
    );
};
