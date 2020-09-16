import React from "react";
import { Dialog, DialogContent, Box, makeStyles } from "@material-ui/core";
import { DialogTitleWithCloseIcon } from "./DialogTitleWithCloseIcon";


export interface IFormDialog {
    open: boolean,
    onClose: any,
    headline: string,
    loading: boolean,
    onFormSubmit: any,
    content: React.ReactElement,
    actions: React.ReactElement,
    hideBackdrop: boolean
}

const useStyles = makeStyles(theme => ({
    dialogPaper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420
    },
    actions: {
        marginTop: theme.spacing(2)
    },
    dialogPaperScrollPaper: {
        maxHeight: "none"
    },
    dialogContent: {
        paddingTop: 0,
        paddingBottom: 0
    }
}));


export const FormDialog = ({ open, onClose, loading, headline, onFormSubmit, content, actions, hideBackdrop }: IFormDialog) => {

    const classes = useStyles();

    return (
        <Dialog
            open={open}
            onClose={onClose}
            disableBackdropClick={loading}
            disableEscapeKeyDown={loading}
            classes={{
                paper: classes.dialogPaper,
                paperScrollPaper: classes.dialogPaperScrollPaper
            }}
            hideBackdrop={hideBackdrop ? hideBackdrop : false}
        >
            <DialogTitleWithCloseIcon
                title={headline}
                onClose={onClose}
                disabled={loading}
            />
            <DialogContent className={classes.dialogContent}>
                <form onSubmit={onFormSubmit}>
                    <div>{content}</div>
                    <Box width="100%" className={classes.actions}>
                        {actions}
                    </Box>
                </form>
            </DialogContent>
        </Dialog>
    );
}
