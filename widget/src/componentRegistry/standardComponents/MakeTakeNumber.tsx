import * as React from "react";
import { TextField, Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, NodeTypes, responseAction } from "..";
import { getChildNodes } from "../utils";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeTakeNumber = ({ node, nodeList, client, convoId }: IProgressTheChat) => {

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const useStyles = makeStyles(() => ({
        root: {
            borderBottom: "0px solid white",
        },
    }));

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);

        const classes = useStyles();

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <TableRow>
                    <TableCell className={classes.root}>
                        <TextField
                            disabled={disabled}
                            label=""
                            type="number"
                            onChange={event => {
                                const intValue = parseInt(event.target.value);
                                if (!intValue) return;
                                if (intValue < 0) return;
                                setResponse(intValue.toString());                            }}
                        />
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell className={classes.root} align="right">
                        <ResponseButton
                            disabled={disabled}
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, response);
                                setDisabled(true);
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return Component;
};
