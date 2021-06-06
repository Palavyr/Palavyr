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

    return (
        <List>
            <SidebarSectionHeader title="Review" onClick={() => setReviewOpen(!reviewOpen)} currentState={reviewOpen} />
            <Collapse in={reviewOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem text="Check Enquiries" isActive={isActive} onClick={enquiriesOnClick} IconComponent={<InboxIcon className={cls.icon} />}>
                    <Badge showZero={false} badgeContent={unseenNotifications} color="secondary">
                        <NotificationsIcon fontSize="small" />
                    </Badge>
                </SidebarLinkItem>
                <SidebarLinkItem text="Chat Demo" isActive={isActive} onClick={chatDemoOnClick} IconComponent={<CompareIcon className={cls.icon} />} />
                <SidebarLinkItem disabled={planTypeMeta && !planTypeMeta.allowedImageUpload} text="Uploads" isActive={isActive} onClick={imagesReviewOnClick} IconComponent={<PhotoLibraryIcon className={cls.icon} />} />
            </Collapse>
        </List>
    );
});