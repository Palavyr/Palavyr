import { makeStyles } from "@material-ui/core";
import { PalavyrCard } from "material/surface/PalavyrCard";
import React, { useEffect, useState } from "react";

import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import AnimatedLines from "../images/animated-lines.gif";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyles = makeStyles({
    root: {
        maxWidth: 345,
        margin: "1rem",
        marginLeft: "2rem",
        marginRight: "2rem",
    },
    paper: {},
    infoContainer: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
    },
    info: {
    },
});

export interface ActivityCardProps {
    areaName: string;
    count: number;
    completed: number;
    emails: number;
    onClick?(): void;
}

export const IntentActivityCard = ({ areaName, count, completed, emails, onClick }: ActivityCardProps) => {
    const cls = useStyles();
    return (
        <PalavyrCard className={cls.root}>
            <CardActionArea onClick={onClick}>
                <CardMedia component="img" alt="AnimatedLines" height="140" image={AnimatedLines} title="Animated Lines" />
                <CardContent>
                    <PalavyrText align="center" gutterBottom variant="h5" component="h2">
                        {areaName}
                    </PalavyrText>
                </CardContent>
            </CardActionArea>
            <CardActions className={cls.infoContainer}>
                <div className={cls.info}>
                    <PalavyrText align="center" display="block">Total</PalavyrText>
                    <PalavyrText align="center" display="block">{count}</PalavyrText>
                </div>
                <div className={cls.info}>
                    <PalavyrText align="center" display="block">Emails</PalavyrText>
                    <PalavyrText align="center" display="block">{emails}</PalavyrText>
                </div>
                <div className={cls.info}>
                    <PalavyrText align="center" display="block">Completed</PalavyrText>
                    <PalavyrText align="center" display="block">{completed}</PalavyrText>
                </div>
            </CardActions>
        </PalavyrCard>
    );
};
