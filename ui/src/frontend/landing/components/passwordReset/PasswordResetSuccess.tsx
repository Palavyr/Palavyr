import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

const useStyles = makeStyles<{}>((theme: any) => ({
    root: {
        textAlign: "center",
        padding: "2rem",
    },
    title: {},
    linkdiv: { marginTop: "2rem" },
    link: {},
}));
export const RenderResetSuccess = () => {
    const cls = useStyles();

    return (
        <div className={cls.root}>
            <Typography variant="h4" className={cls.title}>
                You have successfully reset your password.
            </Typography>
            <Typography>You can now return to the homepage to login with your new password.</Typography>
            <div className={cls.linkdiv}>
                <Link className={cls.link} to="/">
                    Return to HomePage
                </Link>
            </div>
        </div>
    );
};
