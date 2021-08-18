import React from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles, Typography } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";

const useStyles = makeStyles(theme => ({
    customizetext: {
        paddingTop: "1.8rem",
    },

}));

export const ChatDemoHeader = () => {
    const cls = useStyles();
    return (
        <>
            <Typography gutterBottom align="center" variant="h4" className={cls.customizetext}>
                Try out your widget
            </Typography>
        </>
    );
};
