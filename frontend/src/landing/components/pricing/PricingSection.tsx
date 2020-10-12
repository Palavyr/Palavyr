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
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
        paddingRight: "16%",
        paddingLeft: "16%"
    },
    paperCommon: {
        margin: "1rem",
        width: "30%",
        textAlign: "center",
        borderRadius: "0px",
        alignContent: "center",
        marginTop: "3rem"
    },
    paperFree: {
        backgroundColor: "#0093CB",
        color: "white",
        border: "2px dashed white"

    },
    paperPremium: {
        backgroundColor: "#014B91",
        color: "white",
        border: "6px solid black"
    },
    paperPro: {
        backgroundColor: "#011E6D",
        color: "white",
        border: "2px dashed white"

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
    price: {
        paddingTop: "1.2rem",
        paddingBottom: "1.2rem",
        display: 'inline-block'
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

    return (
        <section className={classes.body}>
            <Paper className={classNames(classes.paperCommon, classes.paperFree)} variant="outlined">
                <FreeBreakfastIcon className={classes.icon} />
                <Typography className={classes.title} variant="h5">Lyte</Typography>
                <Typography className={classes.price} variant="h3">FREE</Typography>
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
                <Typography className={classes.title} variant="h5">Premium</Typography>
                <Typography className={classes.price} variant="h3">{"$5 "}</Typography><Typography className={classes.price} variant="h5">{"/ area / month"}</Typography>
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
                <Typography className={classes.title} variant="h5">Pro</Typography>
                {/* <Typography className={classes.price} variant="h3">$75 /</Typography><Typography variant="h5"> month</Typography> */}
                <Typography className={classes.price} variant="h3">{"$75 "}</Typography><Typography className={classes.price} variant="h5">{"/ month"}</Typography>

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