import React, { memo } from "react";
import { Button } from "@material-ui/core";
import { AnyVoidFunction } from "@Palavyr-Types";

export interface IColoredButton {
    color: "primary" | "secondary";
    children: React.ReactNode;
    onClick?: AnyVoidFunction;
    variant: "text" | "outlined" | "contained" | undefined;
    type?: "button" | "reset" | "submit" | undefined;
    disabled?: boolean;
    classes?: string;
}

export const ColoredButton = memo(({ color, children, onClick, variant, type, disabled, classes }: IColoredButton) => {
    return (
        <Button className={classes} variant={variant} onClick={onClick} color={color} type={type} disabled={disabled}>
            {children}
        </Button>
    );
});
