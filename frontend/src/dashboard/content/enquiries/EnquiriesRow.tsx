import React from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { Enquiries, EnquiryRow, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useState } from "react";

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
    tableCell: {
        textAlign: "center",
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
    const repository = new PalavyrRepository();
    const history = useHistory();

    const [deleteIsWorking, setDeleteIsWorking] = useState<boolean>(false);

    const { setIsLoading } = React.useContext(DashboardContext);

    const markAsSeen = async (conversationId: string) => {
        setIsLoading(true);
        await repository.Enquiries.updateEnquiry(conversationId);
        setIsLoading(false);
    };

    const toggleSeenValue = async (conversationId: string) => {
        setIsLoading(true);
        const enqs = await repository.Enquiries.updateEnquiry(conversationId);
        setEnquiries(enqs);
        setIsLoading(false);
    };

    const responseLinkOnClick = async (enquiry: EnquiryRow) => {
        setIsLoading(true);
        markAsSeen(enquiry.conversationId);
        const signedUrl = await repository.Enquiries.getSignedUrl(enquiry.linkReference.fileReference);
        window.open(signedUrl, "_blank");
        setIsLoading(false);
    };

    const convoDetailsOnClick = async (enquiry: EnquiryRow) => {
        setIsLoading(true);
        const url = formConversationReviewPath(enquiry.conversationId);
        markAsSeen(enquiry.conversationId);
        history.push(url);
        setIsLoading(false);
    };

    const deleteEnquiryOnClick = async (enquiry: EnquiryRow) => {
        setIsLoading(true);
        setDeleteIsWorking(true);
        const enquiries = await repository.Enquiries.deleteEnquiry(enquiry.linkReference.fileReference);
        setEnquiries(enquiries);
        setIsLoading(false);
        setDeleteIsWorking(false);
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
                <Typography variant="caption">{formattedDate}</Typography>
                <Typography variant="caption">{formattedTime}</Typography>
            </>
        );
    };

    return (
        <TableRow style={{ backgroundColor: enquiry.seen ? "white" : "lightgray", fontWeight: enquiry.seen ? "normal" : "bold" }} key={enquiry.conversationId}>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "a"}>
                <Typography variant="caption">{index + 1}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "b"}>
                <Typography variant="caption">{enquiry.name}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "c"}>
                <Typography variant="caption">{enquiry.email}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "d"}>
                <Typography variant="caption">{enquiry.phoneNumber}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "e"}>
                <Link className={cls.link} onClick={() => convoDetailsOnClick(enquiry)}>
                    <Typography variant="caption">History</Typography>
                </Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "f"}>
                <Link className={cls.link} onClick={() => responseLinkOnClick(enquiry)}>
                    <Typography variant="caption">PDF</Typography>
                </Link>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "g"}>
                <Typography variant="caption">{enquiry.areaName}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "h"}>
                <Typography variant="caption">{formatTimeStamp(enquiry.timeStamp)}</Typography>
            </TableCell>
            <TableCell className={cls.tableCell} key={enquiry.conversationId + "i"}>
                <Checkbox
                    checked={enquiry.seen}
                    onClick={() => {
                        toggleSeenValue(enquiry.conversationId);
                    }}
                ></Checkbox>
            </TableCell>
            <TableCell>
                <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => deleteEnquiryOnClick(enquiry)}>
                    <Typography variant="caption"> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </TableCell>
        </TableRow>
    );
};
