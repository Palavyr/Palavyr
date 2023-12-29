import { Card, Divider, makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import React from "react";


const useStyles = makeStyles<{}>((theme: any) => ({
    sectionDiv: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: theme.palette.background.default,
    },
    card: {
        width: "50%",
        marginBottom: "4rem",
        // padding: "3rem",
        color: "white",
        background: theme.palette.primary.dark,
    },
    highlight: {
        "&:hover": {
            background: theme.palette.secondary.main,
            color: theme.palette.getContrastText(theme.palette.primary.dark),
        },
    },
    clickable: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    rowNumber: {
        fontSize: "24pt",
        fontWeight: "bolder",
        fontFamily: "Poppins",
        textAlign: "center",
        display: "flex",
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center",
        position: "relative",
        top: "1rem",
        left: ".5rem",
        width: "7ch",
        height: "2rem",
        color: theme.palette.secondary.main,
    },
}));

export interface QuickStartCardProps {
    title: string;
    content: string;
    onClick?(): void;
    rowNumber?: number;
}

export const QuickStartCard = ({ title, content, onClick = () => null, rowNumber }: QuickStartCardProps) => {
    const cls = useStyles();
    return (
        <div className={cls.sectionDiv}>
            <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => onClick()}>
                {rowNumber && <div className={cls.rowNumber}>Step{" "}{rowNumber}</div>}
                <div style={{ padding: "3rem", paddingTop: "1rem", paddingLeft: "4rem" }}>
                    <Typography align="center" gutterBottom variant="h4">
                        {title}
                        <Divider variant="middle" />
                    </Typography>
                    <Typography variant="h6" align="center" gutterBottom>
                        {content}
                    </Typography>
                </div>
            </Card>
        </div>
    );
};
