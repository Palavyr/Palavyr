import React from "react";
import { makeStyles, TextField, Button, TableRow, Table, TableHead, TableBody, TableCell, Checkbox, FormControlLabel } from "@material-ui/core";
import { StaticTableMetas, StaticTableMeta, StaticTableRow } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import { StaticRow } from "./StaticRow";
import AddBoxIcon from "@material-ui/icons/AddBox";
import ArrowDownwardIcon from "@material-ui/icons/ArrowDownward";
import ArrowUpwardIcon from "@material-ui/icons/ArrowUpward";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";
import Fade from "react-reveal/Fade";

const useStyles = makeStyles(theme => ({
    staticFees: {
        margin: "1.2rem",
        background: theme.palette.secondary.light,
        color: theme.palette.getContrastText(theme.palette.secondary.light),
        padding: "1.3rem",
        border: `1px solid ${theme.palette.common.black}`,
        borderRadius: "7px",
    },
    tableDescription: {
        marginBottom: "0.3rem",
        paddingBottom: "1.5rem",
        borderRadius: "3px",
        minWidth: "50%",
    },
    buttonWrapper: {
        marginTop: "1.5rem",
        marginBottom: "0.5rem",
    },
    feeTableButton: {
        marginLeft: "0.6rem",
        marginRight: "0.6rem",
        padding: "0.4rem",
        paddingLeft: "0.6rem",
        paddingRight: "0.6rem",
    },
    headerText: {
        fontSize: "16pt",
        fontWight: "bold",
    },
}));

export interface IStaticFeeTable {
    staticTableMetas: StaticTableMetas;
    staticTableMeta: StaticTableMeta;
    tableModifier: StaticTablesModifier;
}

// we can produce a list of these components depending on the
export const StaticFeeTable = ({ staticTableMetas, staticTableMeta, tableModifier }: IStaticFeeTable) => {
    const cls = useStyles();
    const cellAlignment = "center";

    const anyStaticTableRowsWithPerIndividualSet = (staticTableMeta: StaticTableMeta) => {
        let result = false;
        staticTableMeta.staticTableRows.forEach((row: StaticTableRow) => {
            if (row.perPerson === true) {
                result = true;
            }
        });
        if (result === false) {
            tableModifier.setPerPersonRequired(staticTableMetas, staticTableMeta.tableOrder, false);
        }
        return result;
    };

    return (
        <Fade>
            <div className={cls.staticFees}>
                <TextField
                    className={cls.tableDescription}
                    multiline
                    rows={2}
                    value={staticTableMetas[staticTableMeta.tableOrder].description}
                    label="Table Description"
                    onChange={event => {
                        tableModifier.setTableDescription(staticTableMetas, staticTableMeta.tableOrder, event.target.value);
                    }}
                />
                <Table>
                    <TableHead>
                        <TableRow style={{ borderBottom: "1px solid black" }}>
                            <TableCell align={cellAlignment} className={cls.headerText}></TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}>
                                Description
                            </TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}>
                                Amount
                            </TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}>
                                Max Amount (if range)
                            </TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}>
                                Range
                            </TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}>
                                Per Individual
                            </TableCell>
                            <TableCell align={cellAlignment} className={cls.headerText}></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody style={{ borderTop: "2px solid black" }}>
                        {staticTableMeta.staticTableRows.map((row: StaticTableRow, index: number) => (
                            <StaticRow
                                key={row.id}
                                index={index}
                                staticTableMetas={staticTableMetas}
                                tableOrder={staticTableMeta.tableOrder}
                                rowOrder={row.rowOrder}
                                modifier={tableModifier}
                                minFee={row.fee.min}
                                maxFee={row.fee.max}
                                rangeState={row.range}
                                perState={row.perPerson}
                                description={row.description}
                            />
                        ))}
                    </TableBody>
                </Table>
                <div className={cls.buttonWrapper}>
                    <Button
                        startIcon={<AddBoxIcon />}
                        variant="contained"
                        size="small"
                        color="primary"
                        className={cls.feeTableButton}
                        onClick={() => tableModifier.addRow(staticTableMetas, staticTableMeta.tableOrder)}
                    >
                        Add Row
                    </Button>

                    {!tableModifier.isTableLastPosition(staticTableMetas, staticTableMeta.tableOrder) && (
                        <Button
                            startIcon={<ArrowDownwardIcon />}
                            variant="contained"
                            size="small"
                            color="primary"
                            className={cls.feeTableButton}
                            onClick={() => tableModifier.moveTableDown(staticTableMetas, staticTableMeta.tableOrder)}
                        >
                            Shift Down
                        </Button>
                    )}

                    {!tableModifier.isTableFirstPosition(staticTableMeta.tableOrder) && (
                        <Button
                            startIcon={<ArrowUpwardIcon />}
                            variant="contained"
                            size="small"
                            color="primary"
                            className={cls.feeTableButton}
                            onClick={() => tableModifier.moveTableUp(staticTableMetas, staticTableMeta.tableOrder)}
                        >
                            Shift Up
                        </Button>
                    )}

                    <Button
                        startIcon={<DeleteOutlineIcon />}
                        variant="contained"
                        color="secondary"
                        size="small"
                        className={cls.feeTableButton}
                        onClick={() => tableModifier.delTable(staticTableMetas, staticTableMeta.tableOrder)}
                    >
                        Remove Table
                    </Button>
                    <FormControlLabel
                        label="Show Totals"
                        control={<Checkbox checked={staticTableMeta.includeTotals} onChange={() => tableModifier.toggleShowTotals(staticTableMetas, staticTableMeta.tableOrder)} />}
                    />
                    {anyStaticTableRowsWithPerIndividualSet(staticTableMeta) && (
                        <FormControlLabel
                            label="Require num individuals"
                            control={<Checkbox checked={staticTableMeta.perPersonInputRequired} onChange={() => tableModifier.togglePerPersonRequired(staticTableMetas, staticTableMeta.tableOrder)} />}
                        />
                    )}
                </div>
            </div>
        </Fade>
    );
};
