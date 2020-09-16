import { TableHead, TableRow, TableCell } from "@material-ui/core"
import React from "react"

export const EnquiriesTableHeader = () => {
    return (
        <TableHead>
            <TableRow>
                <TableCell>Id</TableCell>
                <TableCell>Name</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>PhoneNumber</TableCell>
                <TableCell>Estimate</TableCell>
                <TableCell>Area</TableCell>
                <TableCell>Time</TableCell>
            </TableRow>
        </TableHead>
    )
}