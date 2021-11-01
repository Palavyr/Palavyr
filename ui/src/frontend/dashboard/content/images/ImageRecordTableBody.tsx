import { TableBody } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import React from "react";
import { ImageRecordTableRow } from "./ImageRecordTableRow";

export interface ImageRecordTableBodyProps {
    imageRecords: FileLink[];
    setImageRecords: SetState<FileLink[]>;
    setCurrentPreview: SetState<string>;
    setShowSpinner: SetState<boolean>;
}

export const ImageRecordTableBody = ({ imageRecords, setImageRecords, setCurrentPreview, setShowSpinner }: ImageRecordTableBodyProps) => {
    return (
        <TableBody>
            {imageRecords.map((record: FileLink, index: number) => (
                <ImageRecordTableRow key={record.fileId} setShowSpinner={setShowSpinner} imageRecord={record} setImageRecords={setImageRecords} index={index} setCurrentPreview={setCurrentPreview} />
            ))}
        </TableBody>
    );
};
