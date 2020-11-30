import { DynamicTableMeta } from "@Palavyr-Types";
import React from "react";
import { CustomSelect } from "./CustomSelect";


export interface IDynamicTableSelector {
    selection: string;
    tableOptions: Array<string>;

    handleChange: any;
}

const selectStyle = {
    marginLeft: "1rem",
    marginRight: "1rem",
    marginBottom: "0rem",
    marginTop: "0.2rem"
}

export const DynamicTableSelector = ({selection, handleChange, tableOptions}: IDynamicTableSelector) => {

    return (
        <div style={selectStyle}>
            <CustomSelect onChange={handleChange} option={selection} options={tableOptions} minWidth={120} helperText="Select table type"/>
        </div>);
};
