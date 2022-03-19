import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableCell, TableHead, TableRow, useTheme } from "@material-ui/core";
import React from "react";

export interface FileAssetRecordTableHeaderProps {
    extension: string;
}
export const FileAssetRecordTableHeader = ({ extension }: FileAssetRecordTableHeaderProps) => {
    const theme = useTheme();
    return (
        <TableHead style={{ border: "none" }}>
            <TableRow style={{ border: "none", backgroundColor: theme.palette.secondary.light }}>
                <TableCell style={{ border: "none", borderTopLeftRadius: "10px" }}></TableCell>
                <TableCell style={{ border: "none" }}>
                    <PalavyrText style={{ fontSize: "18pt", fontWeight: 600 }}>{extension.toLocaleUpperCase()}</PalavyrText>
                </TableCell>
                <TableCell style={{ border: "none", borderTopRightRadius: "10px" }}></TableCell>
            </TableRow>
        </TableHead>
    );
};
