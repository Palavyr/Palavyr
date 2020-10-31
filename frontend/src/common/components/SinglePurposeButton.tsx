import { Button, makeStyles } from '@material-ui/core'
import { AnyFunction } from '@Palavyr-Types'
import classNames from 'classnames'
import React from 'react'


export interface ISinglePurposeButton {
    classes?: string;
    variant: "contained" | "outlined";
    color: "primary" | "secondary";
    buttonText: string;
    onClick: AnyFunction;
}

const useStyles = makeStyles(theme => ({
    singlePurposeButton: {
        marginTop: "1rem",
        color: "black",
        background: "white",
        "&:hover": {
            color: "white"
        }
    }
}))


export const SinglePurposeButton = ({ classes, variant, color, buttonText, onClick }: ISinglePurposeButton) => {
    const cls = useStyles();
    return (
        <Button
            className={classNames(cls.singlePurposeButton, classes)}
            variant={variant}
            color={color}
            onClick={onClick}
        >
            {buttonText}
        </Button>
    )
}
