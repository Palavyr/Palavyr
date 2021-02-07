import * as React from "react";
import { ConvoTableRow } from "../../types";
import { toggleInputDisabled, toggleMsgLoader } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { TableRow, TableCell, Table, makeStyles, Typography } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useState } from "react";

const sortChildrenByOptions = (children: ConvoTableRow[]) => {
    return children.sort((a, b) => {
        if (a.optionPath == null || b.optionPath == null) {
            return 0;
        }
        var nameA = a.optionPath.toUpperCase(); // ignore upper and lowercase
        var nameB = b.optionPath.toUpperCase(); // ignore upper and lowercase
        if (nameA < nameB) {
            return -1;
        }
        if (nameA > nameB) {
            return 1;
        }
        // names must be equal
        return 0;
    });
};

const useStyles = makeStyles(theme => ({
    table: {
        borderBottom: "none",
    },
}));

export const makeMultipleChoiceAsPathButtons = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

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
                                            responseAction(node, child, nodeList, client, convoId, response, convoContext);
                                            toggleInputDisabled();
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
