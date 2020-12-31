import * as React from 'react';
import { Button, PropTypes } from '@material-ui/core';


export interface IResponseButton {
    onClick: any;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
}


export const ResponseButton = ({onClick, disabled = false, variant = "outlined", text = "Submit", color = "primary"}: IResponseButton) => {

    return (
        <Button disabled={disabled} variant={variant} color={color} size="small" onClick={onClick}>
            {text}
        </Button>
    )
}