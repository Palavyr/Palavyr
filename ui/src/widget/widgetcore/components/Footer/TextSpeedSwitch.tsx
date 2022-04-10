import { styled, Switch } from "@material-ui/core";
import * as React from "react";


export const TextSpeedSwitch = styled(Switch)(({ theme }) => ({
    padding: 8,
    "& .MuiSwitch-track": {
        borderRadius: 22 / 2,
        "&:before, &:after": {
            content: '""',
            position: "absolute",
            top: "50%",
            transform: "translateY(-50%)",
            width: 16,
            height: 16,
        },
        "&:before": {
            content: "'F'",
            left: 14,
        },
        "&:after": {
            content: "'N'",
            right: 8,
        },
    },
    "& .MuiSwitch-thumb": {
        boxShadow: "none",
        width: 20,
        height: 20,
        margin: 1,
    },
}));
