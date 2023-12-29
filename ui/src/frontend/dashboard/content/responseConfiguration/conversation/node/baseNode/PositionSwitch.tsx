import React, { useContext } from "react";
import ArrowLeftIcon from "@material-ui/icons/ArrowLeft";
import ArrowRightIcon from "@material-ui/icons/ArrowRight";
import { makeStyles } from "@material-ui/core";
import { IPalavyrNode } from "@Palavyr-Types";
import { ConversationTreeContext, DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { NodeTypeOptionResources } from "@Palavyr-Types";

export interface IPositionSwitcherProps {
    currentNode: IPalavyrNode;
}

class PositionSwapper {
    constructor(currentNode: IPalavyrNode) {
        this.SetSwappers(currentNode);
    }
    private left: boolean = false;
    private right: boolean = false;
    private show: boolean = true;
    private positionIndex: number;

    private dontShow() {
        this.show = false;
    }

    public get Show(): boolean {
        return this.show;
    }

    public set Show(v: boolean) {
        this.show = v;
    }

    public get Left(): boolean {
        return this.left;
    }

    public set Left(v: boolean) {
        this.left = v;
    }

    public get Right(): boolean {
        return this.right;
    }

    public set Right(v: boolean) {
        this.right = v;
    }

    public get PositionIndex(): number {
        return this.positionIndex;
    }

    public set PositionIndex(index: number) {
        this.positionIndex = index;
    }

    private SetSwappers = (currentNode: IPalavyrNode): void => {
        if (currentNode.isRoot) {
            this.dontShow();
            return;
        }

        if (currentNode.parentNodeReferences.Length !== 1) {
            this.dontShow();
            return;
        }

        const parent = currentNode.parentNodeReferences.Single();

        if (!parent.isMultiOptionType) {
            this.dontShow();
            return;
        }

        if (parent.childNodeReferences.Length < 2) {
            this.dontShow();
            return;
        }

        const currentNodeIndex = parent.childNodeReferences.findIndexOf(currentNode);
        if (currentNodeIndex === null) return;

        this.PositionIndex = currentNodeIndex;
        if (parent.nodeType === "Anabranch" || parent.nodeType === "LoopbackAnchor") {
            if (parent.childNodeReferences.Length <= 2) {
                this.Right = false;
                this.Left = false;
            } else if (currentNodeIndex === 0) {
                this.Right = false;
                this.Left = false;
            } else if (currentNodeIndex === 1) {
                // can only move right
                this.Right = true;
            } else if (currentNodeIndex === parent.childNodeReferences.Length - 1) {
                // can only move left
                this.Left = true;
            } else {
                // can move left or right
                this.Right = true;
                this.Left = true;
            }
        } else {
            if (currentNodeIndex === 0) {
                this.Right = true;
            } else if (currentNodeIndex === parent.childNodeReferences.Length - 1) {
                this.Left = true;
            } else {
                this.Right = true;
                this.Left = true;
            }
        }
    };
}

const rightArrowOnClick = (event: any, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) => {
    currentNode.parentNodeReferences.Single().childNodeReferences.ShiftRight(currentNode);
    // currentNode.UpdateTree();
    currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
};

const leftArrowOnClick = (event: any, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) => {
    currentNode.parentNodeReferences.Single().childNodeReferences.ShiftLeft(currentNode);
    currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
};

export const PositionSwitcher = ({ currentNode }: IPositionSwitcherProps) => {
    const cls = useStyles();
    const positions = new PositionSwapper(currentNode);
    const { nodeTypeOptions } = useContext(ConversationTreeContext);

    return positions.Show ? (
        <div className={cls.arrows}>
            <span>{positions.Left && <ArrowLeftIcon onClick={(event: any) => leftArrowOnClick(event, currentNode, nodeTypeOptions)} className={cls.arrow} fontSize="large" />}</span>

            {/* <span>TODO: Text to indicate which position in the option list this will be</span> */}

            <span>{positions.Right && <ArrowRightIcon onClick={(event: any) => rightArrowOnClick(event, currentNode, nodeTypeOptions)} className={cls.arrow} fontSize="large" />}</span>
        </div>
    ) : (
        <></>
    );
};


const useStyles = makeStyles<{}>((theme: any) => ({
    arrows: {
        display: "flex",
        justifyContent: "space-between",
        width: "100%",
    },
    arrow: {
        "&:hover": {
            cursor: "pointer",
            background: "gray",
            borderRadius: "10px",
        },
    },
}));
