import React from "react";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableRow, Typography } from "@material-ui/core";
import { EnquiryResource } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";
import { EnquiryTableRowCell } from "./EnquiriesTableRowCell";
import { formatLegitTimeStamp } from "./enquiriesUtils";
import { EnquiryTimeStamp } from "./EnquiryTimeStamp";

export interface EnquiriesTableRowProps {
    enquiry: EnquiryResource;
    toggleSelected: (conversationId: string) => void;
    markAsSeen: (conversationId: string) => Promise<void>;
    selected: boolean;
}

const formConversationReviewPath = (conversationId: string) => {
    return CONVERSATION_REVIEW + `?${CONVERSATION_REVIEW_PARAMNAME}=${conversationId}`;
};


const useStyles = makeStyles<{}>((theme: any) => ({
    headerRow: {
        borderBottom: theme.palette.common.black,
    },
    title: {
        padding: "1rem",
    },
    link: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    delete: {
        minWidth: "8ch",
    },
}));

export const EnquiriesTableRow = ({ enquiry, toggleSelected, markAsSeen, selected }: EnquiriesTableRowProps) => {
    const cls = useStyles();
    const history = useHistory();

    const { setUnseenNotifications, unseenNotifications } = React.useContext(DashboardContext);

    const update = async () => {
        await markAsSeen(enquiry.conversationId);
        setUnseenNotifications(unseenNotifications - 1);
    };

    const responseLinkOnClick = async () => {
        await update();
        window.open(enquiry.fileAssetResource.link, "_blank");
    };

    const convoDetailsOnClick = async () => {
        await update();
        const url = formConversationReviewPath(enquiry.conversationId);
        history.push(url);
    };

    const { formattedDate, formattedTime } = formatLegitTimeStamp(enquiry.timeStamp);

    return (
        <TableRow style={{ backgroundColor: enquiry.seen ? "white" : "lightgray", fontWeight: enquiry.seen ? "normal" : "bold" }} key={enquiry.conversationId}>
            <EnquiryTableRowCell>
                <Checkbox
                    checked={selected}
                    onClick={() => {
                        toggleSelected(enquiry.conversationId);
                    }}
                />
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography>{enquiry.name}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography>{enquiry.email}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography>{enquiry.phoneNumber}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Link className={cls.link} onClick={async () => await convoDetailsOnClick()}>
                    <Typography>History</Typography>
                </Link>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                {enquiry.hasResponse ? (
                    <Link className={cls.link} onClick={async () => await responseLinkOnClick()}>
                        <Typography>PDF</Typography>
                    </Link>
                ) : (
                    <Typography variant="body2">N/A</Typography>
                )}
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography>{enquiry.intentName}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <EnquiryTimeStamp formattedDate={formattedDate} formattedTime={formattedTime} />
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <></>
            </EnquiryTableRowCell>
        </TableRow>
    );
};
