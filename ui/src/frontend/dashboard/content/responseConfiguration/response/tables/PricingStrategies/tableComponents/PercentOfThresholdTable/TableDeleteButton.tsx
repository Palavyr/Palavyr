import React from "react";
import { Button, makeStyles } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";

export const useStyles = makeStyles(theme => ({
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
