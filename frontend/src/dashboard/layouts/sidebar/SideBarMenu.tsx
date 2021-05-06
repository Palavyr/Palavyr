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

export interface ISideBarMenu {
    areaNameDetails: AreaNameDetails;
}

const useStyles = makeStyles((theme) => ({
    sidebarlist: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
        backgroundColor: theme.palette.primary.dark,
        paddingBottom: "8rem",
        // height: "inherit"
    },
}));

export const SideBarMenu = memo(({ areaNameDetails }: ISideBarMenu) => {
    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);

    const currentPage = history.location.pathname + history.location.search;
    const cls = useStyles();

    return (
        <div className={cls.sidebarlist}>
            <WidgetStateSwitch isActive={isActive} />
            <ConfigureSection currentPage={currentPage} areaNameDetails={areaNameDetails} isActive={isActive} />
            <Divider />
            <ReviewSection isActive={isActive} />
            <Divider />
            <BillingSection isActive={isActive} />
            <Divider />
            <OtherSection isActive={isActive} />
        </div>
    );
});
