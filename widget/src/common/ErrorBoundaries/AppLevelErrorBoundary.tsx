import { makeStyles, Typography } from "@material-ui/core";
import { Align } from "common/Align";
import React from "react";

const useStyles = makeStyles({
    error: {
        marginBottom: "1rem",
    },
});

export const ErrorFallback = ({ error, resetErrorBoundary }) => {
    const cls = useStyles();
    return (
        <Align>
            <div>
                <Typography align="center" variant="h4">
                    Oh my gosh... Somethings gone wrong. We're terribly sorry!
                </Typography>
                <Typography align="center" variant="body1">
                    We realize this is a major inconvenience. This app is currently is beta testing and we are working hard to resolve these sorts of issues.
                </Typography>
                <Typography align="center" variant="body1">
                    If you can, please report this problem to us at info.palavyr@gmail.com
                </Typography>
                <Typography>{error}</Typography>
            </div>
        </Align>
    );
};
