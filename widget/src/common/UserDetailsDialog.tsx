import React from "react";
import { Dialog, DialogContent, Box, makeStyles, IconButton } from "@material-ui/core";
import { DialogTitleWithCloseIcon } from "./DialogTitleWithCloseIcon";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";


export interface UserDetailsDialogProps {
    open: boolean;
    onClose: any;
    title: string;
    content: React.ReactElement;
    detailsSet: boolean;
}

const useStyles = makeStyles(theme => ({
    dialogPaper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
    },
    actions: {
        marginTop: theme.spacing(2),
    },
    dialogPaperScrollPaper: {
        maxHeight: "none",
    },
    dialogContent: {
        paddingTop: 0,
        paddingBottom: 0,
    },
}));

export const UserDetailsDialog = ({ open, onClose, title, content, detailsSet }: UserDetailsDialogProps) => {
    const classes = useStyles();
    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
    };
    return (
        <Dialog
            style={{ zIndex: 999999 }}
            open={open}
            onClose={onClose}
            classes={{
                paper: classes.dialogPaper,
                paperScrollPaper: classes.dialogPaperScrollPaper,
            }}
            disableBackdropClick
            hideBackdrop={false}
        >
            <DialogTitleWithCloseIcon title={title} onClose={onClose} detailsSet={detailsSet} />
            <DialogContent className={classes.dialogContent}>
                <form onSubmit={onFormSubmit}>
                    <div>{content}</div>
                    {detailsSet && (
                    <button type="submit" onClick={onClose} style={{ marginRight: -12, marginTop: -10 }} aria-label="Close">
                        <CheckCircleOutlineIcon />
                    </button>
                )}
                </form>
            </DialogContent>
        </Dialog>
    );
};
