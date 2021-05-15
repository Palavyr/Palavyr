import { Box, Card, Grid, Typography, Button, Divider, makeStyles } from "@material-ui/core";
import React from "react";
import { NavBar } from "../navbar/NavBar";

const useStyles = makeStyles((theme) => ({
    container: {
        paddingLeft: "15%",
        paddingRight: "15%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        backgroundColor: theme.palette.primary.dark,
        paddingBottom: "3rem",
        textAlign: "center",
    },
    card: {
        boxShadow: "0 0 white",
        background: "none",
        border: "none",
        paddingTop: "2rem",
        paddingBottom: "2rem",
        minWidth: "60%",
    },
    primaryText: {
        color: theme.palette.success.main,
    },
    secondaryText: {
        color: theme.palette.success.dark,
    },
    button: {
        width: "18rem",
        alignSelf: "center",
        backgroundColor: theme.palette.background.default,
        color: theme.palette.common.black,
        "&:hover": {
            backgroundColor: theme.palette.success.light,
            color: theme.palette.common.black,
        },
    },
}));

interface IHeader {
    openRegisterDialog: any;
    openLoginDialog: any;
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    isMobileDrawerOpen: any;
    selectedTab: any;
    setSelectedTab: any;
}

export const Header = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, isMobileDrawerOpen, selectedTab, setSelectedTab }: IHeader) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <NavBar
                openRegisterDialog={openRegisterDialog}
                openLoginDialog={openLoginDialog}
                handleMobileDrawerOpen={handleMobileDrawerOpen}
                handleMobileDrawerClose={handleMobileDrawerClose}
                mobileDrawerOpen={isMobileDrawerOpen}
                selectedTab={selectedTab}
                setSelectedTab={setSelectedTab}
            />
            <Card className={cls.card}>
                <Grid container alignContent="center">
                    <Grid item xs={12}>
                        <Box>
                            <Typography align="center" variant="h2" className={cls.primaryText}>
                                CRAFT INCREDIBLE CONVERSATIONS
                            </Typography>
                        </Box>
                    </Grid>
                    <Grid item xs={12}>
                        <Box>
                            <Typography align="center" variant="h5" className={cls.secondaryText}>
                                Client engagement done <i>YOUR</i> way
                            </Typography>
                        </Box>
                    </Grid>
                    <Divider />
                </Grid>
            </Card>
            <Button className={cls.button} variant="contained" onClick={openRegisterDialog}>
                <Typography variant="h6">Create a free account</Typography>
            </Button>
        </div>
    );
};
