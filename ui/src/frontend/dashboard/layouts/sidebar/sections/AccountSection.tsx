import React, { memo, useContext, useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import SubscriptionsIcon from "@material-ui/icons/Subscriptions";
import { DashboardContext } from "../../DashboardContext";
import PaymentIcon from "@material-ui/icons/Payment";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { webUrl } from "@common/client/clientUtils";
import SettingsIcon from "@material-ui/icons/Settings";
import { GeneralSettingsLoc } from "@common/types/api/Enums";


const useStyles = makeStyles<{}>((theme: any) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface AccountSectionProps {
    isActive: boolean;
    menuOpen: boolean;
}

export const AccountSection = ({ isActive, menuOpen }: AccountSectionProps) => {
    const [billingOpen, setBillingOpen] = useState<boolean>(true);
    const { repository, setViewName, planTypeMeta } = useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const subscribeOnClick = () => {
        history.push("/dashboard/subscribe");
    };

    const createCustomerPortalSession = async () => {
        const returnUrl = `${webUrl}/dashboard`;
        const customerId = await repository.Purchase.Customer.GetCustomerId();
        const portalUrl = await repository.Purchase.Customer.GetCustomerPortal(customerId, returnUrl);
        window.open(portalUrl, "_blank");
    };

    const generalSettingsOnClick = () => {
        setViewName("General Settings");
        history.push(`/dashboard/settings/email?tab=${GeneralSettingsLoc.email}`);
    };

    return (
        <List>
            <SidebarSectionHeader menuOpen={menuOpen} className={"billing-sidebar-tour"} title="Account" onClick={() => setBillingOpen(!billingOpen)} currentState={billingOpen} />
            <Collapse in={billingOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem
                    toolTipText=" General Settings"
                    menuOpen={menuOpen}
                    className={"settings-sidebar-tour"}
                    text="Settings"
                    isActive={isActive}
                    onClick={generalSettingsOnClick}
                    IconComponent={<SettingsIcon className={cls.icon} />}
                />
                {planTypeMeta &&
                    (planTypeMeta.isFreePlan ? (
                        <SidebarLinkItem
                            toolTipText="Purchase A Subscription"
                            menuOpen={menuOpen}
                            text="Subscribe"
                            isActive={isActive}
                            onClick={subscribeOnClick}
                            IconComponent={<SubscriptionsIcon className={cls.icon} />}
                        />
                    ) : (
                        <SidebarLinkItem
                            toolTipText="Manage Your Subscription"
                            menuOpen={menuOpen}
                            text="Subscription"
                            isActive={isActive || !planTypeMeta.isFreePlan}
                            onClick={createCustomerPortalSession}
                            IconComponent={<PaymentIcon className={cls.icon} />}
                        />
                    ))}
            </Collapse>
        </List>
    );
};
