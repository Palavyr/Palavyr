import React from "react";
import { OsTypeToggle } from "@frontend/dashboard/content/responseConfiguration/areaSettings/enableAreas/OsTypeToggle";

export const TableButton = ({ onMessage = "Range", offMessage="Value", state, onClick, disabled }: TableButtonProps) => {
    return <OsTypeToggle controlledState={state} onChange={onClick} enabledLabel={onMessage} disabledLabel={offMessage} disabled={disabled} />;
};

export interface TableButtonProps {
    state: boolean;
    onClick: () => void;
    onMessage?: string;
    offMessage?: string;
    disabled?: boolean
}
