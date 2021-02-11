import { IOSSwitch } from '@common/components/IOSSwitch'
import { FormControlLabel } from '@material-ui/core'
import { AlignCenter } from 'dashboard/layouts/positioning/AlignCenter'
import React from 'react'

interface ToggleProps {
    controlledState: boolean;
    onChange(): void;
    enabledLabel: string;
    disabledLabel: string;
}

export const OsTypeToggle = ({controlledState, onChange, enabledLabel, disabledLabel}: ToggleProps) => {
    return (
        <AlignCenter>
            <FormControlLabel
                    control={<IOSSwitch disabled={controlledState === null} checked={controlledState === true} onChange={onChange} name="Active" />}
                    style={{ color: "black", fontWeight: "bolder", paddingBottom: "0.8rem" }}
                    label={controlledState === null ? "loading..." : controlledState === true ? enabledLabel : disabledLabel}
                />
        </AlignCenter>
    )
}