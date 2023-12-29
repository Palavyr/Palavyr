import { makeStyles } from "@material-ui/core";
import React from "react";

interface SaveBarProps {
    deleteButton: JSX.Element;
    addInnerButton: JSX.Element;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    savebar: {
        border: "none",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        width: "100%",
        padding: ".5rem",
    },
}));

export const ButtonBar = ({ deleteButton, addInnerButton }: SaveBarProps) => {
    const cls = useStyles();
    return (
        <div className={cls.savebar}>
            <div style={{ float: "left" }}>{addInnerButton}</div>
            <div style={{ float: "right" }}>{deleteButton}</div>
        </div>
    );
};
