import React from "react";
import { Paper, Typography, Table, TableBody, TableRow, TableCell } from "@material-ui/core";
import classNames from "classnames";
import FreeBreakfastIcon from "@material-ui/icons/FreeBreakfast";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import NotInterestedIcon from "@material-ui/icons/NotInterested";

import { useStyles } from "./cardStyles";
import { ICard } from "./ICard";

export const FreeCard = ({ border }: ICard) => {
    const cls = useStyles({ border });

    return (
        <>
            <FreeBreakfastIcon className={cls.icon} />
            <Typography className={cls.title} variant="h5">
                Lyte
            </Typography>
            <Typography className={cls.price} variant="h3">
                FREE
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
                                Enquiries Dashboard
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                <NotInterestedIcon className={cls.no} />
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Attachments
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                <NotInterestedIcon className={cls.no} />
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Tree Depth
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                3
                            </TableCell>
                        </TableRow>
                        <TableRow className={cls.tableRow}>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                                Static Fee Tables
                            </TableCell>
                            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                                1
                            </TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </div>
        </>
    );
};
