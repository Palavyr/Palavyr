import React from "react";
import { Container, Paper, Typography } from "@material-ui/core";

interface IViewEmailTemplate {
    emailTemplate: string;
}

const style = {
    marginTop: "2rem",
    padding: "1.2rem"
}

export const ViewEmailTemplate = ({ emailTemplate }: IViewEmailTemplate) => {
    // Shall maintain no state.
    // state is maintained in the Email configuration, which also handles state for the editor.

    return (
        <Container style={{paddingBottom: "5rem"}}>
            <Typography style={{marginTop: "2rem"}} variant="h4" align="center">Preview</Typography>
            <Paper elevation={12} style={style}>
                <div dangerouslySetInnerHTML={{ __html: emailTemplate }}></div>
            </Paper>
        </Container>
    )
}