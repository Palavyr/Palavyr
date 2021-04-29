import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import InboxIcon from "@material-ui/icons/Inbox";
import CompareIcon from "@material-ui/icons/Compare";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ReviewSectionProps {
    isActive: boolean;
}

export const ReviewSection = ({ isActive }: ReviewSectionProps) => {
    const [reviewOpen, setReviewOpen] = useState<boolean>(true);
    const { setViewName } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const enquiriesOnClick = () => {
        setViewName("Enquiries");
        history.push("/dashboard/enquiries/");
    };

    const chatDemoOnClick = () => {
        setViewName("Widget Demo");
        history.push("/dashboard/demo/");
    };

    return (
        <List>
            <SidebarSectionHeader title="Review" onClick={() => setReviewOpen(!reviewOpen)} currentState={reviewOpen} />
            <Collapse in={reviewOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem text="Check Enquiries" isActive={isActive} onClick={enquiriesOnClick} IconComponent={<InboxIcon className={cls.icon} />} />
                <SidebarLinkItem text="Chat Demo" isActive={isActive} onClick={chatDemoOnClick} IconComponent={<CompareIcon className={cls.icon} />} />
            </Collapse>
        </List>
    );
};