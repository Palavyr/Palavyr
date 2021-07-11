import React from "react";
import { IPalavyrNode } from "../../Contracts";
import { AnabranchMergeCheckBox } from "../options/AnabranchMergeCheckBox";
import { AnabranchMergeNodeLabel } from "../options/AnabranchMergeNodeLabel";
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
        </>
    );
};
