import React from "react";
import { Typography, makeStyles } from "@material-ui/core";


export interface IHighlightedInformation {
    children: string | React.ReactElement | Array<any>;
}


const useStyles = makeStyles((theme) => ({
    main: {
        backgroundColor: theme.palette.warning.light,
        border: "2px solid black",
        // border: `${theme.border.borderWidth}px solid ${theme.palette.warning.main}`,
        padding: theme.spacing(2),
        borderRadius: theme.shape.borderRadius
    }
}))

export const HighlightedInformation = ({ children }: IHighlightedInformation) => {
    const classes = useStyles();

    return (
        <div className={classes.main}>
            <Typography variant="body2">{children}</Typography>
        </div>
    );
}
