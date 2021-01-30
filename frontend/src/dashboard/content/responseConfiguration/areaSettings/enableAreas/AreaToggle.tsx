import { IOSSwitch } from '@common/components/IOSSwitch'
import { FormControlLabel } from '@material-ui/core'
import { AlignCenter } from 'dashboard/layouts/positioning/AlignCenter'
import React from 'react'

interface AreaToggleProps {
    isComplete: boolean;
    onChange(): void;
}

export const AreaToggle = ({isComplete, onChange}: AreaToggleProps) => {
    return (
        <AlignCenter>
            <FormControlLabel
                    control={<IOSSwitch disabled={isComplete === null} checked={isComplete === true} onChange={onChange} name="Active" />}
                    style={{ color: "black", fontWeight: "bolder", paddingBottom: "0.8rem" }}
                    label={isComplete === null ? "loading..." : isComplete === true ? "Area Enabled" : "Area Disabled"}
                />
        </AlignCenter>
    )
}