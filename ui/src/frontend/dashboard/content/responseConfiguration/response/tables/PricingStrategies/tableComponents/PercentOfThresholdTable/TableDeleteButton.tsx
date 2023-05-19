import React from "react";
import { Button, makeStyles } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";

const useStyles = makeStyles<{}>((theme: any) => ({
    deleteIcon: {
        borderRadius: "5px",
    },
}));
export const TableDeleteButton = ({ onClick }: { onClick: () => void }) => {
    const cls = useStyles();

    return (
        <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={onClick}>
            Remove
        </Button>
    );
};
