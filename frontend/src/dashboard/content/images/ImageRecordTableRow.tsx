import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { makeStyles, TableRow, TableCell, Typography, Link } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import React, { useState } from "react";

export interface ImageRecordTableRowProps {
    imageRecord: FileLink;
    setImageRecords: SetState<FileLink[]>;
    index: number;
    setCurrentPreview: SetState<string>;
    setShowSpinner: SetState<boolean>;
}

const useStyles = makeStyles((theme) => ({
    delete: {
        padding: ".7rem",
        margin: ".4rem",
    },
    link: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    cell: {
        textAlign: "center",
    },
}));

export const ImageRecordTableRow = ({ imageRecord, setImageRecords, index, setCurrentPreview, setShowSpinner }: ImageRecordTableRowProps) => {
    const repo = new PalavyrRepository();
    const cls = useStyles();
    const [deleteIsWorking, setDeleteIsWorking] = useState<boolean>(false);
    const [loading, setIsLoading] = useState<boolean>(false);

    const onDeleteTableClick = async (imageRecord: FileLink) => {
        // 1. Delete Image record
        // 2. Delete any convo nodes that reference this in the imageId col
        setDeleteIsWorking(true);
        const fileId = imageRecord.fileId;
        const updatedImageRecords = await repo.Configuration.Images.deleteImage([fileId]);
        setImageRecords(updatedImageRecords);
        setDeleteIsWorking(false);
    };

    const responseLinkOnClick = async (fileLink: FileLink) => {
        setIsLoading(true);
        setShowSpinner(true);
        if (!fileLink.isUrl) {
            const signedUrl = await repo.Configuration.Images.getSignedUrl(fileLink.link, fileLink.fileId);
            setCurrentPreview(signedUrl);
        } else {
            const url = fileLink.link;
            setCurrentPreview(url);
        }

        setIsLoading(false);
        setShowSpinner(false);
    };

    return (
        <TableRow>
            <TableCell>
                <Typography>{index}</Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="body1">
                    {imageRecord.fileName}
                </Typography>
            </TableCell>
            <TableCell>
                <Link className={cls.link} onClick={() => responseLinkOnClick(imageRecord)}>
                    <Typography variant="body1">Image Preview</Typography>
                </Link>
            </TableCell>
            <TableCell>
                <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => onDeleteTableClick(imageRecord)}>
                    <Typography variant="caption"> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </TableCell>
        </TableRow>
    );
};
