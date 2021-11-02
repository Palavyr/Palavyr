import React from "react";
import { Card, makeStyles, Typography } from "@material-ui/core";
import { Link, useHistory } from "react-router-dom";

export interface TodoCardProps {
    link: string;
    text: string;
}

const useStyles = makeStyles((theme) => ({
    card: {
        backgroundColor: theme.palette.warning.main,
        padding: "1.2rem",
        margin: "1.2rem",
        "&:hover": {
            cursor: "pointer",
            backgroundColor: theme.palette.warning.dark,
            color: theme.palette.getContrastText(theme.palette.warning.dark),
        },
    },
    link: {
        textDecoration: "none",
        "&:hover": {
            cursor: "pointer",
        },
    },
    text: {},
}));

export const TodoCard = ({ link, text }: TodoCardProps) => {
    const cls = useStyles();
    const history = useHistory();

    return (
        <Card className={cls.card} onClick={() => window.open(link)}>
            <Typography className={cls.text} variant="h5">
                {text}
            </Typography>
        </Card>
    );
};
