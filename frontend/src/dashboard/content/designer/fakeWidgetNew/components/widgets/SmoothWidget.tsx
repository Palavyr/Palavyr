import * as React from "react";
import "./style.scss";
import { ConvoHeader } from "../components/ConvoHeader/ConvoHeader";
import { makeStyles } from "@material-ui/core";
import { Messages } from "../components/Messages/Messages";
import { BrandingStrip } from "../components/Footer/BrandingStrip";

const useStyles = makeStyles(theme => ({
    container: {
        height: "100%",
        display: "flex",
        flexDirection: "column",
        boxShadow: theme.shadows[10],
    },
}));

export const SmoothWidget = () => {
    const cls = useStyles();
    return (
        <div className={cls.container} aria-live="polite">
            <ConvoHeader />
            <Messages showTimeStamp={true} />
            <BrandingStrip />
        </div>
    );
};
