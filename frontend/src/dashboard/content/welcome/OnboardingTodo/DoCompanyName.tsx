import { TableCell, TableRow } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

export const DoCompanyName = () => {
    return (
        <TableRow>
            <TableCell>
                <Link to={"/dashboard/settings/companyName?tab=2"}>Company Name</Link>
            </TableCell>
        </TableRow>
    );
};
