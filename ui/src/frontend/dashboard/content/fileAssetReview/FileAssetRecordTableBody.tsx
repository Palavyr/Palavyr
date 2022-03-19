import { TableBody } from "@material-ui/core";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import React from "react";
import { FileAssetRecordTableRow } from "./FileAssetRecordTableRow";
import { FileDetails } from "./FileAssetReview";

export interface FileAssetRecordTableBodyProps {
    fileAssetResources: FileAssetResource[];
    setFileAssetResourceRecord: SetState<FileAssetResource[]>;
    setCurrentPreview: SetState<FileDetails>;
    setShowSpinner: SetState<boolean>;
}

export const FileAssetRecordTableBody = ({ fileAssetResources, setFileAssetResourceRecord, setCurrentPreview, setShowSpinner }: FileAssetRecordTableBodyProps) => {
    return (
        <TableBody>
            {fileAssetResources.map((record: FileAssetResource, index: number) => (
                <FileAssetRecordTableRow
                    key={record.fileId}
                    setShowSpinner={setShowSpinner}
                    fileAssetResource={record}
                    setFileAssetResourceRecord={setFileAssetResourceRecord}
                    index={index}
                    setCurrentPreview={setCurrentPreview}
                />
            ))}
        </TableBody>
    );
};
