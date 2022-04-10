import * as React from "react";
import { makeStyles, Switch, SwitchProps } from "@material-ui/core";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

// w / h / s
// 42 / 26 / 16

// 42 / x == 26 / y == 16 / z

const CONTAINER_WIDTH = 21;
const CONTAINER_HEIGHT = 13;
const SLIDE_DISTANCE = 8;

const SWITCH_HEIGHT = CONTAINER_HEIGHT - 2;
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
            transform: `translateX(${SLIDE_DISTANCE}px)`,
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
        width: SWITCH_HEIGHT,
        height: SWITCH_WIDTH,
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
    const { context } = React.useContext(WidgetContext);
    return (
        <Switch
            focusVisibleClassName={cls.focusVisible}
            disableRipple
            onChange={(event: any, checked: boolean) => {
                if (checked) {
                    context.setReadingSpeed(3);
                } else {
                    context.setReadingSpeed(1);
                }
            }}
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
