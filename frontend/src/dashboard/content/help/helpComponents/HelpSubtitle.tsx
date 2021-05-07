import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

export interface IHelpSubtitle {
    subtitle: string;
}

const useStyles = makeStyles((theme) => ({
    body: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
    },
}));

export const HelpSubtitle = ({ subtitle }: IHelpSubtitle) => {
    const cls = useStyles();
    return (
        <div className={cls.body}>
            <Typography variant="h5" align="center">
                {subtitle}
            </Typography>
        </div>
    );
};
