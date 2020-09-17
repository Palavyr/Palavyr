import React from "react"
import { EnquiryRow } from "@Palavyr-Types"
import { TableBody, TableRow, TableCell } from "@material-ui/core"

interface IEnquiriesBody {
    enquiries: Array<EnquiryRow>
}

export const EnquiriesTableBody = ({ enquiries }: IEnquiriesBody) => {
    return (
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
    )
}