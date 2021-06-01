import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import SubscriptionsIcon from "@material-ui/icons/Subscriptions";
import { DashboardContext } from "../../DashboardContext";
import PaymentIcon from "@material-ui/icons/Payment";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { webUrl } from "@api-client/clientUtils";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface BillingSectionProps {
    isActive: boolean;
}

export const BillingSection = ({ isActive }: BillingSectionProps) => {
    const [billingOpen, setBillingOpen] = useState<boolean>(false);
    const { setViewName, planTypeMeta } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const subscribeOnClick = () => {
        setViewName("Subscriptions");
        history.push("/dashboard/subscribe/");
    };

    const createCustomerPortalSession = async () => {
        const repository = new PalavyrRepository();
        var returnUrl = `${webUrl}/dashboard/`;
        const customerId = await repository.Purchase.Customer.GetCustomerId();
        const portalUrl = await repository.Purchase.Customer.GetCustomerPortal(customerId, returnUrl);
        window.open(portalUrl, "_blank");
    };

    return (
        <List>
            <SidebarSectionHeader title="Billing" onClick={() => setBillingOpen(!billingOpen)} currentState={billingOpen} />
            <Collapse in={billingOpen} timeout="auto" unmountOnExit>
                {(planTypeMeta && planTypeMeta.isFreePlan) && (
                    <SidebarLinkItem text="Subscribe" isActive={isActive} onClick={subscribeOnClick} IconComponent={<SubscriptionsIcon className={cls.icon} />} />
                )}
                {(planTypeMeta && !planTypeMeta.isFreePlan) && (
                    <SidebarLinkItem
                        text="Manage"
                        isActive={isActive || !planTypeMeta.isFreePlan}
                        onClick={createCustomerPortalSession}
                        IconComponent={<PaymentIcon className={cls.icon} />}
                    />
                )}
            </Collapse>
        </List>
    );
};
