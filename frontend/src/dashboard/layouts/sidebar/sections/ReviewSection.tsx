import React, { memo, useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles, Badge } from "@material-ui/core";
import InboxIcon from "@material-ui/icons/Inbox";
import CompareIcon from "@material-ui/icons/Compare";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import PhotoLibraryIcon from "@material-ui/icons/PhotoLibrary";
import NotificationsIcon from "@material-ui/icons/Notifications";
import BarChartIcon from "@material-ui/icons/BarChart";
import TrendingUpIcon from "@material-ui/icons/TrendingUp";
import TrendingUp from "@material-ui/icons/TrendingUp";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ReviewSectionProps {
    isActive: boolean;
}

export const ReviewSection = memo(({ isActive }: ReviewSectionProps) => {
    const [reviewOpen, setReviewOpen] = useState<boolean>(true);
    const { setViewName, unseenNotifications, planTypeMeta } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const enquiriesOnClick = () => {
        setViewName("Enquiries");
        history.push("/dashboard/enquiries");
    };

    const chatDemoOnClick = () => {
        setViewName("Widget Demo");
        history.push("/dashboard/demo");
    };

    const imagesReviewOnClick = () => {
        setViewName("Images");
        history.push("/dashboard/images");
    };

    const dashboardOnClick = () => {
        setViewName("Data Dashboard");
        history.push("/dashboard/data");
    };

    return (
        <List className={"review-sidebar-tour"}>
            <SidebarSectionHeader title="Review" onClick={() => setReviewOpen(!reviewOpen)} currentState={reviewOpen} />
            <Collapse in={reviewOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem className={"activity-sidebar-tour"} text="Activity" isActive={isActive} onClick={dashboardOnClick} IconComponent={<TrendingUpIcon className={cls.icon} />} />
                <SidebarLinkItem className={"check-enquiries-sidebar-tour"} text="Check Enquiries" isActive={isActive} onClick={enquiriesOnClick} IconComponent={<InboxIcon className={cls.icon} />}>
                    <Badge className={"check-enquiries-badge-sidebar-tour"} showZero={false} badgeContent={unseenNotifications} color="secondary">
                        <NotificationsIcon fontSize="small" />
                    </Badge>
                </SidebarLinkItem>
                <SidebarLinkItem className={"chat-demo-link-tour"} text="Chat Demo" isActive={isActive} onClick={chatDemoOnClick} IconComponent={<CompareIcon className={cls.icon} />} />
                <SidebarLinkItem
                    className={"uploads-sidebar-tour"}
                    disabled={planTypeMeta && !planTypeMeta.allowedImageUpload}
                    text="Uploads"
                    isActive={isActive}
                    onClick={imagesReviewOnClick}
                    IconComponent={<PhotoLibraryIcon className={cls.icon} />}
                />
            </Collapse>
        </List>
    );
});
