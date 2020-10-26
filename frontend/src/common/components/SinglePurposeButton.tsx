import { Button } from '@material-ui/core'
import { AnyFunction } from '@Palavyr-Types'
import React from 'react'


export interface ISinglePurposeButton {
    classes: string;
    variant: "contained" | "outlined";
    color: "primary" | "secondary";
    buttonText: string;
    onClick: AnyFunction;
}


export const SinglePurposeButton = ({classes, variant, color, buttonText, onClick}: ISinglePurposeButton) => {

    return (
        <Button
            className={classes}
            variant={variant}
            color={color}
            onClick={onClick}
        >
            {buttonText}
        </Button>
    )
}
