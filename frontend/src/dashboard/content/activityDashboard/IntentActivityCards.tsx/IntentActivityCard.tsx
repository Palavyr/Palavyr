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
import { EnquiryActivtyResource } from "@Palavyr-Types";

const useStyles = makeStyles(theme => ({
    root: {
        maxWidth: 345,
        margin: "1rem",
    },
    paper: {},
    infoContainer: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
        alignItems: "flex-end",
    },
    info: {},
    sepBar: {
        height: "2rem",
        border: `1px solid ${theme.palette.grey[400]}`,
        borderRadius: "50%",
        textAlign: "center",
        margin: "0.2rem",
    },
    table: {},
    header: {},
    row: {},
}));

export interface ActivityCardProps {
    activityResource: EnquiryActivtyResource;
    onClick?(): void;
}

type MiniTableProps = {
    header: React.ReactNode;
    content: React.ReactNode;
};
const MiniTable = (props: MiniTableProps) => {
    const cls = useStyles();
    return (
        <table className={cls.table}>
            <th className={cls.header}>
                <PalavyrText align="center">{props.header}</PalavyrText>
            </th>
            <tr className={cls.row}>
                <PalavyrText align="center">{props.content}</PalavyrText>
            </tr>
        </table>
    );
};

export const IntentActivityCard = ({ activityResource, onClick }: ActivityCardProps) => {
    const cls = useStyles();
    return (
        <PalavyrCard className={cls.root}>
            <CardActionArea onClick={onClick}>
                <CardMedia component="img" alt="AnimatedLines" height="140" image={AnimatedLines} title="Animated Lines" />
                <CardContent>
                    <PalavyrText align="center" gutterBottom variant="h5" component="h2">
                        {activityResource.intentName}
                    </PalavyrText>
                </CardContent>
            </CardActionArea>
            <CardActions className={cls.infoContainer}>
                <MiniTable header="Started" content={activityResource.numRecords} />
                <div className={cls.sepBar} />
                <MiniTable header="Emails Sent" content={activityResource.sentEmailCount} />
                <div className={cls.sepBar} />
                <MiniTable header="Finished" content={activityResource.completed} />
                <div className={cls.sepBar} />
                <MiniTable header="Avg. Completion" content={activityResource.averageIntentCompletion === -1 ? "N/A" : formatAsPercent(activityResource.averageIntentCompletion)} />
            </CardActions>
        </PalavyrCard>
    );
};

const formatAsPercent = (x: number) => {
    return `${x.toFixed(2)}%`;
};
