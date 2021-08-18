import React, { memo, useContext, useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import SubscriptionsIcon from "@material-ui/icons/Subscriptions";
import { DashboardContext } from "../../DashboardContext";
import PaymentIcon from "@material-ui/icons/Payment";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { webUrl } from "@api-client/clientUtils";

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface BillingSectionProps {
    isActive: boolean;
    menuOpen: boolean;
}

export const BillingSection = memo(({ isActive, menuOpen }: BillingSectionProps) => {
    const [billingOpen, setBillingOpen] = useState<boolean>(false);
    const { setViewName, planTypeMeta } = useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const subscribeOnClick = () => {
        history.push("/dashboard/subscribe");
    };

    const createCustomerPortalSession = async () => {
        const { repository } = useContext(DashboardContext);
        const returnUrl = `${webUrl}/dashboard`;
        const customerId = await repository.Purchase.Customer.GetCustomerId();
        console.log(customerId);
        const portalUrl = await repository.Purchase.Customer.GetCustomerPortal(customerId, returnUrl);
        console.log(portalUrl);
        window.open(portalUrl, "_blank");
    };

    return (
        <List>
            <SidebarSectionHeader menuOpen={menuOpen} className={"billing-sidebar-tour"} title="Billing" onClick={() => setBillingOpen(!billingOpen)} currentState={billingOpen} />
            <Collapse in={billingOpen} timeout="auto" unmountOnExit>
                {planTypeMeta && planTypeMeta.isFreePlan && <SidebarLinkItem text="Subscribe" isActive={isActive} onClick={subscribeOnClick} IconComponent={<SubscriptionsIcon className={cls.icon} />} />}
                {planTypeMeta && !planTypeMeta.isFreePlan && (
                    <SidebarLinkItem text="Manage" isActive={isActive || !planTypeMeta.isFreePlan} onClick={createCustomerPortalSession} IconComponent={<PaymentIcon className={cls.icon} />} />
                )}
            </Collapse>
        </List>
    );
});
