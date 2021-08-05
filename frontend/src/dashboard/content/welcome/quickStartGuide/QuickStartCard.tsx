import { Divider, makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import React from "react";
import { Card } from "react-bootstrap";

const useStyles = makeStyles((theme) => ({
    sectionDiv: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: theme.palette.background.default,
    },
    card: {
        width: "50%",
        margin: "4rem",
        padding: "3rem",
        color: "white",
        background: theme.palette.secondary.main,
    },
    highlight: {
        "&:hover": {
            background: theme.palette.primary.dark,
            color: theme.palette.getContrastText(theme.palette.primary.dark),
        },
    },
    clickable: {
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export interface QuickStartCardProps {
    title: string;
    content: string;
    checkAreaCount(): void;
}

export const QuickStartCard = ({ title, content, checkAreaCount }: QuickStartCardProps) => {
    const cls = useStyles();
    return (
        <div className={cls.sectionDiv}>
            <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => checkAreaCount()}>
                <Typography align="center" gutterBottom variant="h4">
                    {title}
                    <Divider variant="middle" />
                </Typography>
                <Typography variant="body1" align="center" gutterBottom>
                    {content}
                </Typography>
            </Card>
        </div>
    );
};
