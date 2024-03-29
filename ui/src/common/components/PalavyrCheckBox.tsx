import { makeStyles } from "@material-ui/core";
import React from "react";
import { FormControlLabel, Checkbox } from "@material-ui/core";
import { v4 as uuid } from 'uuid';

type StyleProps = {
    checked: boolean;
};

const useStyles = makeStyles<{}>((theme: any) => ({
    formstyle: {
        fontSize: "12px",
        alignSelf: "bottom",
    },
    formLabelStyle: (props: StyleProps) => ({
        fontSize: "12px",
        color: props.checked ? "black" : "gray",
    }),
}));

export interface PalavyrCheckxoxProps {
    label: string;
    checked: boolean;
    onChange: (event: { target: { checked: boolean } }) => void;
    disabled?: boolean;
}

export const PalavyrCheckbox = ({ label, checked, onChange, disabled }: PalavyrCheckxoxProps) => {
    const cls = useStyles({ checked });
    const uniqueName = uuid();
    return (
        <FormControlLabel
            className={cls.formstyle}
            classes={{
                label: cls.formLabelStyle,
            }}
            control={<Checkbox disabled={disabled} className={cls.formstyle} size="small" checked={checked} value="" name={"crit-" + uniqueName + "-merge"} onChange={onChange} />}
            label={label}
        />
    );
};
