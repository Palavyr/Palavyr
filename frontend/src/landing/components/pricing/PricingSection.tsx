import { Card, Grid, makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@material-ui/core'
import React from 'react'
import FreeBreakfastIcon from '@material-ui/icons/FreeBreakfast';
import classNames from 'classnames';
import CardMembershipIcon from '@material-ui/icons/CardMembership';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import NotInterestedIcon from '@material-ui/icons/NotInterested';

const useStyles = makeStyles(theme => ({
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)"

    },
    paperCommon: {
        margin: "1rem",
        width: "30%",
        textAlign: "center",
        borderRadius: "0px",
        alignContent: "center"
    },
    paperFree: {
        backgroundColor: "#0093CB",
        color: "white"

    },
    paperPremium: {
        backgroundColor: "#014B91",
        color: "white"

    },
    paperPro: {
        backgroundColor: "#011E6D",
        color: "white"

    },
    icon: {
        marginTop: "2rem",
        marginBottom: "1rem",
        fontSize: "45pt"
    },
    title: {
        paddingTop: "1rem",
        paddingBottom: "1rem"
    },
    tablecontainer: {
        marginRight: "10%",
        marginLeft: "10%",
        marginBottom: "2rem",
        textAlign: "center",
    },
    table: {
        padding: "0.3rem",
    },
    tableBody: {
        padding: "1rem",
    },
    tableRow: {
        borderBottom: "2px solid lightgray",
        margin: "0px"
    },
    tableRoot: {
        paddingTop: "0.8rem",
        paddingBottom: "0.8rem",

    },
    tablecellLeft: {
        padding: "0px",
        fontSize: "16px",
        color: "white"
    },
    tablecellRight: {
        textAlign: "center",
        fontSize: "16px",
        color: "white"
    },
    yes: {
        color: "#01C448"
    },
    no: {
        color: "#E11010"
    }

    // background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)"

}))

export const PricingSection = () => {
    const classes = useStyles();


    // Custom Emails	Yes	Yes	Yes
    // Equiries Dashboard	No	Yes	Yes
    // Attachments	No	1/ area	Unlimited
    // Number of Questions	3	Unlimited	Unlimited
    // Number of Tables	1	Unlimited	Unlimited

    return (
        <section className={classes.body}>
            <Paper className={classNames(classes.paperCommon, classes.paperFree)} variant="outlined">
                <FreeBreakfastIcon className={classes.icon} />
                <Typography className={classes.title} variant="h3">Free</Typography>
                <div className={classes.tablecontainer}>
                    <Table className={classes.table}>
                        <TableBody className={classes.tableBody}>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">PDF Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>

                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Configurable Email Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Enquiries Dashboard</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><NotInterestedIcon className={classes.no} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Attachments</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><NotInterestedIcon className={classes.no} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Tree Depth</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">3</TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Static Fee Tables</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">1</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </div>
            </Paper>
            <Paper className={classNames(classes.paperCommon, classes.paperPremium)} variant="outlined">
                <CardMembershipIcon className={classes.icon} />
                <Typography className={classes.title} variant="h3">Premium</Typography>
                <div className={classes.tablecontainer}>
                    <Table className={classes.table}>
                        <TableBody className={classes.tableBody}>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">PDF Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>

                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Configurable Email Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Enquiries Dashboard</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Attachments</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">1 / Area</TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Tree Depth</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">Unlimited</TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Static Fee Tables</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">2</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </div>
            </Paper >
            <Paper className={classNames(classes.paperCommon, classes.paperPro)} variant="outlined">
                <AccountBalanceIcon className={classes.icon} />
                <Typography className={classes.title} variant="h3">Pro</Typography>
                <div className={classes.tablecontainer}>
                    <Table className={classes.table}>
                        <TableBody className={classes.tableBody}>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">PDF Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>

                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Configurable Email Response</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Enquiries Dashboard</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right"><CheckCircleIcon className={classes.yes} /></TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Attachments</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">Unlimited</TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Tree Depth</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">Unlimited</TableCell>
                            </TableRow>
                            <TableRow className={classes.tableRow}>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellLeft} align="left">Static Fee Tables</TableCell>
                                <TableCell classes={{ root: classes.tableRoot }} className={classes.tablecellRight} align="right">Unlimited</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </div>
            </Paper>
        </section >
    )
}