import * as React from "react";
import { Button, makeStyles, PropTypes } from "@material-ui/core";

export interface IResponseButton {
    onClick: any;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
}

const useStyles = makeStyles(theme => ({
    button: {
        color: "black",
        borderColor: "black",
        "&:hover": {
            borderColor: "black",
            backgroundColor: "gray"
        },
    },
    buttonFocus: {
        color: "black",
        borderColor: "black",
    },
}));

export const ResponseButton = ({ onClick, disabled = false, variant = "outlined", text = "Submit", color = "primary" }: IResponseButton) => {
    const cls = useStyles();
    return (
        <Button disableElevation focusVisibleClassName={cls.buttonFocus} className={cls.button} disabled={disabled} variant={variant} color={color} size="small" onClick={onClick}>
            {text}
        </Button>
    );
};
