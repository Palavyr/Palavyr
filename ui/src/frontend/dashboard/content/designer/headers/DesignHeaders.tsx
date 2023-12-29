import { makeStyles } from "@material-ui/core";
import { WidgetPreferencesResource, SetState } from "@Palavyr-Types";
import React from "react";
import { DesignChatHeader } from "./ChatHeader";
import { DesignLandingHeader } from "./LandingHeader";

export interface DesignHeadersProps {
    widgetPreferences: WidgetPreferencesResource;
    setWidgetPreferences: SetState<WidgetPreferencesResource>;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    wrapper: {
        marginTop: "2rem",
        marginBottom: "2rem",
        display: "flex",
        justifyContent: "space-around",
    },
}));

export const DesignHeaders = ({ widgetPreferences }: DesignHeadersProps) => {
    const cls = useStyles();
    return (
        <div className={cls.wrapper}>
            {/* <DesignLandingHeader {...preferences} /> */}
            {/* <DesignChatHeader widgetPreferences={widgetPreferences} /> */}
        </div>
    );
};
