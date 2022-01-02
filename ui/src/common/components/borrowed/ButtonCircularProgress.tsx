import React from "react";
import { CircularProgress, Box, makeStyles } from "@material-ui/core";


export interface IButtonCircularProgress {
    size?: number;
}

const useStyles = makeStyles(theme => ({
    circularProgress: {
        color: theme.palette.secondary.main
    }
}));

export const ButtonCircularProgress = ({ size }: IButtonCircularProgress) => {

    const classes = useStyles();

    return (
        <Box color="secondary.main" pl={1.5} display="flex">
            <CircularProgress
                size={size ? size : 24}
                thickness={size ? (size / 5) * 24 : 5}
                className={classes.circularProgress}
            />
        </Box>
    );
}
