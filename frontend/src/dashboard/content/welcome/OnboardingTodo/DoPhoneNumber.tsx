import { TableRow, TableCell } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

export const DoPhoneNumber = () => {
    return (
        <TableRow>
            <TableCell>
                <Link to={"dashboard/settings/phoneNumber?tab=3"}>Set your default contact phone number in the settings page</Link>
            </TableCell>
        </TableRow>
    );
};
