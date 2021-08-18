import { makeStyles } from "@material-ui/core";
import { WidgetPreferences, SetState } from "@Palavyr-Types";
import React from "react";
import { DesignChatHeader } from "./ChatHeader";
import { DesignLandingHeader } from "./LandingHeader";

export interface DesignHeadersProps {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}

const useStyles = makeStyles(theme => ({
    wrapper: {
        marginTop: "2rem",
        marginBottom: "2rem",
        display: "flex",
        justifyContent: "space-around",
    },
}));

export const DesignHeaders = ({ ...preferences }: DesignHeadersProps) => {
    const cls = useStyles();
    return (
        <div className={cls.wrapper}>
            <DesignLandingHeader {...preferences} />
            <DesignChatHeader {...preferences} />
        </div>
    );
};
