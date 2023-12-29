import { makeStyles, Paper, Card } from "@material-ui/core";
import React from "react";



const useStyles = makeStyles<{}>((theme: any) => ({
    card: {
        margin: "2rem",
        padding: "4rem"
    },
    paper: {
        width: "50%",
        height: "20%",
        padding: "2rem",
        alignContent: "center",
        justifyContent: "center",
        textAlign: "center",
        alignItems: "center",
    }
}))


interface ICodeCard {
    children: React.ReactNode;
}

export const CodeCard = ({ children }: ICodeCard) => {

    const classes = useStyles();

    return (
        <Paper className={classes.paper}>
            <Card className={classes.card}>
                {children}
            </Card>
        </Paper>
    )
}