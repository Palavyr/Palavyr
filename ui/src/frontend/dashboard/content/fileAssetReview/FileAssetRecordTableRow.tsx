import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { makeStyles, TableRow, TableCell, Typography } from "@material-ui/core";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useState } from "react";
import { useContext } from "react";
import { FileDetails } from "./FileAssetReview";

export interface FileAssetRecordTableRowProps {
    fileAssetResource: FileAssetResource;
    setFileAssetResourceRecord: SetState<FileAssetResource[]>;
    index: number;
    setCurrentPreview: SetState<FileDetails>;
    setShowSpinner: SetState<boolean>;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    delete: {
        backgroundColor: theme.palette.warning.main,
        border: "none",
        "&:hover": {
            border: "none",
            color: "white",
            backgroundColor: theme.palette.error.main,
        },
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

        const extension = fileAssetResource.fileName.split(".").pop() ?? "";
        setCurrentPreview({ link: fileAssetResource.link, extension });

        setIsLoading(false);
        setShowSpinner(false);
    };

    return (
        <TableRow>
            <TableCell width={50}>
                <Typography>{index}</Typography>
            </TableCell>
            <TableCell align="left">
                <ColoredButton variant="outlined" onClick={() => responseLinkOnClick(fileAssetResource)}>
                    {fileAssetResource.fileName}
                </ColoredButton>
            </TableCell>
            <TableCell align="right">
                <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => onDeleteTableClick(fileAssetResource)}>
                    <Typography variant="caption"> Delete</Typography>
                    {deleteIsWorking && <ButtonCircularProgress />}
                </ColoredButton>
            </TableCell>
        </TableRow>
    );
};
