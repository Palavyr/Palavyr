import React from "react";
import { useHistory } from "react-router-dom";
import { Divider, makeStyles } from "@material-ui/core";
import { AuthContext } from "../DashboardContext";
import { ConfigureSection } from "./sections/ConfigureSection";
import { ReviewSection } from "./sections/ReviewSection";
import { BillingSection } from "./sections/BillingSection";
import { WidgetStateSwitch } from "./WidgetStateSwitch";
import { OtherSection } from "./sections/OtherSection";

export interface ISideBarMenu {
    areaIdentifiers: Array<string>;
    areaNames: Array<string>;
}

const useStyles = makeStyles((theme) => ({
    sidebarlist: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
        backgroundColor: theme.palette.primary.dark,
        paddingBottom: "8rem",
    },
}));

export const SideBarMenu = ({ areaIdentifiers, areaNames }: ISideBarMenu) => {
    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);

    const currentPage = history.location.pathname + history.location.search;
    const cls = useStyles();

    return (
        <div className={cls.sidebarlist}>
            <WidgetStateSwitch isActive={isActive} />
            <ConfigureSection currentPage={currentPage} areaNames={areaNames} isActive={isActive} areaIdentifiers={areaIdentifiers} />
            <Divider />
            <ReviewSection isActive={isActive} />
            <Divider />
            <BillingSection isActive={isActive} />
            <Divider />
            <OtherSection isActive={isActive} />
        </div>
    );
};
