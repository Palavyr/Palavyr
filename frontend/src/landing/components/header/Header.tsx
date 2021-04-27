import { Box, Card, Grid, Typography, Button, Divider, makeStyles } from "@material-ui/core";
import React from "react";
import { NavBar } from "../navbar/NavBar";

const useStyles = makeStyles((theme) => ({
    container: {
        paddingLeft: "15%",
        paddingRight: "15%",
        border: "0px dotted black",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        background: "rgb(1,96,162)",
        paddingBottom: "7rem",
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
    button: {
        width: "18rem",
        alignSelf: "center",
        fontSize: "large",
        backgroundColor: "#90a5bb",
        color: "white",
        "&:hover": {
            backgroundColor: "white",
            color: "navy",
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
                            <Typography align="center" variant="h2">
                                Build Incredible Conversations
                            </Typography>
                        </Box>
                    </Grid>
                    <Grid item xs={12}>
                        <Box>
                            <Typography align="center" variant="h5">
                                Use Palavyr to configure a custom chat widget for your site to engage customers.
                            </Typography>
                        </Box>
                    </Grid>
                    <Divider />
                </Grid>
            </Card>
            <Button className={cls.button} variant="contained" onClick={openRegisterDialog}>
                Create a free account
            </Button>
        </div>
    );
};
