import { makeStyles, Typography } from '@material-ui/core'
import classNames from 'classnames';
import React from 'react'

const useStyles = makeStyles((theme) => ({
    line: {
        height: "1px",
        width: "35%",
        marginTop: "20px",
        borderTop: `1px solid ${theme.palette.common.black}`
    },
    left: {
        marginLeft: "5%",
        alignContent: "flex-start"
    },
    right: {
        marginRight: "5%",
        alignContent: "flex-end"
    },
    separator: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between"
    }
}))


export interface DividerWithTextProps {
    text?: string;
    variant?: "h1" | "h2" | "h3" | "h4" | "h5"
}

export const DividerWithText = ({ text, variant }: DividerWithTextProps) => {

    const classes = useStyles();
    return (
        <div className={classes.separator}>
            <div className={classNames(classes.line, classes.left)} />
            {text && (<Typography variant={variant}>{text}</Typography>)}
            <div className={classNames(classes.line, classes.right)} />
        </div>
    )
}
