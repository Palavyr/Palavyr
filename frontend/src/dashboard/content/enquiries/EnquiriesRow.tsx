import React from "react";
import { ApiClient } from "@api-client/Client";
import { webUrl } from "@api-client/clientUtils";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableCell, TableRow } from "@material-ui/core";
import { Enquiries, EnquiryRow, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";

export interface EnquiriesTableRowProps {
    enquiry: EnquiryRow;
    setEnquiries: SetState<Enquiries>;
    index: number;
}

const formConversationReviewPath = (conversationId: string) => {
    return CONVERSATION_REVIEW + `?${CONVERSATION_REVIEW_PARAMNAME}=${conversationId}`;
};

const useStyles = makeStyles((theme) => ({
    headerCell: {
        fontWeight: "bold",
        fontSize: "16pt",
        textAlign: "center",
    },
    headerRow: {
        borderBottom: "3px solid black",
    },
    title: {
        padding: "1rem",
    },
    tableCell: {
        textAlign: "center",
    },
}));

export const EnquiriesTableRow = ({ enquiry, setEnquiries, index }: EnquiriesTableRowProps) => {
    const cls = useStyles();
    const client = new ApiClient();
    const history = useHistory();

    const { setIsLoading } = React.useContext(DashboardContext);

    const markAsSeen = async (conversationId: string) => {
        setIsLoading(true);
        await client.Enquiries.updateEnquiry(conversationId);
        setIsLoading(false);
    };

    const toggleSeenValue = async (conversationId: string) => {
        setIsLoading(true);
        const { data: enqs } = await client.Enquiries.updateEnquiry(conversationId);
        setEnquiries(enqs);
        setIsLoading(false);
    };

    const responseLinkOnClick = async (enquiry: EnquiryRow) => {
        setIsLoading(true);
        markAsSeen(enquiry.conversationId);
        const { data: signedUrl } = await client.Enquiries.getSignedUrl(enquiry.linkReference.fileReference);
        window.open(signedUrl, "_blank");
        setIsLoading(false);
    };

    const convoDetailsOnClick = async (enqiury: EnquiryRow) => {
        setIsLoading(true);
        const url = formConversationReviewPath(enquiry.conversationId);
        markAsSeen(enquiry.conversationId);
        history.push(url);
        setIsLoading(false);
    };

    return (
        <TableRow style={{ backgroundColor: enquiry.seen ? "white" : "lightgray", fontWeight: enquiry.seen ? "normal" : "bold" }} key={enquiry.conversationId}>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "a"}>
                {index + 1}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "b"}>
                {enquiry.name}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "c"}>
                {enquiry.email}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "d"}>
                {enquiry.phoneNumber}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "e"}>
                <Link onClick={() => convoDetailsOnClick(enquiry)}>Conversation Details</Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "f"}>
                <Link onClick={() => responseLinkOnClick(enquiry)}>Response PDF</Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "g"}>
                {enquiry.areaName}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "h"}>
                {enquiry.timeStamp}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "i"}>
                <Checkbox
                    checked={enquiry.seen}
                    onClick={() => {
                        toggleSeenValue(enquiry.conversationId);
                    }}
                ></Checkbox>
            </TableCell>
        </TableRow>
    );
};
