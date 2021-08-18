import React, { memo } from "react";
import { useHistory } from "react-router-dom";
import { Divider, makeStyles } from "@material-ui/core";
import { AuthContext } from "../DashboardContext";
import { ConfigureSection } from "./sections/ConfigureSection";
import { ReviewSection } from "./sections/ReviewSection";
import { BillingSection } from "./sections/BillingSection";
import { WidgetStateSwitch } from "./WidgetStateSwitch";
import { OtherSection } from "./sections/OtherSection";
import { AreaNameDetails } from "@Palavyr-Types";
import classNames from "classnames";

export interface ISideBarMenu {
    areaNameDetails: AreaNameDetails;
    menuOpen: boolean;
}

const useStyles = makeStyles(theme => ({
    sidebarlist: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
        backgroundColor: theme.palette.primary.main,
        paddingBottom: "8rem",
    },
}));

export const SideBarMenu = memo(({ areaNameDetails, menuOpen }: ISideBarMenu) => {
    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);

    const currentPage = history.location.pathname + history.location.search;
    const cls = useStyles();

    return (
        <div className={classNames(cls.sidebarlist)}>
            <WidgetStateSwitch isActive={isActive} menuOpen={menuOpen} />
            <ConfigureSection menuOpen={menuOpen} currentPage={currentPage} areaNameDetails={areaNameDetails} isActive={isActive} />
            <Divider />
            <ReviewSection menuOpen={menuOpen} isActive={isActive} />
            <Divider />
            <BillingSection menuOpen={menuOpen} isActive={isActive} />
            <Divider />
            <OtherSection menuOpen={menuOpen} isActive={isActive} />
        </div>
    );
});
