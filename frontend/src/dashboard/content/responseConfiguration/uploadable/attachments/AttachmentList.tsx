import { FileLink } from "@Palavyr-Types";
import { Container, Paper, TableContainer, Table, TableBody, makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { AttachmentListRow } from "./AttachmentListRow";

interface AttachmentList {
    fileList: Array<FileLink>;
    setCurrentPreview: any; // func
    removeAttachment: any; // func
}

const useStyles = makeStyles((theme) => ({
    paper: {
        paddingTop: "0.5rem",
        paddingBottom: "0.5rem",
        paddingLeft: "2.5rem",

        margingTop: "1rem",
        marginBottom: "2rem",
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.getContrastText(theme.palette.primary.main)
    },
    body: {
        margin: "3rem",
    },
}));

export const AttachmentList = ({ fileList, setCurrentPreview, removeAttachment }: AttachmentList) => {
    const cls = useStyles();
    return (
        <Container style={{ marginTop: "3rem" }}>
            <Paper className={cls.paper}>
                <Typography align="center" variant="h3">Current Attachments</Typography>
            </Paper>
            <TableContainer component={Paper}>
                <Table>
                    <TableBody className={cls.body}>
                        {fileList.map((row: FileLink) => (
                            <AttachmentListRow key={row.fileName} fileName={row.fileName} link={row.link} fileId={row.fileId} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment} />
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Container>
    );
};
