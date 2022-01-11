import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
    logowrap: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
    },
    logotypography: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
        border: `3px solid ${theme.palette.success.light}`,
        padding: "0.4rem",
        borderRadius: "12px",
    },
    brandText: {
        fontSize: 64,
        fontWeight: "bolder",
        color: theme.palette.success.main,
        cursor: "pointer",
    },
}));

export const BrandName = () => {
    const history = useHistory();
    const cls = useStyles();
    return (
        <div className={cls.logowrap}>
            <div className={cls.logotypography} onClick={() => history.push("https://www.palavyr.com/")}>
                <Typography variant="body2" className={cls.brandText} display="inline">
                    Palavyr
                </Typography>
            </div>
        </div>
    );
};
