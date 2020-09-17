import * as React from 'react';
import { Button, PropTypes } from '@material-ui/core';


export interface IResponseButton {
    onClick: any;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
}


export const ResponseButton = ({onClick, variant = "outlined", text = "Submit", color = "primary"}: IResponseButton) => {

    return (
        <Button variant={variant} color={color} size="small" onClick={onClick}>
            {text}
        </Button>
    )
}