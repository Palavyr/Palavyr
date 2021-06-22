import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Tooltip, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useState } from "react";
import { NodeCheckBox } from "../nodes/nodeInterface/NodeCheckBox";
import { IPalavyrNode } from "./Contracts";
import { NodeCreator } from "./NodeCreator";


export interface NodeOptionalProps {
    node: IPalavyrNode;
}

export const AnabranchMergeNodeLabel = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrAnabranchEnd;
    return shouldShow ? <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography> : <></>;
};

export const ShowResponseInPdf = ({ node }: NodeOptionalProps) => {
    const shouldShow = () => {
        const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
        return !node.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(node.nodeType);
    };
    const onChange = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        node.shouldPresentResponse = checked;
        node.setTreeWithHistory(node.palavyrLinkedList);
    };

    return shouldShow() ? <NodeCheckBox label="Show response in PDF" checked={node.shouldPresentResponse} onChange={onChange} /> : <></>;
};

export const UnsetNodeButton = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.nodeIsSet() && (!node.isPalavyrAnabranchMember || node.isAnabranchLocked) && !node.isPalavyrSplitmergeMergePoint && !node.isAnabranchLocked;

    const onClick = () => {
        node.removeSelf();
        node.UpdateTree();
    };

    return shouldShow ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};

export const ShowMergeWithPrimarySiblingBranchOption = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrSplitmergeMember && node.isPalavyrSplitmergePrimarybranch && node.nodeIsSet() && !node.isTerminal && !node.isMultiOptionType && node.isPenultimate();

    const onClick = async (event: { target: { checked: boolean } }, setMergeBoxChecked: SetState<boolean>) => {
        const checked = event.target.checked;
        setMergeBoxChecked(checked);
        if (checked) {
            node.RouteToMostRecentSplitMerge();
        } else {
            const nodeCreator = new NodeCreator();
            nodeCreator.addDefaultChild(node, "Continue");
        }
    };

    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(node.isPalavyrSplitmergeEnd);
    return shouldShow ? <NodeCheckBox label="Merge with primary sibling branch" checked={mergeBoxChecked} onChange={(event) => onClick(event, setMergeBoxChecked)} /> : <></>;
};

export const SplitMergeAnchorLabel = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrSplitmergeMergePoint;
    return shouldShow ? <Typography>This is the primary sibling. Branches will merge to this node.</Typography> : <></>;
};

export const AnabranchMergeCheckBox = ({ node }: NodeOptionalProps) => {
    const disabled = node.isPalavyrAnabranchStart && node.isPalavyrAnabranchEnd;

    const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>) => {
        const checked = event.target.checked;
        const origin = node.anabranchContext.anabranchOriginId;
        const anabranchOriginNode = node.palavyrLinkedList.findNode(origin);

        if (checked) {
            node.isPalavyrAnabranchEnd = true;
            anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(node);
            setAnabranchMergeChecked(true);
        } else {
            node.isPalavyrAnabranchEnd = false;
            setAnabranchMergeChecked(false);
            anabranchOriginNode.recursiveDereferenceThisAnabranchOrigin(node);
        }
        node.UpdateTree();
    };

    const shouldShow = () => {
        const isChildOfAnabranchType = node.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
        const _shouldShow = node.isPalavyrAnabranchMember && !node.isTerminal && !isChildOfAnabranchType && (node.isMemberOfLeftmostBranch || node.isPalavyrAnabranchStart);
        return _shouldShow;
    };

    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    return shouldShow() ? (
        <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
            <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked)} />
        </Tooltip>
    ) : (
        <></>
    );
};
