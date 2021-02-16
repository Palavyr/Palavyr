import * as React from "react";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { uuid } from "uuidv4";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useState } from "react";

const useStyles = makeStyles(theme => ({
    table: {
        borderBottom: "none",
    },
}));

export const makeMultipleChoiceContinueButtons = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    const child = getChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
    const valueOptions = node.valueOptions.split(",");
    const Component: React.ElementType<{}> = () => {
        const cls = useStyles();
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <TableRow>
                    {valueOptions.map((valueOption: string) => {
                        return (
                            <TableCell className={cls.table}>
                                <ResponseButton
                                    disabled={disabled}
                                    key={valueOption + "-" + uuid()}
                                    text={valueOption}
                                    onClick={() => {
                                        const response = valueOption;
                                        responseAction(node, child, nodeList, client, convoId, response);
                                        setDisabled(true);
                                    }}
                                />
                            </TableCell>
                        );
                    })}
                </TableRow>
            </Table>
        );
    };
    return Component;
};
