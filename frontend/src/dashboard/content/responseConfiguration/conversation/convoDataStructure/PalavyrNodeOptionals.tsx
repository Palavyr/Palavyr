import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Tooltip, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useState } from "react";
import { NodeCheckBox } from "../nodes/nodeInterface/NodeCheckBox";
import { IPalavyrNode } from "./Contracts";
import { NodeCreator } from "./NodeCreator";

export class PalavyrNodeOptionals {
    private palavyrNode: IPalavyrNode;
    constructor(node: IPalavyrNode) {
        this.palavyrNode = node;
    }

    public renderSplitMergeAnchorLabel() {
        const shouldShow = this.palavyrNode.isPalavyrSplitmergeMergePoint;
        return () => {
            return shouldShow ? <Typography>This is the primary sibling. Branches will merge to this node.</Typography> : <></>;
        };
    }

    public renderAnabranchMergeCheckBox() {
        const disabled = this.palavyrNode.isPalavyrAnabranchStart && this.palavyrNode.isPalavyrAnabranchEnd;

        const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>) => {
            const checked = event.target.checked;
            const origin = this.palavyrNode.anabranchContext.anabranchOriginId;
            const anabranchOriginNode = this.palavyrNode.palavyrLinkedList.findNode(origin);

            if (checked) {
                this.palavyrNode.isPalavyrAnabranchEnd = true;
                anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(this.palavyrNode);
                setAnabranchMergeChecked(true);
            } else {
                this.palavyrNode.isPalavyrAnabranchEnd = false;
                setAnabranchMergeChecked(false);
                anabranchOriginNode.recursiveDereferenceThisAnabranchOrigin(this.palavyrNode);
            }
            this.palavyrNode.UpdateTree();
        };

        const shouldShow = () => {
            const isChildOfAnabranchType = this.palavyrNode.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
            return (
                this.palavyrNode.isPalavyrSplitmergeMember && !this.palavyrNode.isTerminal && !isChildOfAnabranchType && (this.palavyrNode.isMemberOfLeftmostBranch || this.palavyrNode.isPalavyrAnabranchStart)
            ); // && decendentLevelFromAnabranch < 4; TODO
        };

        return () => {
            const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

            return shouldShow() ? (
                <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
                    <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked)} />
                </Tooltip>
            ) : (
                <></>
            );
        };
    }

    public renderUnsetNodeButton() {
        const shouldShow = () => {
            return (
                this.palavyrNode.nodeIsSet() &&
                (!this.palavyrNode.isPalavyrAnabranchMember || this.palavyrNode.isAnabranchLocked) &&
                !this.palavyrNode.isPalavyrSplitmergeMergePoint &&
                !this.palavyrNode.isAnabranchLocked
            );
        };

        const onClick = () => {
            this.palavyrNode.removeSelf();
            this.palavyrNode.UpdateTree();
        };

        return () => {
            return shouldShow() ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
        };
    }

    public renderAnabranchMergeNodeLabel() {
        return () => {
            return this.palavyrNode.isPalavyrAnabranchEnd ? <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography> : <></>;
        };
    }

    public renderShowResponseInPdf() {
        const shouldShow = () => {
            const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
            return !this.palavyrNode.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(this.palavyrNode.nodeType);
        };
        const onChange = (event: { target: { checked: boolean } }) => {
            const checked = event.target.checked;
            this.palavyrNode.shouldPresentResponse = checked;
            this.palavyrNode.setTreeWithHistory(this.palavyrNode.palavyrLinkedList);
        };

        return () => {
            return shouldShow() ? <NodeCheckBox label="Show response in PDF" checked={this.palavyrNode.shouldPresentResponse} onChange={onChange} /> : <></>;
        };
    }

    public renderShowMergeWithPrimarySiblingBranchOption() {
        const shouldShow = () => {
            return (
                this.palavyrNode.isPalavyrSplitmergeMember &&
                this.palavyrNode.isPalavyrSplitmergePrimarybranch &&
                this.palavyrNode.nodeIsSet() &&
                !this.palavyrNode.isTerminal &&
                !this.palavyrNode.isMultiOptionType &&
                this.palavyrNode.isPenultimate()
            );
        };

        const onClick = async (event: { target: { checked: boolean } }, setMergeBoxChecked: SetState<boolean>) => {
            const checked = event.target.checked;
            setMergeBoxChecked(checked);
            if (checked) {
                this.palavyrNode.RouteToMostRecentSplitMerge();
            } else {
                // const thing = this.palavyrNode.parentNodeReferences.references[0]
                const nodeCreator = new NodeCreator();
                nodeCreator.addDefaultChild(this.palavyrNode, "Continue");
            }
        };

        return () => {
            const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(this.palavyrNode.isPalavyrSplitmergeEnd);
            return shouldShow() ? <NodeCheckBox label="Merge with primary sibling branch" checked={mergeBoxChecked} onChange={(event) => onClick(event, setMergeBoxChecked)} /> : <></>;
        };
    }
}
