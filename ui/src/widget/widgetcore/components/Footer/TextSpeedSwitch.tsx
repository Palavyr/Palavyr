import { styled, Switch } from "@material-ui/core";
import * as React from "react";

export const TextSpeedSwitch = styled(Switch)(({ theme }) => ({
    padding: 8,
    "& .MuiSwitch-track": {
        borderRadius: 22 / 2,
        color: theme.palette.common.white,
        "&:before, &:after": {
            content: '""',
            position: "absolute",
            transform: "translateY(-50%)",
            width: 16,
            height: 16,
        },
        "&:before": {
            color: theme.palette.common.white,
            content: "'Fast'",
            fontSize: "8px",
            left: 3,
            top: "65%",
            position: "relative",
            float: "left",
        },
        "&:after": {
            content: "'N'",
            top: "45%",
            position: "relative",
            float: "right",
        },
    },
    "& .MuiSwitch-thumb": {
        boxShadow: "none",
        width: 20,
        height: 20,
        margin: 1,
    },
}));
