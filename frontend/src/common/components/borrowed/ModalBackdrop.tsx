import React from "react";
import { Backdrop, makeStyles } from "@material-ui/core";

const useStyles = makeStyles({
    backdrop: {
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        zIndex: 1200,
        position: "fixed",
        touchAction: "none",
        backgroundColor: "rgba(0, 0, 0, 0.5)"
    }
});

export interface IModalBackdrop {
    open: boolean;
}

export const ModalBackdrop = ({ open }: IModalBackdrop) => {
    const classes = useStyles();
    return <Backdrop open={open} className={classes.backdrop} />;
}
