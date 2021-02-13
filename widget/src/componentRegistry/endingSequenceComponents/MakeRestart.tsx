import * as React from "react";
import { IProgressTheChat } from "..";
import { TableCell, Table, TableRow } from "@material-ui/core";
import { ResponseButton } from "../../common/ResponseButton";

export const makeRestart = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    const Component: React.ElementType<{}> = () => {
        const noBorder = { borderBottom: "none" };
        return (
            <Table>
                <TableRow>
                    <TableCell>{node.text}</TableCell>
                </TableRow>
                <TableRow>
                    <TableCell style={noBorder} align="right">
                        <ResponseButton
                            text="restart"
                            onClick={() => {
                                window.location.reload();
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return Component;
};
