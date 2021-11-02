import { makeStyles, Table, TableBody } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    tablecontainer: {
        marginRight: "10%",
        marginLeft: "10%",
        marginBottom: "2rem",
        textAlign: "center",
    },
    table: {
        padding: "0.3rem",
        marginTop: "4rem",
        marginBottom: "6rem",
    },
    tableBody: {
        padding: "1rem",
    },

}));

export interface PricingCardProps {
    header: React.ReactNode;
    body: React.ReactNode;
}
export const PricingCard = ({ header, body }: PricingCardProps) => {
    const cls = useStyles();

    return (
        <>
            {header}
            <div className={cls.tablecontainer}>
                <Table className={cls.table}>
                    <TableBody className={cls.tableBody}>{body}</TableBody>
                </Table>
            </div>
        </>
    );
};
