import React from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles, Typography } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";

const useStyles = makeStyles((theme) => ({
    customizetext: {
        paddingTop: "1.8rem",
    },
    button: {
        marginBottom: "1rem",
    },
}));

export const ChatDemoHeader = () => {
    const cls = useStyles();
    return (
        <>
            <Typography gutterBottom align="center" variant="h4" className={cls.customizetext}>
                Customize your widget
            </Typography>
            <Align>
                <SinglePurposeButton classes={cls.button} variant="outlined" color="primary" buttonText="Reload" onClick={() => window.location.reload()} />
            </Align>
        </>
    );
};
