import { TableRow, TableCell } from "@material-ui/core";
import React from "react";
import { LinkButton } from "./LinkButton";
import AttachmentIcon from "@material-ui/icons/Attachment";
import { FileAssetResource } from "@common/types/api/EntityResources";

interface IAttachmentTableRow {
    fileName: string;
    fileId: string;
    link: string;
    setCurrentPreview: any; // func
    removeAttachment: any; //func
}

var tableCellStyle = {
    padding: "2rem",
    margin: "3rem",
};

var tableRowStyle = {
    margin: "3rem",
    padding: "10rem",
    backgroundColor: "#C7ECEE",
};

export const AttachmentListRow = ({ fileName, link, fileId, setCurrentPreview, removeAttachment }: IAttachmentTableRow) => {
    const viewButtonClickAction = (fileName: string, link: string, fileId: string) => {
        const attachmentLink: FileAssetResource = {
            fileName,
            fileId,
            link,
        };
        setCurrentPreview(attachmentLink);
    };

    const removeButtonClickAction = (fileName: string, link: string, fileId: string) => {
        removeAttachment(fileId);
        setCurrentPreview(null!);
    };

    return (
        <TableRow style={tableRowStyle}>
            <TableCell style={tableCellStyle}>
                <AttachmentIcon />
            </TableCell>
            <TableCell style={tableCellStyle}>
                <strong>{fileName}</strong>
            </TableCell>
            <TableCell style={tableCellStyle}>
                <LinkButton color="primary" link={link} fileName={fileName} fileId={fileId} clickAction={viewButtonClickAction} childText="View" />
            </TableCell>
            <TableCell style={tableCellStyle}>
                <LinkButton color="secondary" link={link} fileName={fileName} fileId={fileId} clickAction={removeButtonClickAction} childText="Remove" />
            </TableCell>
        </TableRow>
    );
};
