import { TableHead, TableRow, TableCell } from "@material-ui/core"
import React from "react"

export const SelectOneFlatHeader = () => {

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" ></TableCell>
                <TableCell align="center" style={{borderRight: "1px solid lightgray"}}>Option Name</TableCell>
                <TableCell align="center" style={{borderRight: "1px solid lightgray"}}>Value</TableCell>
                <TableCell align="center" >Max Value (if range)</TableCell>
                <TableCell align="center" ></TableCell>
            </TableRow>
        </TableHead>
    )
}
