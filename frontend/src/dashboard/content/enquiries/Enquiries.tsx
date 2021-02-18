import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Enquiries, EnquiryRow } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableRow, TableCell, TableBody, Table, Checkbox, makeStyles, Typography } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import classNames from "classnames";
import { CONVERSATION_REVIEW, CONVERSATION_REVIEW_PARAMNAME } from "@constants";
import { webUrl } from "@api-client/clientUtils";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

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

export const Enquires = () => {
    const client = new ApiClient();
    const cls = useStyles();

    const [enquiries, setEnquiries] = useState<Enquiries>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const { setIsLoading } = React.useContext(DashboardContext);

    const loadEnquiries = useCallback(async () => {
        var { data: enqs } = await client.Enquiries.getEnquiries();
        setEnquiries(enqs);
        setLoading(false);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [setLoading]);

    const numberPropertyGetter = (enquiry: EnquiryRow) => {
        return enquiry.id;
    };

    const toggleSeenValue = async (conversationId: string) => {
        setIsLoading(true);
        await client.Enquiries.updateEnquiry(conversationId);
        var { data: enqs } = await client.Enquiries.getEnquiries();
        setEnquiries(enqs);
        setIsLoading(false);
    };

    const formConversationReviewPath = (conversationId: string) => {
        return webUrl + CONVERSATION_REVIEW + `?${CONVERSATION_REVIEW_PARAMNAME}=${conversationId}`;
    };

    const markAsSeen = async (conversationId: string) => {
        setIsLoading(true);
        await client.Enquiries.updateEnquiry(conversationId);
        setIsLoading(false);
    };

    useEffect(() => {
        setIsLoading(true);
        loadEnquiries();
    }, [loadEnquiries]);

    const NoDataAvailable = () => {
        return (
            <div style={{ paddingTop: "3rem" }}>
                <Typography align="center" variant="h4">
                    There are no completed enquires yet.
                </Typography>
                ;
            </div>
        );
    };

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
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "a"}>
                                        {index + 1}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "b"}>
                                        {enq.name}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "c"}>
                                        {enq.email}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "d"}>
                                        {enq.phoneNumber}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "e"}>
                                        <a onClick={() => markAsSeen(enq.conversationId)} href={formConversationReviewPath(enq.conversationId)}>
                                            Conversation Details
                                        </a>
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "f"}>
                                        <a target="_blank" onClick={() => markAsSeen(enq.conversationId)} href={enq.responsePdfLink.link}>Response PDF</a>
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "g"}>
                                        {enq.areaName}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "h"}>
                                        {enq.timeStamp}
                                    </TableCell>
                                    <TableCell className={cls.tableCell} key={enq.conversationId + "i"}>
                                        <Checkbox
                                            checked={enq.seen}
                                            onClick={() => {
                                                toggleSeenValue(enq.conversationId);
                                            }}
                                        ></Checkbox>
                                    </TableCell>
                                </TableRow>
                            );
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
            {!loading && enquiries.length === 0 && <NoDataAvailable />}
        </div>
    );
};
