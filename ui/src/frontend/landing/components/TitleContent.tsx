import React from "react";
import { Box, Card, Grid, Divider, makeStyles } from "@material-ui/core";

const useStyles = makeStyles<{}>((theme: any) => ({
    card: {
        boxShadow: "0 0 white",
        background: "none",
        border: "none",
        paddingTop: "1rem",
        paddingBottom: "2rem",
        minWidth: "60%",
    },
}));

export interface ITitleContent {
    title?: React.ReactNode;
    subtitle?: React.ReactNode;
    children?: React.ReactNode;
}
export const TitleContent = ({ title, subtitle, children }: ITitleContent) => {
    const cls = useStyles();

    return (
        <>
            <Card className={cls.card}>
                <Grid container alignContent="center">
                    <Grid item xs={12}>
                        {title && <Box>{title}</Box>}
                    </Grid>
                    <Grid item xs={12}>
                        {subtitle && <Box>{subtitle}</Box>}
                    </Grid>
                    <Divider />
                </Grid>
            </Card>
            {children && children}
        </>
    );
};
