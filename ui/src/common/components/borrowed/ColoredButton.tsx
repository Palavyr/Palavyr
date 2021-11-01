import React, { memo } from "react";
import { Button } from "@material-ui/core";
import { AnyVoidFunction } from "@Palavyr-Types";

export interface IColoredButton {
    color?: "primary" | "secondary";
    children: React.ReactNode;
    onClick?: AnyVoidFunction;
    variant?: "text" | "outlined" | "contained" | undefined;
    type?: "button" | "reset" | "submit" | undefined;
    disabled?: boolean;
    classes?: string;
    disableElevation?: boolean;
    styles?: Object;
    href?: string;
    startIcon?: React.ReactNode;
}

export const ColoredButton = memo(({ color, children, onClick, variant = "contained", type, disabled, classes, disableElevation = false, styles = {}, href, startIcon }: IColoredButton) => {
    return (
        <Button startIcon={startIcon} className={classes} href={href} style={styles} variant={variant} onClick={onClick} color={color} type={type} disabled={disabled} disableElevation={disableElevation}>
            {children}
        </Button>
    );
});
