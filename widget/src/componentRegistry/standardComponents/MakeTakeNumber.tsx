import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { TextField, Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction } from "..";
import { getChildNodes } from "../utils";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeTakeNumber = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

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
                                setResponse(event.target.value);
                            }}
                        />
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell className={classes.root} align="right">
                        <ResponseButton
                            disabled={disabled}
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, response);
                                toggleInputDisabled();
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
