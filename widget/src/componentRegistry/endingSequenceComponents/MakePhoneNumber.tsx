import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'src/widgetCore/store/dispatcher';
import { Table, TableRow, TableCell, FormControlLabel } from '@material-ui/core';
import { useState } from 'react';
import { getChildNodes } from '../utils';
import { responseAction, IProgressTheChat, ConvoContextProperties } from '..';
import NumberFormat from 'react-number-format';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';
import { IOSSwitch } from '../../common/IOSStyleSwitch';


export const makePhoneNumber = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {

        const region = convoContext.region;
        const [phoneNumberResponse, setResponse] = useState<string>("");
        const [regionSwitch, setRegionSwitch] = useState<boolean>(region === "AU" || region === undefined ? true : false);
        const noBorder = { borderBottom: "none", padding: "8px"};

        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell>
                            {node.text}
                        </TableCell>
                    </TableRow>
                    <TableRow >
                        <TableCell style={noBorder}>
                            {
                                regionSwitch ? "AU  " : "US  "
                            }
                            <NumberFormat
                                format={regionSwitch ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                                mask="_"
                                type="tel"
                                onValueChange={(values) => {
                                    setResponse(values.formattedValue)
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
                <Table>
                    <TableRow >
                        <TableCell style={noBorder}>
                            <FormControlLabel
                                control={<IOSSwitch checked={regionSwitch} onChange={() => setRegionSwitch(!regionSwitch)} name="Region" />}
                                label="Region"
                            />
                        </TableCell>
                        <TableCell style={noBorder}>
                            <ResponseButton
                                onClick={
                                    (e) => {

                                        e.preventDefault();
                                        if (node.isCritical) {
                                            convoContext[ConvoContextProperties.KeyValues].push({ "Phone Number": phoneNumberResponse })
                                        }
                                        convoContext["PhoneNumber"] = phoneNumberResponse;
                                        responseAction(node, child, nodeList, client, convoId, phoneNumberResponse, convoContext)
                                        toggleInputDisabled()
                                    }
                                }
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper >
        )
    }
    return Component;
}