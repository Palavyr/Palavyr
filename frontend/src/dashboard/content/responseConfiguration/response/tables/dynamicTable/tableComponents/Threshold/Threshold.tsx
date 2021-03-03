import React from "react";
import { IDynamicTableProps } from "../../DynamicTableTypes";
import { ThresholdModifier } from "./ThresholdModifier";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { TableContainer, Table, Button, AccordionActions, makeStyles } from "@material-ui/core";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { ApiClient } from "@api-client/Client";
import { ThresholdHeader } from "./ThresholdHeader";
import { ThresholdBody } from "./ThresholdBody";

const useStyles = makeStyles(() => ({
    alignLeft: {
        position: "relative",
        top: "50%",
        float: "left",
        paddingLeft: "0.3rem",
    },
    alignRight: {
        position: "relative",
        top: "50%",
        transform: "translateY(25%)",
        float: "right",
        paddingRight: "0.3rem",
        height: "100%",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {
        marginRight: "0.4rem",
    },
}));

export const Threshold = ({ tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: Omit<IDynamicTableProps, "tableMeta" | "setTableMeta">) => {
    const cls = useStyles();
    const client = new ApiClient();

    const modifier = new ThresholdModifier(setTableData);

    const onSave = async () => {
        console.log("Saving the table");
        return true;
    };
    const addThresholdOnClick = () => modifier.addThreshold(tableData);

    return (
        <>
            <TableContainer>
                <Table>
                    <ThresholdHeader />
                    <ThresholdBody tableData={tableData} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addThresholdOnClick} color="primary" variant="contained">
                            Add Threshold
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
        </>
    );
};
