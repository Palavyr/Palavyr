import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { makeStyles, TextField } from "@material-ui/core";
import { ConvoNode } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { Align } from "dashboard/layouts/positioning/Align";
import React, { useContext, useState } from "react";

export interface UseLinkProps {
    node: ConvoNode;
}

const useStyles = makeStyles((theme) => ({
    textField: {
        margin: "0.5rem",
    },
}));

export const UseLink = ({ node }: UseLinkProps) => {
    const repository = new PalavyrRepository();

    const { setIsLoading, setSuccessOpen, setSuccessText } = useContext(DashboardContext);

    const cls = useStyles();
    const [url, setText] = useState<string>("");
    const onTextChange = (event) => {
        const text = event.target.value;
        setText(text);
    };

    const onLinkSubmit = async () => {

        // const urlIsValid = checkUrlIsValid(url);
        // if (urlIsValid) {
        //     const fileLink = await repository.Configuration.Images.saveImageUrl(url, node.nodeId);
        // } else {

        // }

    };

    return (
        <PalavyrAccordian title="Submit a Url" initialState={false}>
            <Align>
                <form style={{ width: "100%" }} onSubmit={onLinkSubmit}>
                    <TextField className={cls.textField} fullWidth value={url} type="text" onChange={onTextChange} />
                    <Align direction="flex-start">
                        <SaveOrCancel buttonType="submit" saveText="Submit Url" />
                    </Align>
                </form>
            </Align>
        </PalavyrAccordian>
    );
};
