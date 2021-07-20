import React from "react";
import { CustomSelect } from "./CustomSelect";
import { Tooltip } from "@material-ui/core";

export interface IDynamicTableSelector {
    selection: string;
    tableOptions: Array<string>;
    handleChange: any;
    disabled?: boolean;
    toolTipTitle?: string;
}

const selectStyle = {
    marginLeft: "1rem",
    marginRight: "1rem",
    marginBottom: "0rem",
    marginTop: "0.2rem",
};

export const DynamicTableSelector = ({ toolTipTitle, disabled, selection, handleChange, tableOptions }: IDynamicTableSelector) => {
    return (
        <div style={selectStyle}>
            <CustomSelect toolTipTitle={toolTipTitle} disabled={disabled} onChange={handleChange} option={selection} options={tableOptions} minWidth={120} helperText="Select table type" />
        </div>
    );
};
