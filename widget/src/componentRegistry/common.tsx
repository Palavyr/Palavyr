import * as React from 'react';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles(theme => ({
    messageText: {
        backgroundColor: "#f4f7f9",
        borderRadius: "10px",
        padding: "15px",
        maxWidth: "215px",
        textAlign: "left",
    }
}))

export interface IWrapMessages {
    children: React.ReactNode;
}

export const MessageWrapper = ({ children }: IWrapMessages) => {
    const classes = useStyles();
    return (
        <div className={classes.messageText}>
            {children}
        </div>
    )
}
