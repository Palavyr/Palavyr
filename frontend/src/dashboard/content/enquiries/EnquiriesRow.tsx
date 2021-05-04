import React from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
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
    link: {
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export const EnquiriesTableRow = ({ enquiry, setEnquiries, index }: EnquiriesTableRowProps) => {
    const cls = useStyles();
    const client = new PalavyrRepository();
    const history = useHistory();

    const { setIsLoading } = React.useContext(DashboardContext);

    const markAsSeen = async (conversationId: string) => {
        setIsLoading(true);
        await client.Enquiries.updateEnquiry(conversationId);
        setIsLoading(false);
    };

    const toggleSeenValue = async (conversationId: string) => {
        setIsLoading(true);
        const enqs = await client.Enquiries.updateEnquiry(conversationId);
        setEnquiries(enqs);
        setIsLoading(false);
    };

    const responseLinkOnClick = async (enquiry: EnquiryRow) => {
        setIsLoading(true);
        markAsSeen(enquiry.conversationId);
        const signedUrl = await client.Enquiries.getSignedUrl(enquiry.linkReference.fileReference);
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

    const formatTimeStamp = (timeStamp: string) => {
        const parts = timeStamp.split("--");

        const time = parts[1].split("-");
        const hour = parseInt(time[0]);
        let formattedHour: number;
        let phase: string;
        if (hour < 12) {
            formattedHour = hour;
            phase = "a.m.";
        } else {
            formattedHour = hour === 12 ? hour : hour - 12;
            phase = "p.m.";
        }

        const formattedTime = `${formattedHour}: ${time[1]} ${phase}`;

        const date = parts[0].split("-");
        const months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
        const formattedDate = `${date[1]} - ${months[parseInt(date[2]) - 1]} - ${date[0]}`;
        return (
            <>
                <Typography>{formattedDate}</Typography>
                <Typography>{formattedTime}</Typography>
            </>
        );
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
                <Link className={cls.link} onClick={() => convoDetailsOnClick(enquiry)}>
                    Conversation Details
                </Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "f"}>
                <Link className={cls.link} onClick={() => responseLinkOnClick(enquiry)}>
                    Response PDF
                </Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "g"}>
                {enquiry.areaName}
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "h"}>
                {formatTimeStamp(enquiry.timeStamp)}
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
