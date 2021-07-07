import { Tooltip } from "@material-ui/core";
import { NodeTypeOptions, SetState } from "@Palavyr-Types";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useState } from "react";
import { useEffect } from "react";
import { PalavyrCheckbox } from "../../../../../../common/components/PalavyrCheckBox";
import { IPalavyrNode, NodeOptionalProps } from "../../Contracts";
import AnabranchConfigurer from "../actions/AnabranchConfigurer";

const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>, node: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) => {
    const checked = event.target.checked;
    AnabranchConfigurer.SetAnabranchCheckBox(checked, setAnabranchMergeChecked, node, nodeTypeOptions);
};

export const AnabranchMergeCheckBox = ({ node }: NodeOptionalProps) => {
    const disabled = node.isPalavyrAnabranchStart && node.isPalavyrAnabranchEnd;
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);
    const { nodeTypeOptions } = useContext(ConversationTreeContext);

    useEffect(() => {
        setAnabranchMergeChecked(node.isAnabranchMergePoint);
    }, []);

    return AnabranchConfigurer.shouldShowAnabranchCheckBox(node) ? (
        <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
            <span>
                <PalavyrCheckbox
                    disabled={disabled}
                    label="Set as Anabranch merge point"
                    checked={anabranchMergeChecked}
                    onChange={(event) => onChange(event, setAnabranchMergeChecked, node, nodeTypeOptions)}
                />
            </span>
        </Tooltip>
    ) : (
        <></>
    );
};
