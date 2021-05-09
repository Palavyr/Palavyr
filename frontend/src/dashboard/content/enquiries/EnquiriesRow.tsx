import React from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { Checkbox, Link, makeStyles, TableRow, Typography } from "@material-ui/core";
import { Enquiries, EnquiryRow, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useState } from "react";
import { EnquiryTableRowCell } from "./EnquiriesTableRowCell";
import { formatTimeStamp } from "./enquiriesUtils";
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

    const { formattedDate, formattedTime } = formatTimeStamp(enquiry.timeStamp);

    return (
        <TableRow style={{ backgroundColor: enquiry.seen ? "white" : "lightgray", fontWeight: enquiry.seen ? "normal" : "bold" }} key={enquiry.conversationId}>
            <EnquiryTableRowCell>
                <Typography variant="caption">{index + 1}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography variant="caption">{enquiry.name}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography variant="caption">{enquiry.email}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography variant="caption">{enquiry.phoneNumber}</Typography>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Link className={cls.link} onClick={() => convoDetailsOnClick(enquiry)}>
                    <Typography variant="caption">History</Typography>
                </Link>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Link className={cls.link} onClick={() => responseLinkOnClick(enquiry)}>
                    <Typography variant="caption">PDF</Typography>
                </Link>
            </EnquiryTableRowCell>
            <EnquiryTableRowCell>
                <Typography variant="caption">{enquiry.areaName}</Typography>
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
                    <Typography variant="caption"> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </EnquiryTableRowCell>
        </TableRow>
    );
};
