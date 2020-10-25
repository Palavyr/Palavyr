import { makeStyles } from '@material-ui/core'
import classNames from 'classnames';
import React from 'react'

const useStyles = makeStyles(({
    line: {
        height: "1px",
        width: "35%",
        marginTop: "10px",
        borderTop: "1px solid gray"
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
}

export const DividerWithText = ({ text }: DividerWithTextProps) => {

    const classes = useStyles();
    return (
        <div className={classes.separator}>
            <div className={classNames(classes.line, classes.left)} />
            {text && (<div>{text}</div>)}
            <div className={classNames(classes.line, classes.right)} />
        </div>
    )
}
