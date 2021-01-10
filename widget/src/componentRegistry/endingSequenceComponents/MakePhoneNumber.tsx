import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { Table, TableRow, TableCell, FormControlLabel } from "@material-ui/core";
import { useState } from "react";
import { getChildNodes } from "../utils";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import NumberFormat from "react-number-format";
import { ResponseButton } from "../../common/ResponseButton";
import { IOSSwitch } from "../../common/IOSStyleSwitch";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makePhoneNumber = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const region = convoContext.region;
        const [phoneNumberResponse, setResponse] = useState<string>("");
        const [regionSwitch, setRegionSwitch] = useState<boolean>(region === "AU" || region === undefined ? true : false);
        const [disabled, setDisabled] = useState<boolean>(false);

        const noBorder = { borderBottom: "none", padding: "8px" };

        return (
            <>
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell style={noBorder}>
                            {regionSwitch ? "AU  " : "US  "}
                            <NumberFormat
                                disabled={disabled}
                                format={regionSwitch ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                                mask="_"
                                type="tel"
                                onValueChange={values => {
                                    setResponse(values.formattedValue);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
                <Table>
                    <TableRow>
                        <TableCell style={noBorder}>
                            <FormControlLabel control={<IOSSwitch disabled={disabled} checked={regionSwitch} onChange={() => setRegionSwitch(!regionSwitch)} name="Region" />} label="Region" />
                        </TableCell>
                        <TableCell style={noBorder}>
                            <ResponseButton
                                disabled={disabled}
                                onClick={e => {
                                    e.preventDefault();
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ "Phone Number": phoneNumberResponse });
                                    }
                                    convoContext["PhoneNumber"] = phoneNumberResponse;
                                    responseAction(node, child, nodeList, client, convoId, phoneNumberResponse, convoContext);
                                    toggleInputDisabled();
                                    setDisabled(true);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </>
        );
    };
    return Component;
};
