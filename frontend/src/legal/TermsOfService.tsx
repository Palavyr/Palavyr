import React from "react";

import ArrowBackIcon from "@material-ui/icons/ArrowBack";
import { makeStyles, useTheme, Dialog, DialogTitle, DialogContent, Typography, DialogActions } from "@material-ui/core";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";


export interface ITermsOfServiceDialog {
    onClose: any;
}


const useStyles = makeStyles(theme => ({
    termsConditionsListitem: {
        marginLeft: theme.spacing(3),
        marginTop: theme.spacing(1)
    },
    dialogActions: {
        justifyContent: "flex-start",
        paddingTop: theme.spacing(2),
        paddingBottom: theme.spacing(2),
        paddingRight: theme.spacing(2)
    },
    backIcon: {
        marginRight: theme.spacing(1),
        color: "white",
        backgroundColor: "#3e5f82",
    },
    backButton: {
        color: "white",
        backgroundColor: "#3e5f82",
    }
}));

export const TermsOfServiceDialog = ({ onClose }: ITermsOfServiceDialog) => {

    const classes = useStyles();
    const theme = useTheme();

    return (
        <Dialog open scroll="paper" onClose={onClose} hideBackdrop>
            <DialogTitle>Terms and Conditions</DialogTitle>
            <DialogContent>
                <Typography variant="h6" color="primary" paragraph>
                    Introduction
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem
                    ipsum dolor sit amet.
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Intellectual Property Rights
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Restrictions
        </Typography>
                <Typography paragraph>
                    You are specifically restricted from all of the following:
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem} paragraph>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Your Content
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    No warranties
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Limitation of liability
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Indemnification
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Severability
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Variation of Terms
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Assignment
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Entire Agreement
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Governing Law & Jurisdiction
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
            </DialogContent>
            <DialogActions className={classes.dialogActions}>
                <ColoredButton
                    classes={classes.backButton}
                    onClick={onClose}
                    variant="contained"
                    color="secondary"
                >
                    <ArrowBackIcon className={classes.backIcon} />
                    Back
                </ColoredButton>
            </DialogActions>
        </Dialog>
    );
}