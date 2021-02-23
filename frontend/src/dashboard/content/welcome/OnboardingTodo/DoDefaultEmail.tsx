import { TableCell, TableRow } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

interface IDoDefaultEmail {
    emailAddress?: string;
    awaitingVerification?: boolean;
}

export const DoDefaultEmail = ({ emailAddress, awaitingVerification }: IDoDefaultEmail) => {
    return (
        <TableRow>
            <TableCell style={{alignContent: "center"}}>
                <Link to={"/dashboard/settings/email?tab=1"}>
                    Set your default email: {emailAddress} - {awaitingVerification ? "Check your email to verify you address" : "Trigger an email verification in your settings."}
                </Link>
            </TableCell>
        </TableRow>
    );
};
