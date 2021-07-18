import { Button, makeStyles } from "@material-ui/core";
import { AnyFunction } from "@Palavyr-Types";
import classNames from "classnames";
import React from "react";

export interface ISinglePurposeButton {
    variant?: "contained" | "outlined";
    color?: "primary" | "secondary";
    buttonText: string;
    onClick: AnyFunction;
    size?: "small" | "medium" | "large" | undefined;
    classes?: string;
    disabled?: boolean;
}

const useStyles = makeStyles((theme) => ({
    singlePurposeButton: {
        marginTop: "1rem",
        color: "black",
        background: "white",
        "&:hover": {
            color: "black",
        },
    },
}));

export const SinglePurposeButton = ({ classes, size = "large", variant = "contained", color = "secondary", buttonText, disabled, onClick }: ISinglePurposeButton) => {
    const cls = useStyles();
    return (
        <Button size={size} className={classNames(cls.singlePurposeButton, classes)} variant={variant} color={color} onClick={onClick} disabled={disabled}>
            {buttonText}
        </Button>
    );
};
