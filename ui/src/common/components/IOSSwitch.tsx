import * as React from "react";
import { makeStyles, Switch, SwitchProps } from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    root: {
        width: 42,
        height: 26,
        padding: 0,
        margin: theme.spacing(1),
    },
    switchBase: {
        padding: 1,
        "&$checked": {
            transform: "translateX(16px)",
            color: theme.palette.common.white,
            "& + $track": {
                backgroundColor: "#52d869",
                opacity: 1,
                border: "none",
            },
        },
        "&$focusVisible $thumb": {
            color: "#52d869",
            border: "6px solid #fff",
        },
    },

    thumb: {
        width: 24,
        height: 24,
    },
    track: {
        borderRadius: 26 / 2,
        border: `1px solid ${theme.palette.grey[400]}`,
        backgroundColor: theme.palette.grey[50],
        opacity: 1,
        transition: theme.transitions.create(["background-color", "border"]),
    },
    checked: {},
    focusVisible: {},
}));

export const IOSSwitch = ({ ...props }: SwitchProps) => {
    const cls = useStyles();
    return (
        <Switch
            focusVisibleClassName={cls.focusVisible}
            disableRipple
            classes={{
                root: cls.root,
                switchBase: cls.switchBase,
                thumb: cls.thumb,
                track: cls.track,
                checked: cls.checked,
            }}
            {...props}
        />
    );
};
