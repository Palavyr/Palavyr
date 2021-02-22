import * as React from "react";
import { setNumIndividualsContext } from "src/widgetCore/store/dispatcher";
import { TextField, Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction } from "..";
import { getChildNodes } from "../utils";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { parseInt } from "lodash";

export const makeTakeNumberIndividuals = ({ node, nodeList, client, convoId }: IProgressTheChat) => {

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const useStyles = makeStyles(() => ({
        root: {
            borderBottom: "0px solid white",
        },
    }));

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<number>(null);
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
                            value={response}
                            type="number"
                            onChange={event => {
                                const intValue = parseInt(event.target.value);
                                if (!intValue) return;
                                if (intValue < 0) return;
                                setResponse(intValue);
                            }}
                        />
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell className={classes.root} align="right">
                        <ResponseButton
                            disabled={disabled}
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, response.toString());
                                setNumIndividualsContext(response)
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
