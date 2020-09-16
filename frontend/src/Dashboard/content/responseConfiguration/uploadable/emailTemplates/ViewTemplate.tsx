import React, { useEffect } from "react";
import { Container, Paper } from "@material-ui/core";

interface IViewEmailTemplate {
    updateableEmailTemplate: string;
    setUpdateableEmailTemplate: any;
}

const style = {
    marginTop: "4rem",
    padding: "1.2rem"
}

export const ViewEmailTemplate = ({ updateableEmailTemplate, setUpdateableEmailTemplate }: IViewEmailTemplate) => {

    useEffect(() => {
        return () => {
            setUpdateableEmailTemplate(null!);
        }
    })
    return (
        <Container >
            <Paper elevation={12} style={style}>
                <div dangerouslySetInnerHTML={{ __html: updateableEmailTemplate }}></div>
            </Paper>
        </Container>
    )
}