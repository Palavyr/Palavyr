import React from "react";
import ArrowLeftIcon from "@material-ui/icons/ArrowLeft";
import ArrowRightIcon from "@material-ui/icons/ArrowRight";
import { makeStyles } from "@material-ui/core";
import { IPalavyrNode } from "../../Contracts";

export interface IPositionSwitcher {
    currentNode: IPalavyrNode;
}

class Positions {
    private left: boolean = false;
    private right: boolean = false;
    private show: boolean = true;

    public dontShow() {
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
}
const swappers = (currentNode: IPalavyrNode): Positions => {
    const pos = new Positions();
    if (currentNode.isRoot) {
        pos.dontShow();
        return pos;
    }
    if (currentNode.parentNodeReferences.Length !== 1) {
        pos.dontShow();
        return pos;
    }

    const parent = currentNode.parentNodeReferences.Single();

    if (!parent.isMultiOptionType) {
        pos.dontShow();
        return pos;
    }
    if (parent.childNodeReferences.Length < 2) {
        pos.dontShow();
        return pos;
    }

    const currentNodeIndex = parent.childNodeReferences.findIndexOf(currentNode);
    if (currentNodeIndex === 0) {
        pos.Right = true;
    } else if (currentNodeIndex === parent.childNodeReferences.Length - 1) {
        pos.Left = true;
    } else {
        pos.Right = true;
        pos.Left = true;
    }
    return pos;
};

const rightArrowOnClick = (event: any, currentNode: IPalavyrNode) => {
    currentNode.parentNodeReferences.Single().childNodeReferences.ShiftRight(currentNode);
    currentNode.UpdateTree();
};

const leftArrowOnClick = (event: any, currentNode: IPalavyrNode) => {
    currentNode.parentNodeReferences.Single().childNodeReferences.ShiftLeft(currentNode);
    currentNode.UpdateTree();
};

export const PositionSwitcher = ({ currentNode }: IPositionSwitcher) => {
    const cls = useStyles();
    const positions = swappers(currentNode);

    return positions.Show ? (
        <div className={cls.arrows}>
            <span>{positions.Left && <ArrowLeftIcon onClick={(event: any) => leftArrowOnClick(event, currentNode)} className={cls.arrow} fontSize="large" />}</span>
            {/* <span>TODO: Text to indicate which position in the option list this will be</span> */}
            {positions.Right && (
                <span>
                    <ArrowRightIcon onClick={(event: any) => rightArrowOnClick(event, currentNode)} className={cls.arrow} fontSize="large" />
                </span>
            )}
        </div>
    ) : (
        <></>
    );
};

const useStyles = makeStyles((theme) => ({
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
