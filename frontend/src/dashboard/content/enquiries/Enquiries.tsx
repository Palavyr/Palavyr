import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Enquiries, EnquiryRow } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableRow, TableCell, TableBody, Table, Checkbox, makeStyles, Typography } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import classNames from "classnames";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { webUrl } from "@api-client/clientUtils";

const useStyles = makeStyles((theme) => ({
    headerCell: {
        fontWeight: "bold",
        fontSize: "16pt",
        textAlign: "center"
    },
    headerRow: {
        borderBottom: "3px solid black",
    },
    title: {
        padding: "1rem",
    },
    tableCell: {
        textAlign: "center"
    }
}));

export const Enquires = () => {
    const client = new ApiClient();
    const cls = useStyles();

    const [enquiries, setEnquiries] = useState<Enquiries>([]);

    const loadEnquiries = useCallback(async () => {
        var { data: enqs } = await client.Enquiries.getEnquiries();
        setEnquiries(enqs);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const numberPropertyGetter = (enquiry: EnquiryRow) => {
        return enquiry.id;
    };

    const toggleSeenValue = async (conversationId: string) => {
        await client.Enquiries.updateEnquiry(conversationId);
        var { data: enqs } = await client.Enquiries.getEnquiries();
        setEnquiries(enqs);
    };

    const formConversationReviewPath = (conversationId: string) => {
        const convoPath = webUrl + CONVERSATION_REVIEW + `?${CONVERSATION_REVIEW_PARAMNAME}=${conversationId}`;
        console.log(convoPath);
        return convoPath;
    };

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <div>
            <Typography className={cls.title} align="center" variant="h3">
                Enquiries
            </Typography>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow className={cls.headerRow}>
                            <TableCell className={classNames(cls.headerCell)}></TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Client</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Email</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Phone Number</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Conversation</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Estimate</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Area</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Time</TableCell>
                            <TableCell className={classNames(cls.headerCell)}>Seen</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {sortByPropertyNumeric(numberPropertyGetter, enquiries).map((enq: EnquiryRow, index: number) => {
                            return (
                                <TableRow style={{ backgroundColor: enq.seen ? "white" : "lightgray", fontWeight: enq.seen ? "normal" : "bold" }} key={enq.conversationId}>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "a"}>{index + 1}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "b"}>{enq.name}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "c"}>{enq.email}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "d"}>{enq.phoneNumber}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "e"}>
                                        <a href={formConversationReviewPath(enq.conversationId)}>Conversation Details</a>
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "f"}>
                                        <a href={enq.responsePdfLink.link}>Response PDF</a>
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "g"}>{enq.areaName}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "h"}>{enq.timeStamp}</TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "i"}>
                                        <Checkbox checked={enq.seen} onClick={() => toggleSeenValue(enq.conversationId)}></Checkbox>
                                    </TableCell>
                                </TableRow>
                            );
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    );
};
