import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { SpaceEvenly } from "./SpaceEvenly";
import ReplayIcon from "@material-ui/icons/Replay";

const useStyles = makeStyles(theme => ({
    // leadingText: {},
    // wrapper: {
    //     fontFamily: "Poppins",
    //     display: "static",
    //     bottom: "0px",
    //     alignText: "center",
    // },
    // brand: {
    //     "&:hover": {
    //         cursor: "pointer",
    //     },
    // },
    // spacer: {
    //     height: "7%",
    //     width: "100%",
    //     backgroundColor: "#264B94",
    //     color: "white",
    // },

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
        justifyItems: "center",
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
    return (
        <div style={{ display: "flex", flexDirection: "row", justifyContent: "space-between" }} className={cls.spacer}>
            <div style={{ alignItems: "center", display: "flex" }}>
                <Typography className={cls.wrapper} variant="caption">
                    Crafted with{" "}
                    <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")}>
                        Palavyr
                    </strong>
                </Typography>
            </div>
            <div style={{ height: "100%", alignItems: "center", display: "flex" }}>
                <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIcon} />
            </div>
        </div>
    );
    // return (
    // <SpaceEvenly vertical classes={cls.spacer} center>
    //     <Typography className={cls.wrapper} variant="caption" align="center" display="inline">
    //         Crafted with{" "}
    //         <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")}>
    //             Palavyr
    //         </strong>
    //     </Typography>
    //     <div style={{ height: "100%", alignItems: "center", display: "flex" }}>
    //         <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIcon} />
    //     </div>
    // </SpaceEvenly>
    // );
};
