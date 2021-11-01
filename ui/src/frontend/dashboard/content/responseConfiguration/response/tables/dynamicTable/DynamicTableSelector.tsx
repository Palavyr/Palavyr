import { makeStyles } from "@material-ui/core";
import React from "react";
import { CustomSelect } from "./CustomSelect";

export interface IDynamicTableSelector {
    selection: string;
    tableOptions: Array<string>;
    handleChange: any;
    disabled?: boolean;
    toolTipTitle?: string;
}


const useStyles = makeStyles(theme => ({
    selector: {
        marginLeft: "1rem",
        marginRight: "1rem",
        marginBottom: "0.3rem",
        // marginTop: "0.2rem",
    },
}));

export const DynamicTableSelector = ({ toolTipTitle, disabled, selection, handleChange, tableOptions }: IDynamicTableSelector) => {
    const cls = useStyles();
    return (
        <div className={cls.selector}>
            <CustomSelect toolTipTitle={toolTipTitle} disabled={disabled} onChange={handleChange} option={selection} options={tableOptions} minWidth={120} helperText="Select Pricing Strategy" />
        </div>
    );
};
