import { makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles((theme) => ({
    line: {
        flexGrow: 1,
        height: "1px",
        width: "35%",
        marginTop: "20px",
        borderTop: `1px solid ${theme.palette.common.black}`,
    },
    left: {
        flexGrow: 1,
        marginRight: "5%",
        alignContent: "flex-start",
    },
    right: {
        flexGrow: 1,
        marginLeft: "5%",
        alignContent: "flex-end",
    },
    separator: {
        flexGrow: 1,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
    },
}));

export interface DividerWithTextProps {
    text?: string;
    textComponent?: React.ReactElement;
    variant?: "h1" | "h2" | "h3" | "h4" | "h5";
}

export const DividerWithText = ({ text, textComponent, variant }: DividerWithTextProps) => {
    const cls = useStyles();
    return (
        <div className={cls.separator}>
            <div className={classNames(cls.line, cls.left)} />
            {text && (
                <Typography style={{ flexGrow: 1, transform: "translate(0px, 7px) scale(1)" }} display="inline" variant={variant}>
                    {text}
                </Typography>
            )}
            {textComponent}
            <div className={classNames(cls.line, cls.right)} />
        </div>
    );
};
