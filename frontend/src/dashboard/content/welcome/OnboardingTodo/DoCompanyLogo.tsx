import { TableRow, TableCell } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

export const DoCompanyLogo = () => {
    return (
        <TableRow>
            <TableCell>
                <Link to={"/dashboard/settings/companyLogo?tab=4"}>Set your company logo in the settings page</Link>
            </TableCell>
        </TableRow>
    );
};
