import { IOSSwitch } from "@common/components/IOSSwitch";
import { FormControlLabel } from "@material-ui/core";
import { Align } from "@common/positioning/Align";
import React from "react";

interface ToggleProps {
    controlledState: boolean;
    onChange(): void;
    enabledLabel: string;
    disabledLabel: string;
    disabled?: boolean;
}

export const OsTypeToggle = ({ controlledState, onChange, enabledLabel, disabledLabel, disabled }: ToggleProps) => {
    return (
        <Align>
            <FormControlLabel
                disabled={disabled}
                control={<IOSSwitch disabled={controlledState === null} checked={controlledState === true} onChange={onChange} name="Active" />}
                style={{ color: "black", fontWeight: "bolder", paddingBottom: "0.8rem" }}
                label={controlledState === null ? "loading..." : controlledState === true ? enabledLabel : disabledLabel}
            />
        </Align>
    );
};
