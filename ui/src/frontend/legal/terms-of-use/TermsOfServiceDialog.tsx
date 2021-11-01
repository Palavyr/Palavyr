import React from "react";

import ArrowBackIcon from "@material-ui/icons/ArrowBack";
import { makeStyles, useTheme, Dialog, DialogTitle, DialogContent, DialogActions, Button } from "@material-ui/core";
import { TermsOfUseContent } from "./TermsOfUseContent";

export interface ITermsOfServiceDialog {
    onClose: any;
}

const useStyles = makeStyles((theme) => ({
    termsConditionsListitem: {
        marginLeft: theme.spacing(3),
        marginTop: theme.spacing(1),
    },
    dialogActions: {
        justifyContent: "flex-start",
        paddingTop: theme.spacing(2),
        paddingBottom: theme.spacing(2),
        paddingRight: theme.spacing(2),
    },

    backButton: {},
}));

export const TermsOfServiceDialog = ({ onClose }: ITermsOfServiceDialog) => {
    const classes = useStyles();
    const theme = useTheme();

    return (
        <Dialog open scroll="paper" onClose={onClose} hideBackdrop>
            <DialogTitle>Terms and Conditions</DialogTitle>
            <DialogContent>
                <TermsOfUseContent />
            </DialogContent>
            <DialogActions className={classes.dialogActions}>
                <Button onClick={onClose} variant="outlined">
                    <ArrowBackIcon onClick={onClose} />
                    Back
                </Button>
            </DialogActions>
        </Dialog>
    );
};
