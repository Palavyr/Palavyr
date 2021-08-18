import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

export interface ConversationDesignerCalloutProps {
    text: string;
    imgSrc: string;
}

const useStyles = makeStyles(theme => ({
    image1: {
        height: "400px",
        borderRadius: "25px",
    },
    paper: {
        padding: "1rem",
        borderRadius: "25px",
        boxShadow: theme.shadows[10],
    },
    img: {
        width: "100%",
        padding: "0.5rem",
        borderRadius: "50px",
    },
    container: {
        backgroundColor: theme.palette.background.default,
    },
    text: {
        paddingTop: "15rem",
        paddingBottom: "15rem",
    },
}));

export const LandingSpotlight = ({ text, imgSrc }: ConversationDesignerCalloutProps) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <PalavyrText className={cls.text} align="center" variant="h1">
                {text}
            </PalavyrText>
            <ZoomImage alt="alt" imgSrc={imgSrc} className={cls.img} />
        </div>
    );
};
