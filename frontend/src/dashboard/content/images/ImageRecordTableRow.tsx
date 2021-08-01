import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { makeStyles, TableRow, TableCell, Typography, Link } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useState } from "react";
import { useContext } from "react";

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
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const [deleteIsWorking, setDeleteIsWorking] = useState<boolean>(false);
    const [loading, setIsLoading] = useState<boolean>(false);

    const onDeleteTableClick = async (imageRecord: FileLink) => {
        // 1. Delete Image record
        // 2. Delete any convo nodes that reference this in the imageId col
        setDeleteIsWorking(true);
        const fileId = imageRecord.fileId;
        const updatedImageRecords = await repository.Configuration.Images.deleteImage([fileId]);
        setImageRecords(updatedImageRecords);
        setDeleteIsWorking(false);
    };

    const responseLinkOnClick = async (fileLink: FileLink) => {
        setIsLoading(true);
        setShowSpinner(true);
        if (!fileLink.isUrl) { // TODO: Always going to be s3 now, so remove this URL nonsense. Links are handled natively via the html nature of the chat text
            const signedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.s3Key, fileLink.fileId);
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
