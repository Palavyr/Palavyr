import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { makeStyles, TableRow, TableCell, Typography, Link } from "@material-ui/core";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useState } from "react";
import { useContext } from "react";

export interface FileAssetRecordTableRowProps {
    fileAssetResource: FileAssetResource;
    setFileAssetResourceRecord: SetState<FileAssetResource[]>;
    index: number;
    setCurrentPreview: SetState<string>;
    setShowSpinner: SetState<boolean>;
}

const useStyles = makeStyles(theme => ({
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

export const FileAssetRecordTableRow = ({ fileAssetResource, setFileAssetResourceRecord, index, setCurrentPreview, setShowSpinner }: FileAssetRecordTableRowProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const [deleteIsWorking, setDeleteIsWorking] = useState<boolean>(false);
    const [loading, setIsLoading] = useState<boolean>(false);

    const onDeleteTableClick = async (fileAssetResource: FileAssetResource) => {
        // 1. Delete Image record
        // 2. Delete any convo nodes that reference this in the imageId col
        setDeleteIsWorking(true);
        const fileId = fileAssetResource.fileId;
        const updatedImageRecords = await repository.Configuration.FileAssets.DeleteFileAsset([fileId]);
        setFileAssetResourceRecord(updatedImageRecords);
        setDeleteIsWorking(false);
    };

    const responseLinkOnClick = async (fileAssetResource: FileAssetResource) => {
        setIsLoading(true);
        setShowSpinner(true);
        setCurrentPreview(fileAssetResource.link);

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
                    {fileAssetResource.fileName}
                </Typography>
            </TableCell>
            <TableCell>
                <Link className={cls.link} onClick={() => responseLinkOnClick(fileAssetResource)}>
                    <Typography variant="body1">File Preview</Typography>
                </Link>
            </TableCell>
            <TableCell>
                <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => onDeleteTableClick(fileAssetResource)}>
                    <Typography variant="caption"> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </TableCell>
        </TableRow>
    );
};
