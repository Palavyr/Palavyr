import * as React from "react";
import { IconButton, ListItem, ListItemIcon, makeStyles, Menu, MenuItem, Tooltip } from "@material-ui/core";
import { SessionStorage } from "@localStorage/sessionStorage";
import { DashboardContext } from "../DashboardContext";
import Fade from "@material-ui/core/Fade";
import { useHistory } from "react-router-dom";
import { useEffect, useState } from "react";
import { TOPBAR_MAX_HEIGHT } from "@constants";
import AccountBoxIcon from "@material-ui/icons/AccountBox";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import AccountBalanceIcon from "@material-ui/icons/AccountBalance";
import FreeBreakfastIcon from "@material-ui/icons/FreeBreakfast";
import CardMembershipIcon from "@material-ui/icons/CardMembership";
import WhatshotIcon from "@material-ui/icons/Whatshot";
import { webUrl } from "@common/client/clientUtils";
import { GeneralSettingsLoc, PurchaseTypes } from "@common/types/api/Enums";

const DETAILS_MAX_HEIGHT = TOPBAR_MAX_HEIGHT - 10;

const useStyles = makeStyles(theme => ({
    logwrapper: {
        maxHeight: `${DETAILS_MAX_HEIGHT}px`,
        border: `1px solid ${theme.palette.info.main}`,
        borderRadius: "8px",
        boxShadow: theme.shadows[0],
        backgroundColor: theme.palette.success.light,
        "&:hover": {
            backgroundColor: theme.palette.success.main,
            cursor: "pointer",
        },
    },
    toolTipInternal: {
        backgroundColor: theme.palette.primary.light,
        maxWidgth: "none",
        zIndex: 9999,
    },
    default: {
        padding: "0.5rem",
        display: "flex",
    },
    icon: {
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
        fontSize: "22pt",
        color: theme.palette.info.light,
    },
}));

export const UserDetailsMenu = React.memo(() => {
    const cls = useStyles();
    const history = useHistory();

    const email = SessionStorage.getEmailAddress();
    const [loading, setLoading] = useState<boolean>(true);

    const { planTypeMeta, setViewName, repository } = React.useContext(DashboardContext);

    const userOnClick = () => {
        setViewName("General Settings");
        history.push(`/dashboard/settings/email?tab=${GeneralSettingsLoc.email}`);
    };

    useEffect(() => {
        setTimeout(() => {
            setLoading(false);
        }, 1200);
    }, []);

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const createCustomerPortalSession = async () => {
        const returnUrl = `${webUrl}/dashboard`;
        const customerId = await repository.Purchase.Customer.GetCustomerId();
        const portalUrl = await repository.Purchase.Customer.GetCustomerPortal(customerId, returnUrl);
        window.open(portalUrl, "_blank");
    };

    const getPlanTypeIcon = () => {
        if (planTypeMeta) {
            switch (planTypeMeta.planType) {
                case PurchaseTypes.Free:
                    return <FreeBreakfastIcon className={cls.icon} />;
                case PurchaseTypes.Lyte:
                    return <CardMembershipIcon className={cls.icon} />;
                case PurchaseTypes.Premium:
                    return <WhatshotIcon className={cls.icon} />;
                case PurchaseTypes.Pro:
                    return <AccountBalanceIcon className={cls.icon} />;
                default:
                    return <></>;
            }
        }
        return;
    };

    return (
        <>
            <Tooltip TransitionComponent={Fade} title={<PalavyrText>User Details</PalavyrText>} placement="bottom" classes={{ tooltip: cls.toolTipInternal }} interactive>
                <IconButton className={cls.icon} aria-controls="simple-menu" aria-haspopup="true" onClick={handleClick}>
                    <AccountBoxIcon />
                </IconButton>
            </Tooltip>
            <Menu anchorEl={anchorEl} keepMounted open={Boolean(anchorEl)} onClose={handleClose}>
                <MenuItem onClick={userOnClick}>
                    <ListItemIcon>
                        <PalavyrText>whoami</PalavyrText>
                    </ListItemIcon>
                    <ListItem>
                        <span className={cls.default}>
                            <PalavyrText noWrap={true}>{email}</PalavyrText>
                        </span>
                    </ListItem>
                </MenuItem>
                <MenuItem onClick={createCustomerPortalSession}>
                    <ListItemIcon>{getPlanTypeIcon()}</ListItemIcon>
                    <ListItem>{planTypeMeta && <PalavyrText>Subscription: {planTypeMeta.planType}</PalavyrText>}</ListItem>
                </MenuItem>
            </Menu>
        </>
    );
});
