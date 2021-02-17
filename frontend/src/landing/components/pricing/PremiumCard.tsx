import React from "react";
import { Typography, Table, TableBody, TableRow, TableCell } from "@material-ui/core";
import { useStyles } from "./cardStyles";
import CardMembershipIcon from "@material-ui/icons/CardMembership";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import { ICard } from "./ICard";

export const PremiumCard = ({ border }: ICard) => {
    const cls = useStyles({ border });

    return (
        <>
            <CardMembershipIcon className={cls.icon} />
            <Typography className={cls.title} variant="h5">
                Premium
            </Typography>
            <Typography className={cls.price} variant="h3">
                $50
            </Typography>
            <Typography className={cls.price} variant="h5">
                /month
            </Typography>
            <div className={cls.tablecontainer}>
                <Table className={cls.table}>
                    <TableBody className={cls.tableBody}>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                PDF Response
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                <CheckCircleIcon className={cls.yes} />
                            </TableCell>
                        </TableRow>

                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Configurable Email Response
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                <CheckCircleIcon className={cls.yes} />
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Areas
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                6
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Enquiries Dashboard
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                <CheckCircleIcon className={cls.yes} />
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Attachments
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                1 / Area
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Tree Depth
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                Unlimited
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Static Fee Tables
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                2
                            </TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </div>
        </>
    );
};
