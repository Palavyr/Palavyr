import React from "react";
import { IconButton, makeStyles, Badge, Tooltip } from "@material-ui/core";
import { useHistory } from "react-router-dom";
import NotificationsIcon from "@material-ui/icons/Notifications";

const useStyles = makeStyles<{}>((theme: any) => ({
    icon: {
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
        fontSize: "22pt",
        color: theme.palette.info.light,
    },
    toolTipInternal: {
        backgroundColor: theme.palette.primary.light,
        maxWidgth: "none",
        zIndex: 9999,
    },
}));

export interface NotificationBadgeProps {
    unseenNotifications: number;
}

export const NotificationBadges = ({ unseenNotifications }: NotificationBadgeProps) => {
    const cls = useStyles();
    const history = useHistory();
    return (
        <Tooltip title="Unseen enquiries" classes={{ tooltip: cls.toolTipInternal }}>
            <IconButton disabled={unseenNotifications === 0} onClick={() => history.push("/dashboard/enquiries")} className={cls.icon} edge={unseenNotifications > 0 ? "start" : false} color="inherit">
                <Badge showZero={false} badgeContent={unseenNotifications} color="secondary">
                    <NotificationsIcon />
                </Badge>
            </IconButton>
        </Tooltip>
    );
};
