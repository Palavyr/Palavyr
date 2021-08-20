import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

export interface ConversationDesignerCalloutProps {
    text: string;
    imgSrc: string;
    className?: string;
}

const useStyles = makeStyles(theme => ({
    paper: {
        padding: "1rem",
        borderRadius: "25px",
        boxShadow: theme.shadows[10],
    },
    img: {
        width: "100%",
        paddingTop: "0.5rem",
        paddingRight: "0.5rem",
        paddingLeft: "0.5rem",
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

export const LandingSpotlight = ({ text, imgSrc, className }: ConversationDesignerCalloutProps) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <PalavyrText className={cls.text} align="center" variant="h1">
                {text}
            </PalavyrText>
            <ZoomImage alt="alt" imgSrc={imgSrc} className={classNames(cls.img, className || "")} />
        </div>
    );
};

export interface ComponentLandingSpotlightProps {
    text: string | React.ReactNode;
    children: React.ReactNode;
}

export const ComponentLandingSpotlight = ({ text, children }: ComponentLandingSpotlightProps) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <PalavyrText className={cls.text} align="center" variant="h1">
                {text}
            </PalavyrText>
            {children}
        </div>
    );
};
