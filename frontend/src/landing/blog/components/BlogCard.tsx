import React from "react";
import { Link } from "react-router-dom";
import format from "date-fns/format";
import classNames from "classnames";
import { Typography, Card, Box, makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    img: {
        width: "100%",
        height: "auto",
        marginBottom: 8,
    },
    card: {
        boxShadow: theme.shadows[2],
    },
    noDecoration: {
        textDecoration: "none !important",
    },
    title: {
        transition: theme.transitions.create(["background-color"], {
            duration: theme.transitions.duration.complex,
            easing: theme.transitions.easing.easeInOut,
        }),
        cursor: "pointer",
        color: theme.palette.secondary.main,
        "&:hover": {
            color: theme.palette.secondary.dark,
        },
        "&:active": {
            color: theme.palette.primary.dark,
        },
    },
    link: {
        transition: theme.transitions.create(["background-color"], {
            duration: theme.transitions.duration.complex,
            easing: theme.transitions.easing.easeInOut,
        }),
        cursor: "pointer",
        color: theme.palette.primary.main,
        "&:hover": {
            color: theme.palette.primary.dark,
        },
    },
    showFocus: {
        "&:focus span": {
            color: theme.palette.secondary.dark,
        },
    },
}));

export interface BlogCardProps {
    url: string;
    src: string;
    date: number;
    title: string;
    snippet: string;
}

export const BlogCard = ({ url, src, date, title, snippet }: BlogCardProps) => {
    const cls = useStyles();
    return (
        <Card className={cls.card}>
            {src && (
                <Link to={url} tabIndex={-1}>
                    <img src={src} className={cls.img} alt="" />
                </Link>
            )}
            <Box p={2}>
                <Typography variant="body2" color="textSecondary">
                    {format(new Date(date * 1000), "PPP", {
                        // awareOfUnicodeTokens: true,
                    })}
                </Typography>
                <Link to={url} className={classNames(cls.noDecoration, cls.showFocus)}>
                    <Typography variant="h6">
                        <span className={cls.title}>{title}</span>
                    </Typography>
                </Link>
                <Typography variant="body1" color="textSecondary">
                    {snippet}
                    <Link to={url} className={cls.noDecoration} tabIndex={-1}>
                        <span className={cls.link}> read more...</span>
                    </Link>
                </Typography>
            </Box>
        </Card>
    );
};
