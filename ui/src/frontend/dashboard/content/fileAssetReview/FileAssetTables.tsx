import { Table, TableContainer } from "@material-ui/core";
import React from "react";
import { FileAssetRecordTableHeader } from "./FileAssetRecordTableHeader";
import { FileAssetRecordTableBody } from "./FileAssetRecordTableBody";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import { FileDetails } from "./FileAssetReview";

type FileAssetGroups = { [extension: string]: FileAssetResource[] };

export interface FileAssetTableProps {
    fileAssetResources: FileAssetResource[];
    setFileAssetResourceRecords: SetState<FileAssetResource[]>;
    setCurrentPreview: SetState<FileDetails>;
    setShowSpinner: SetState<boolean>;
}

export const FileAssetTables = ({ fileAssetResources, setFileAssetResourceRecords, setCurrentPreview, setShowSpinner }: FileAssetTableProps) => {
    const groupedFiles = groupFileAssetsByExtension(fileAssetResources);
    const groupKeys = Object.keys(groupedFiles).sort();

    return (
        <TableContainer style={{ width: "100%", paddingLeft: "1rem", paddingRight: "1rem" }}>
            {groupKeys.map(key => {
                const groupResources = groupedFiles[key];
                return (
                    <Table style={{ marginBottom: "0.5rem" }}>
                        <FileAssetRecordTableHeader extension={key} />
                        <FileAssetRecordTableBody
                            fileAssetResources={groupResources}
                            setFileAssetResourceRecord={setFileAssetResourceRecords}
                            setCurrentPreview={setCurrentPreview}
                            setShowSpinner={setShowSpinner}
                        />
                    </Table>
                );
            })}
        </TableContainer>
    );
};

const groupFileAssetsByExtension = (fileAssets: FileAssetResource[]) => {
    const groupedFiles: FileAssetGroups = {};
    fileAssets.forEach(fileAsset => {
        const extension = getExtension(fileAsset);

        if (extension !== undefined) {
            if (!Object.keys(groupedFiles).includes(extension)) {
                groupedFiles[extension] = [fileAsset];
            } else {
                groupedFiles[extension].push(fileAsset);
            }
        }
    });
    return groupedFiles;
};
const getExtension = (fileAsset: FileAssetResource) => {
    const extension = fileAsset.fileName.split(".").pop();
    return extension;
};
