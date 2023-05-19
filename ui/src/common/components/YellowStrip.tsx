import { makeStyles } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles<{}>((theme: any) => ({
    strip: {
        backgroundColor: theme.palette.warning.main,
        height: "1rem",
    },
}));
export const YellowStrip = () => {
    const cls = useStyles();
    return <div className={cls.strip} />;
};
