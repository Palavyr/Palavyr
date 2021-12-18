import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import ReplayIcon from "@material-ui/icons/Replay";
import "@widgetcore/widget/widget.module.scss";
import classNames from "classnames";
import { useWidgetStyles } from "@widgetcore/widget/Widget";

const useStyles = makeStyles(theme => ({
    leadingText: {},
    wrapper: {
        fontFamily: "Poppins",
        justifyItems: "center",
        paddingLeft: "1rem",
    },
    brand: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    spacer: {
        height: "30px",
        width: "100%",
        backgroundColor: "#264B94",
        color: "white",
        zIndex: 1000,
    },
    replayIcon: {
        color: theme.palette.common.white,
        fontSize: "1rem",
        paddingRight: "1rem",
        "&:hover": {
            cursor: "pointer",
        },
    },
    iconRoot: {
        width: "100%",
        textAlign: "right",

    },
}));
export const BrandingStrip = () => {
    const cls = useStyles();
    const wcls = useWidgetStyles();
    return (
        <div className={classNames(wcls.pwrow, wcls.pfooter, cls.spacer)}>
            <div style={{ alignItems: "center", display: "flex" }}>
                <Typography className={cls.wrapper} variant="caption">
                    Crafted with{" "}
                    <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")}>
                        Palavyr
                    </strong>
                </Typography>
            </div>
            <div style={{ paddingLeft: "3rem", height: "100%", alignItems: "center", display: "flex" }}>
                <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIcon} onClick={() => window.location.reload()} />
            </div>
        </div>
    );
};
