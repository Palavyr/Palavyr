import * as React from "react";
import { ConvoTableRow } from "../../types";
import { getChildNodes } from "../utils";
import { TableRow, TableCell, Table, makeStyles, Typography } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { useState } from "react";
import { sortChildrenByOptions } from "src/common/sorting";

const useStyles = makeStyles(() => ({
    table: {
        borderBottom: "none",
    },
}));

export const makeMultipleChoiceAsPathButtons = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    const children = getChildNodes(node.nodeChildrenString, nodeList);
    const sortedChildren = sortChildrenByOptions(children);

    const Component: React.ElementType<{}> = () => {
        const cls = useStyles();
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <>
                <Typography variant="body2">{node.text}</Typography>
                <Table>
                    <TableRow>
                        {sortedChildren.map((child: ConvoTableRow) => {
                            return (
                                <TableCell className={cls.table}>
                                    <ResponseButton
                                        disabled={disabled}
                                        key={child.id}
                                        text={child.optionPath}
                                        onClick={() => {
                                            var response = child.optionPath;
                                            responseAction(node, child, nodeList, client, convoId, response);
                                            setDisabled(true);
                                        }}
                                    />
                                </TableCell>
                            );
                        })}
                    </TableRow>
                </Table>
            </>
        );
    };
    return Component;
};
