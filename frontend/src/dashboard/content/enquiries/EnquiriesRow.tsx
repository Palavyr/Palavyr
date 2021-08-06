import React, { useContext } from "react";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableRow, Typography } from "@material-ui/core";
import { Enquiries, EnquiryRow, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useState } from "react";
import { EnquiryTableRowCell } from "./EnquiriesTableRowCell";
import { formatLegitTimeStamp } from "./enquiriesUtils";
import { EnquiryTimeStamp } from "./EnquiryTimeStamp";

export interface EnquiriesTableRowProps {
    enquiry: EnquiryRow;
    setEnquiries: SetState<Enquiries>;
    index: number;
}

const formConversationReviewPath = (conversationId: string) => {
    return CONVERSATION_REVIEW + `?${CONVERSATION_REVIEW_PARAMNAME}=${conversationId}`;
};

const useStyles = makeStyles((theme) => ({
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

export const EnquiriesTableRow = ({ enquiry, setEnquiries, index }: EnquiriesTableRowProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const history = useHistory();

    const [deleteIsWorking, setDeleteIsWorking] = useState<boolean>(false);

    const { setUnseenNotifications } = React.useContext(DashboardContext);

    const markAsSeen = async (conversationId: string) => {
        const enquiries = await repository.Enquiries.updateEnquiry(conversationId);
        setEnquiries(enquiries);
    };

    const toggleSeenValue = async (conversationId: string) => {
        const enqs = await repository.Enquiries.updateEnquiry(conversationId);
        const numUnseen = enqs.filter((x: EnquiryRow) => !x.seen).length;
        setUnseenNotifications(numUnseen);
        setEnquiries(enqs);
    };

    const responseLinkOnClick = async (enquiry: EnquiryRow) => {
        markAsSeen(enquiry.conversationId);
        const signedUrl = await repository.Enquiries.getSignedUrl(enquiry.linkReference.fileReference);
        window.open(signedUrl, "_blank");
    };

    const convoDetailsOnClick = async (enquiry: EnquiryRow) => {
        const url = formConversationReviewPath(enquiry.conversationId);
        markAsSeen(enquiry.conversationId);
        history.push(url);
    };

    const deleteEnquiryOnClick = async (enquiry: EnquiryRow) => {
        setDeleteIsWorking(true);
        setTimeout(async () => {
            const enquiries = await repository.Enquiries.deleteSelectedEnquiries([enquiry.conversationId]);
            setEnquiries(enquiries);
            setDeleteIsWorking(false);
        }, 1500);
    };

    const { formattedDate, formattedTime } = formatLegitTimeStamp(enquiry.timeStamp);

    return (
        <TableRow style={{ backgroundColor: enquiry.seen ? "white" : "lightgray", fontWeight: enquiry.seen ? "normal" : "bold" }} key={enquiry.conversationId}>
            <EnquiryTableRowCell>
                <Typography>{index + 1}</Typography>
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
                <Link className={cls.link} onClick={() => convoDetailsOnClick(enquiry)}>
                    <Typography>History</Typography>
                </Link>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                {enquiry.hasResponse ? (
                    <Link className={cls.link} onClick={() => responseLinkOnClick(enquiry)}>
                        <Typography>PDF</Typography>
                    </Link>
                ) : (
                    <Typography variant="body2">N/A</Typography>
                )}
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography>{enquiry.areaName}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <EnquiryTimeStamp formattedDate={formattedDate} formattedTime={formattedTime} />
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Checkbox
                    checked={enquiry.seen}
                    onClick={() => {
                        toggleSeenValue(enquiry.conversationId);
                    }}
                />
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => deleteEnquiryOnClick(enquiry)}>
                    <Typography> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </EnquiryTableRowCell>
        </TableRow>
    );
};
