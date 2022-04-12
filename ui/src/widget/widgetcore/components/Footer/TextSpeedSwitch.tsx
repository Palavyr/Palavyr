import { styled, Switch } from "@material-ui/core";
import * as React from "react";

export const TextSpeedSwitch = styled(Switch)(({ theme }) => ({
    padding: 8,
    color: theme.palette.common.white,
    "& .MuiSwitch-track": {
        borderRadius: 22 / 2,
        color: theme.palette.common.white,
        backgroundColor: "#264B94",
        // marginTop: "-10px",
        opacity: 1,
        "&:before, &:after": {
            content: '""',
            position: "absolute",
            transform: "translateY(-50%)",
            width: 16,
            color: theme.palette.common.white,
            height: 16,
        },
        "&:before": {
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
    // "& .Mui-checked": {
    //     color: theme.palette.common.white,
    //     backgroundColor: "#264B94",
    //     opacity: 1,
    // },
    "& .MuiSwitch-thumb": {
        boxShadow: "none",
        width: 20,
        height: 20,
        margin: 1,
    },
}));
