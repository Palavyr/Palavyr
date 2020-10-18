import { SelectOneFlatData, TableData } from "./SelectOneFlatTypes";
import React, { Dispatch, SetStateAction } from "react";
import { DynamicTableMeta } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableContainer, Paper, Table, Button, FormControlLabel, Checkbox, AccordionActions, makeStyles } from "@material-ui/core";
import { SelectOneFlatHeader } from "./SelectOneFlatHeader";
import { SelectOneFlatBody } from "./SelectOneFlatBody";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { DynamicTableTypes } from "../../DynamicTableTypes";
import { relative } from "path";


export interface ISelectOneFlat {
    tableData: Array<SelectOneFlatData>;
    setTableData: Dispatch<SetStateAction<TableData>>;
    areaIdentifier: string;
    tableId: string;
    tableTag: string;
    tableMeta: DynamicTableMeta;
    setTableMeta: any;
    deleteAction: any;
}

const useStyles = makeStyles({
    tableStyles: {
        width: "100%",
        padding: ".3rem",
        backgroundColor: "transparent",
        borderTop: "1px solid gray"
    },
    alignLeft: {
        position: "relative",
        top: "50%",
        float: "left",
        paddingLeft: "0.3rem"
    },
    alignRight: {
        position: "relative",
        top: "50%",
        transform: 'translateY(25%)',
        float: "right",
        paddingRight: "0.3rem",
        height: "100%"
    },
    trayWrapper: {
        width: "100%",
    },
    add: {
        marginRight: "0.4rem"
    }
})

export const SelectOneFlatTable = ({ tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: ISelectOneFlat) => {

    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new SelectOneFlatModifier(setTableData);

    return (
        <>
            <TableContainer className={classes.tableStyles} component={Paper} >
                <Table>
                    <SelectOneFlatHeader />
                    <SelectOneFlatBody tableData={tableData} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions >
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button
                            className={classes.add}
                            onClick={() => {
                                modifier.addOption(tableData, client, areaIdentifier, tableId)
                            }}
                            color="primary"
                            variant="contained"
                        >
                            Add Option
                    </Button>
                        <FormControlLabel
                            label="Use Options as Paths"
                            control={
                                <Checkbox
                                    checked={tableMeta.valuesAsPaths}
                                    value={"WTF"}
                                    name={"WTF2"}
                                    onChange={async (event) => {
                                        var checked = event.target.checked;
                                        tableMeta.valuesAsPaths = checked;
                                        var res = await client.Configuration.Tables.Dynamic.updateDynamicTableMeta(tableMeta);
                                        var newTableMeta = res.data as DynamicTableMeta;
                                        setTableMeta(newTableMeta);
                                    }}
                                />
                            }
                        />
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel
                            onDelete={() => {
                                deleteAction()
                            }}
                            onSave={() => {
                                client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.SelectOneFlat, tableData, tableId, tableTag);
                                setTableData(tableData);
                                console.log("Saving...");
                            }}
                            onCancel={() => {
                                window.location.reload();
                                console.log("Canceling...");
                            }}
                        />
                    </div>
                </div>
            </AccordionActions >
        </>
    )
}