import React from "react";
import { Typography, Chip, makeStyles, Divider } from "@material-ui/core";
import { TreeErrors } from "@Palavyr-Types";

const useStyles = makeStyles((theme) => ({
    missingNodeText: {
        marginBottom: "1rem",
        fontWeight: "bolder",
    },
    missingNodeChipContainer: {
        textAlign: "center",
    },
    missingNodeChip: {
        marginRight: "0.7rem",
    },
    outOfOrderText: {
        marginBottom: "1rem",
        fontWeight: "bolder",
    },
    outOfOrderNodeChip: {
        marginRight: "0.7rem",
    },
}));

export interface TreeErrorProps {
    treeErrors: TreeErrors;
}

export const TreeErrorPanel = ({ treeErrors }: TreeErrorProps) => {
    const missingNodes = treeErrors.missingNodes;
    const outOfOrderNodes = treeErrors.outOfOrder;
    const anyErrors = treeErrors.anyErrors;

    const cls = useStyles();

    return anyErrors ? (
        <>
            {missingNodes.length > 0 && (
                <>
                    <Typography align="center" className={cls.missingNodeText} variant="h5">
                        These node types are currently missing from your tree:
                    </Typography>
                    <div className={cls.missingNodeChip}>
                        {missingNodes.map((x: string, index: number) => (
                            <Chip key={index} className={cls.missingNodeChip} label={x} color="secondary" />
                        ))}
                    </div>
                </>
            )}

            {missingNodes.length > 0 && outOfOrderNodes.length > 0 && <Divider />}

            {outOfOrderNodes.length > 0 && (
                <>
                    <Typography align="center" className={cls.outOfOrderText} variant="h5">
                        Complex dynamic table nodes sometimes need to specified in a specific order, determined by the dynamic table. The following nodes need to be specified in the provided order:
                    </Typography>
                    <div>
                        {outOfOrderNodes.map((x: string, index: number) => {
                            const chipLabel = x.split(",");
                            return <Chip key={9999 - index} className={cls.outOfOrderNodeChip} label={x} color="secondary" />;
                        })}
                    </div>
                </>
            )}
        </>
    ) : null;
};
