import React, { memo } from "react";
import { useHistory } from "react-router-dom";
import { Divider, makeStyles } from "@material-ui/core";
import { AuthContext } from "../DashboardContext";
import { IntentsSection } from "./sections/IntentsSection";
import { ReviewSection } from "./sections/ReviewSection";
import { AccountSection } from "./sections/AccountSection";
import { WidgetStateSwitch } from "./WidgetStateSwitch";
import { OtherSection } from "./sections/OtherSection";
import { IntentNameDetails } from "@Palavyr-Types";
import classNames from "classnames";

export interface ISideBarMenu {
    intentNameDetails: IntentNameDetails;
    menuOpen: boolean;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    sidebarlist: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
        backgroundColor: theme.palette.primary.main,
        paddingBottom: "8rem",
    },
}));

export const SideBarMenu = memo(({ intentNameDetails, menuOpen }: ISideBarMenu) => {
    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);
    const currentPage = history.location.pathname + history.location.search;
    const cls = useStyles();

    return (
        <div className={classNames(cls.sidebarlist)}>
            <WidgetStateSwitch isActive={isActive} menuOpen={menuOpen} />
            <IntentsSection menuOpen={menuOpen} currentPage={currentPage} intentNameDetails={intentNameDetails} isActive={isActive} />
            <Divider />
            <ReviewSection menuOpen={menuOpen} isActive={isActive} />
            <Divider />
            <AccountSection menuOpen={menuOpen} isActive={isActive} />
            <Divider />
            <OtherSection menuOpen={menuOpen} isActive={isActive} />
        </div>
    );
});
