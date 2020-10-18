import { FileLink } from "@Palavyr-Types";
import { Container, Paper, TableContainer, Table, TableBody } from "@material-ui/core";
import React from "react";
import { AttachmentListRow } from "./AttachmentListRow";


interface AttachmentList {
    fileList: Array<FileLink>;
    setCurrentPreview: any; // func
    removeAttachment: any; // func
}

const paperStyle = {
    // padding: "2.5rem",

    paddingTop: "0.5rem",
    paddingBottom: "0.5rem",
    paddingLeft: "2.5rem",

    margingTop: "1rem",
    marginBottom: "2rem",
    background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
}

const tableBodyStyle ={
    margin: "3rem"
}

export const AttachmentList = ({ fileList, setCurrentPreview, removeAttachment}: AttachmentList) => {
    return (
        <Container style={{marginTop: "3rem"}}>
            <Paper elevation={18} style={paperStyle}>
                <h2 style={{color: "white"}}>Current PDF attachments</h2>
            </Paper>
            <TableContainer component={Paper}>
                <Table >
                    <TableBody style={tableBodyStyle}>
                        {fileList.map((row: FileLink) => (
                            <AttachmentListRow key={row.fileName} fileName={row.fileName} link={row.link} fileId={row.fileId} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment}/>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Container>
    );
}
