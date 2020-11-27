import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Enquiries, EnquiryRow } from "@Palavyr-Types";
import { Statement } from "@common/components/Statement";
import { Divider, TableContainer, Paper, TableHead, TableRow, TableCell, TableBody, Table } from "@material-ui/core";

export const Enquires = () => {

    const client = new ApiClient();

    const title = "Check your enquiries";
    const details = "This table lists all of the completed enquires you have received. Enquiries you have not checked will be in bold."

    const [enquiries, setEnquiries] = useState<Enquiries>([]);

    const loadEnquiries = useCallback(async () => {
        var {data: enqs} = await client.Enquiries.getEnquiries();
        setEnquiries(enqs)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        loadEnquiries();

    }, [loadEnquiries])


    return (
        <div>
            <Statement title={title} details={details} />
            <Divider />
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Id</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Phone Number</TableCell>
                            <TableCell>Estimate</TableCell>
                            <TableCell>Area</TableCell>
                            <TableCell>Time</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            enquiries.map((enq: EnquiryRow) => {
                                return (

                                    <TableRow style={{ fontWeight: enq.seen ? "normal" : "bold" }} key={enq.conversationId + "whaaa"}>
                                        <TableCell key={enq.conversationId + "a"}>{enq.id}</TableCell>
                                        <TableCell key={enq.conversationId + "b"}>{enq.name}</TableCell>
                                        <TableCell key={enq.conversationId + "c"}>{enq.email}</TableCell>
                                        <TableCell key={enq.conversationId + "d"}>{enq.phoneNumber}</TableCell>
                                        <TableCell key={enq.conversationId + "e"}><a href={enq.responsePdfLink.link}>{enq.responsePdfLink.fileName}</a></TableCell>
                                        <TableCell key={enq.conversationId + "f"}>{enq.areaName}</TableCell>
                                        <TableCell key={enq.conversationId + "g"}>{enq.timeStamp}</TableCell>
                                    </TableRow>
                                )
                            })
                        }
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    )
}