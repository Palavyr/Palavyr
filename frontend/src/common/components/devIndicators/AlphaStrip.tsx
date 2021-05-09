import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    devStripContainer: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        backgroundColor: theme.palette.warning.main,
        textAlign: "center",
    },
}));

export const AlphaStrip = () => {
    const cls = useStyles();
    return (
        <>
            <div className={cls.devStripContainer}>
                <Typography variant="h5">Palavyr is currently in Alpha Testing.</Typography>
            </div>
        </>
    );
};
