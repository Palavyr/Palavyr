import * as React from "react";
import { makeStyles, Switch, SwitchProps } from "@material-ui/core";

const CONTAINER_HEIGHT = 30;
const CONTAINER_WIDTH = 40;

const SWITCH_HEIGHT = CONTAINER_HEIGHT;
const SWITCH_WIDTH = SWITCH_HEIGHT;

const useStyles = makeStyles(theme => ({
    root: {
        width: CONTAINER_WIDTH,
        height: CONTAINER_HEIGHT,
        padding: 0,
        margin: theme.spacing(1),
    },
    switchBase: {
        padding: 1,
        "&$checked": {
            transform: `translateX(${Math.floor(CONTAINER_WIDTH / 2)}px)`,
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
        width: SWITCH_WIDTH,
        height: SWITCH_HEIGHT,
    },
    track: {
        borderRadius: CONTAINER_HEIGHT / 2,
        border: `1px solid ${theme.palette.grey[400]}`,
        backgroundColor: theme.palette.grey[50],
        opacity: 1,
        transition: theme.transitions.create(["background-color", "border"]),
    },
    checked: {},
    focusVisible: {},
}));

export const TextSpeedToggle = ({ ...props }: SwitchProps) => {
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
