import React from "react";
import { Container, makeStyles, Paper, Typography } from "@material-ui/core";

interface IViewEmailTemplate {
    emailTemplate: string;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    paper: {
        marginTop: "2rem",
        padding: "1.2rem",
        boxShadow: "none"
    },
}));

export const ViewEmailTemplate = ({ emailTemplate }: IViewEmailTemplate) => {
    // Shall maintain no state.
    // state is maintained in the Email configuration, which also handles state for the editor.

    const cls = useStyles();
    return (
        <Container style={{ paddingBottom: "5rem" }}>
            <Typography style={{ marginTop: "2rem" }} variant="h4" align="center">
                Preview
            </Typography>
            <Paper elevation={12} className={cls.paper}>
                <div dangerouslySetInnerHTML={{ __html: emailTemplate }}></div>
            </Paper>
        </Container>
    );
};
