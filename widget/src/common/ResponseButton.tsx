import * as React from "react";
import { Button, makeStyles, PropTypes } from "@material-ui/core";

type ColorProps = {
    color: string;
}
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
        marginBottom: "0.4rem",
        borderColor: "black",
        "&:hover": {
            borderColor: "black",
            backgroundColor: "gray",
        },
    },
    buttonFocus: {
        color: "black",
        borderColor: "black",
    },
}));

export const ResponseButton = ({ onClick, disabled = false, variant = "outlined", text = "Submit", }: IResponseButton) => {
    const cls = useStyles();
    return (
        <Button disableElevation focusVisibleClassName={cls.buttonFocus} className={cls.button} disabled={disabled} variant={variant} size="small" onClick={onClick}>
            {text}
        </Button>
    );
};
