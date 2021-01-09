import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

export interface IHelpSubtitle {
    subtitle: string;
}

const useStyles = makeStyles((theme) => ({
    body: {
        paddingTop: "1rem",
    },
}));

export const HelpSubtitle = ({ subtitle }: IHelpSubtitle) => {
    const cls = useStyles();
    return (
        <div className={cls.body}>
            <Typography>{subtitle}</Typography>
        </div>
    );
};
