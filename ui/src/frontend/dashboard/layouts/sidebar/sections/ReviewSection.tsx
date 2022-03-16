import React, { memo, useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles, Badge } from "@material-ui/core";
import InboxIcon from "@material-ui/icons/Inbox";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import PhotoLibraryIcon from "@material-ui/icons/PhotoLibrary";
import NotificationsIcon from "@material-ui/icons/Notifications";
import TrendingUpIcon from "@material-ui/icons/TrendingUp";
import BrushIcon from "@material-ui/icons/Brush";
import MotorcycleIcon from "@material-ui/icons/Motorcycle";

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ReviewSectionProps {
    isActive: boolean;
    menuOpen: boolean;
}

export const ReviewSection = memo(({ isActive, menuOpen }: ReviewSectionProps) => {
    const [reviewOpen, setReviewOpen] = useState<boolean>(true);
    const { unseenNotifications, planTypeMeta } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const enquiriesOnClick = () => {
        history.push("/dashboard/enquiries");
    };

    const chatDemoOnClick = () => {
        history.push("/dashboard/demo");
    };

    const fileAssetsReviewOnClick = () => {
        history.push("/dashboard/file-assets");
    };

    const dashboardOnClick = () => {
        history.push("/dashboard/activity");
    };

    const designerOnClick = () => {
        history.push("/dashboard/designer");
    };

    return (
        <List className={"review-sidebar-tour"}>
            <SidebarSectionHeader menuOpen={menuOpen} title="Review" onClick={() => setReviewOpen(!reviewOpen)} currentState={reviewOpen} />
            <Collapse in={reviewOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem toolTipText="Chat bot activity" menuOpen={menuOpen} text="Activity" isActive={isActive} onClick={dashboardOnClick} IconComponent={<TrendingUpIcon className={cls.icon} />} />
                <SidebarLinkItem toolTipText="Enquiries" menuOpen={menuOpen} text="Enquiries" isActive={isActive} onClick={enquiriesOnClick} IconComponent={<InboxIcon className={cls.icon} />}>
                    <Badge className={"check-enquiries-badge-sidebar-tour"} showZero={false} badgeContent={unseenNotifications} color="secondary">
                        <NotificationsIcon fontSize="small" />
                    </Badge>
                </SidebarLinkItem>

                <SidebarLinkItem toolTipText="Style Designer" menuOpen={menuOpen} text="Style Designer" isActive={isActive} onClick={designerOnClick} IconComponent={<BrushIcon className={cls.icon} />} />
                <SidebarLinkItem toolTipText="Chat Bot Demo" menuOpen={menuOpen} text="Chat Bot Demo" isActive={isActive} onClick={chatDemoOnClick} IconComponent={<MotorcycleIcon className={cls.icon} />} />
                <SidebarLinkItem
                    toolTipText="File Uploads"
                    menuOpen={menuOpen}
                    disabled={planTypeMeta && !planTypeMeta.allowedImageUpload}
                    text="Uploads"
                    isActive={isActive}
                    onClick={fileAssetsReviewOnClick}
                    IconComponent={<PhotoLibraryIcon className={cls.icon} />}
                />
            </Collapse>
        </List>
    );
});
